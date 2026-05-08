using Moving.Core.Models;

namespace Moving.Core.Repositories.Abstractions
{
    public interface IStorageBoxRepository
    {
        Task<IReadOnlyCollection<StorageBox>> GetAllAsync(CancellationToken cancellationToken);
        Task<StorageBox?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<StorageBox> CreateAsync(StorageBox storageBox, CancellationToken cancellationToken);
        Task<StorageBox?> UpdateAsync(StorageBox storageBox, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);

        Task<IReadOnlyList<StoredItem>> GetItemsByBoxIdAsync(Guid boxId, CancellationToken cancellationToken);
        Task<StoredItem?> GetItemByIdAsync(Guid itemId, CancellationToken cancellationToken);
        Task<StoredItem?> AddItemAsync(Guid boxId, StoredItem item, CancellationToken cancellationToken);
        Task<StoredItem?> UpdateItemAsync(StoredItem item, CancellationToken cancellationToken);
        Task<bool> DeleteItemAsync(Guid itemId, CancellationToken cancellationToken);
    }
}
