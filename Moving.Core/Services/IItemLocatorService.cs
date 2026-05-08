using Moving.Core.Models;

namespace Moving.Core.Services
{
    public interface IItemLocatorService
    {
        Task<string> LocateItemAsync(string search, CancellationToken cancellationToken);
    }
}
