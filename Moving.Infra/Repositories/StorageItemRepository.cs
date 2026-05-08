using Moving.Core.Models;
using Moving.Core.Repositories.Abstractions;

namespace Moving.Infra.Repositories
{
    public class StorageItemRepository
    {
        private readonly IStorageBoxRepository _storageBoxRepository;

        public StorageItemRepository(IStorageBoxRepository storageBoxRepository)
        {
            _storageBoxRepository = storageBoxRepository;
        }

        public Task<IReadOnlyList<StoredItem>> GetBoxByItemDescriptionAsync(string description, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(description))
                return Task.FromResult<IReadOnlyList<StoredItem>>(Array.Empty<StoredItem>());

            return SearchAsync(description, cancellationToken);
        }

        private async Task<IReadOnlyList<StoredItem>> SearchAsync(string description, CancellationToken cancellationToken)
        {
            var boxes = await _storageBoxRepository.GetAllAsync(cancellationToken);
            var items = boxes
                .SelectMany(box => box.Items)
                .Where(item =>
                    item.ItemName.Contains(description, StringComparison.OrdinalIgnoreCase) ||
                    (item.ItemDescription?.Contains(description, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (item.Keywords?.Contains(description, StringComparison.OrdinalIgnoreCase) ?? false))
                .OrderBy(item => item.ItemName)
                .ToList();

            return items;
        }
    }
}
