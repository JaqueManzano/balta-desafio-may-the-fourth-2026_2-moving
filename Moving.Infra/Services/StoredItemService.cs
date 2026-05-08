using Moving.Core.Models;
using Moving.Core.Repositories.Abstractions;
using Moving.Core.Services;

namespace Moving.Infra.Services
{
    public class StoredItemService : IStoredItemService
    {
        private readonly IStorageBoxRepository _storageBoxRepository;

        public StoredItemService(IStorageBoxRepository storageBoxRepository)
        {
            _storageBoxRepository = storageBoxRepository;
        }

        public Task<IReadOnlyList<StoredItem>> GetByBoxIdAsync(Guid boxId)
        {
            return _storageBoxRepository.GetItemsByBoxIdAsync(boxId, CancellationToken.None);
        }

        public Task<StoredItem?> GetByIdAsync(Guid itemId)
        {
            return _storageBoxRepository.GetItemByIdAsync(itemId, CancellationToken.None);
        }

        public Task<StoredItem?> CreateAsync(Guid boxId, StoredItem item)
        {
            return _storageBoxRepository.AddItemAsync(boxId, item, CancellationToken.None);
        }

        public Task<StoredItem?> UpdateAsync(StoredItem item)
        {
            return _storageBoxRepository.UpdateItemAsync(item, CancellationToken.None);
        }

        public Task<bool> DeleteAsync(Guid itemId)
        {
            return _storageBoxRepository.DeleteItemAsync(itemId, CancellationToken.None);
        }
    }
}
