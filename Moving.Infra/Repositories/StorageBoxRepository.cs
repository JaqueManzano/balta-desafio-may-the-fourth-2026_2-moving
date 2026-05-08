using Moving.Core.Models;
using Moving.Core.Repositories.Abstractions;
using System.Collections.Concurrent;

namespace Moving.Infra.Repositories
{
    public class StorageBoxRepository : IStorageBoxRepository
    {
        private static readonly ConcurrentDictionary<Guid, StorageBox> StorageBoxes = new();

        public Task<IReadOnlyCollection<StorageBox>> GetAllAsync(CancellationToken cancellationToken)
        {
            var boxes = StorageBoxes.Values
                .OrderBy(box => box.Name)
                .ToArray();

            return Task.FromResult<IReadOnlyCollection<StorageBox>>(boxes);
        }

        public Task<StorageBox?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            StorageBoxes.TryGetValue(id, out var box);
            return Task.FromResult(box);
        }

        public Task<StorageBox> CreateAsync(StorageBox storageBox, CancellationToken cancellationToken)
        {
            var boxToSave = new StorageBox
            {
                Id = storageBox.Id == Guid.Empty ? Guid.NewGuid() : storageBox.Id,
                Description = storageBox.Description,
                CreatedAt = storageBox.CreatedAt == default ? DateTime.UtcNow : storageBox.CreatedAt,
                Items = storageBox.Items
            };

            StorageBoxes[boxToSave.Id] = boxToSave;
            return Task.FromResult(boxToSave);
        }

        public Task<StorageBox?> UpdateAsync(StorageBox storageBox, CancellationToken cancellationToken)
        {
            if (!StorageBoxes.TryGetValue(storageBox.Id, out var currentBox))
                return Task.FromResult<StorageBox?>(null);

            currentBox.Description = storageBox.Description;
            currentBox.Items = storageBox.Items;

            return Task.FromResult<StorageBox?>(currentBox);
        }

        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var removed = StorageBoxes.TryRemove(id, out _);
            return Task.FromResult(removed);
        }

        public Task<IReadOnlyList<StoredItem>> GetItemsByBoxIdAsync(Guid boxId, CancellationToken cancellationToken)
        {
            if (!StorageBoxes.TryGetValue(boxId, out var box))
                return Task.FromResult<IReadOnlyList<StoredItem>>([]);

            var list = box.Items.OrderBy(i => i.ItemName).ToList();
            return Task.FromResult<IReadOnlyList<StoredItem>>(list);
        }

        public Task<StoredItem?> GetItemByIdAsync(Guid itemId, CancellationToken cancellationToken)
        {
            foreach (var box in StorageBoxes.Values)
            {
                var found = box.Items.FirstOrDefault(i => i.Id == itemId);
                if (found is not null)
                    return Task.FromResult<StoredItem?>(found);
            }

            return Task.FromResult<StoredItem?>(null);
        }

        public Task<StoredItem?> AddItemAsync(Guid boxId, StoredItem item, CancellationToken cancellationToken)
        {
            if (!StorageBoxes.TryGetValue(boxId, out var box))
                return Task.FromResult<StoredItem?>(null);

            var newItem = new StoredItem
            {
                Id = item.Id == Guid.Empty ? Guid.NewGuid() : item.Id,
                StorageBoxId = boxId,
                ItemName = item.ItemName,
                ItemDescription = item.ItemDescription,
                Keywords = item.Keywords,
                Quantity = item.Quantity,
                CreatedAt = item.CreatedAt == default ? DateTime.UtcNow : item.CreatedAt
            };

            box.Items.Add(newItem);
            return Task.FromResult<StoredItem?>(newItem);
        }

        public Task<StoredItem?> UpdateItemAsync(StoredItem item, CancellationToken cancellationToken)
        {
            if (!StorageBoxes.TryGetValue(item.StorageBoxId, out var box))
                return Task.FromResult<StoredItem?>(null);

            var existing = box.Items.FirstOrDefault(i => i.Id == item.Id);
            if (existing is null)
                return Task.FromResult<StoredItem?>(null);

            existing.ItemName = item.ItemName;
            existing.ItemDescription = item.ItemDescription;
            existing.Keywords = item.Keywords;
            existing.Quantity = item.Quantity;

            return Task.FromResult<StoredItem?>(existing);
        }

        public Task<bool> DeleteItemAsync(Guid itemId, CancellationToken cancellationToken)
        {
            foreach (var box in StorageBoxes.Values)
            {
                var item = box.Items.FirstOrDefault(i => i.Id == itemId);
                if (item is null)
                    continue;

                box.Items.Remove(item);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
