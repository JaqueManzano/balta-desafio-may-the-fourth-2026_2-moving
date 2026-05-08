using Microsoft.Extensions.DependencyInjection;
using Moving.Core.Agents.Abstractions;
using Moving.Core.Enums;
using Moving.Core.Models;
using Moving.Core.Services;
using Moving.Core.Repositories.Abstractions;

namespace Moving.Infra.Services
{
    public class ItemLocatorService(
        IStorageBoxRepository storageBoxRepository,
        [FromKeyedServices(AgentType.ItemLocatorAgent)]
        IAgent<string, string> itemLocatorAgent) : IItemLocatorService
    {
        public async Task<string> LocateItemAsync(string search, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(search))
                return "Please tell me the name of the item so I can find the box.";

            var boxes = await storageBoxRepository.GetAllAsync(cancellationToken);
            if (boxes.Count == 0)
                return "There are currently no boxes registered for search.";

            var inventoryContext = BuildInventoryContext(boxes);
            var aiInput = $"""
                USER_SEARCH:
                {search.Trim()}

                INVENTORY:
                {inventoryContext}
                """;

            var aiResult = await RunAgentSafelyAsync(aiInput, cancellationToken);
            if (string.IsNullOrWhiteSpace(aiResult))
                return "We were unable to process your search at this time. Please try again in a few moments.";

            return aiResult.Trim();
        }

        private async Task<string?> RunAgentSafelyAsync(string input, CancellationToken cancellationToken)
        {
            try
            {
                return await itemLocatorAgent.RunAsync(input, cancellationToken);
            }
            catch
            {
                return null;
            }
        }

        private static string BuildInventoryContext(IEnumerable<StorageBox> boxes)
        {
            return string.Join(
                Environment.NewLine,
                boxes.Select(box =>
                {
                    var items = box.Items.Count != 0
                        ? string.Join("; ", box.Items.Select(item =>
                            $"nome={item.ItemName}, descricao={item.ItemDescription ?? "n/a"}, keywords={item.Keywords ?? "n/a"}, qtd={item.Quantity}"))
                        : "sem itens";

                    return $"caixa={box.Name} | descricao={box.Description ?? "n/a"} | itens=[{items}]";
                }));
        }
    }
}
