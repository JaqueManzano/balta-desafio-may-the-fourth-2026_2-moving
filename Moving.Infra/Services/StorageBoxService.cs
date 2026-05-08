using Moving.Core.Models;
using Moving.Core.Repositories.Abstractions;
using Moving.Core.Services;

namespace Moving.Infra.Services
{
    public class StorageBoxService : IStorageBoxService
    {
        private readonly IStorageBoxRepository _storageBoxRepository;

        public StorageBoxService(IStorageBoxRepository storageBoxRepository)
        {
            _storageBoxRepository = storageBoxRepository;
        }

        public Task<StorageBox> CreateAsync(StorageBox box)
        {
            return _storageBoxRepository.CreateAsync(box, CancellationToken.None);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _storageBoxRepository.DeleteAsync(id, CancellationToken.None);
        }

        public async Task<IEnumerable<StorageBox>> GetAllAsync()
        {
            return await _storageBoxRepository.GetAllAsync(CancellationToken.None);
        }

        public Task<StorageBox?> GetByIdAsync(Guid id)
        {
            return _storageBoxRepository.GetByIdAsync(id, CancellationToken.None);
        }

        public async Task UpdateAsync(StorageBox box)
        {
            await _storageBoxRepository.UpdateAsync(box, CancellationToken.None);
        }

        public async Task<StorageBox?> GetByDescriptionAsync(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return null;

            var boxes = await _storageBoxRepository.GetAllAsync(CancellationToken.None);
            return boxes.FirstOrDefault(box =>
                (box.Description?.Contains(description, StringComparison.OrdinalIgnoreCase) ?? false));
        }
    }
}
