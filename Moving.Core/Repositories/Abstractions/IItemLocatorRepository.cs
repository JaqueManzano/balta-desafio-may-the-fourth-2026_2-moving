using Moving.Core.Models;

namespace Moving.Core.Repositories.Abstractions
{
    public interface IItemLocatorRepository
    {
        Task<StorageBox?> LocateItemAsync(string itemDescription, CancellationToken cancellationToken);
    }
}
