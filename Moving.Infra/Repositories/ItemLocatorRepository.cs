using Moving.Core.Models;
using Moving.Core.Repositories.Abstractions;

namespace Moving.Infra.Repositories
{
    public class ItemLocatorRepository : IItemLocatorRepository
    {
        private readonly IStorageBoxRepository _storageBoxRepository;
        private static readonly char[] TokenSeparators = [' ', ',', '.', ';', ':', '-', '_', '/', '\\', '|', '\t', '\n', '\r'];
        private static readonly HashSet<string> StopWords = new(StringComparer.OrdinalIgnoreCase)
        {
            "onde", "esta", "está", "fica", "ficou", "procura", "procurar", "quero",
            "achar", "encontrar", "item", "caixa", "o", "a", "os", "as", "de", "do",
            "da", "dos", "das", "um", "uma", "uns", "umas", "meu", "minha", "meus", "minhas"
        };

        public ItemLocatorRepository(IStorageBoxRepository storageBoxRepository)
        {
            _storageBoxRepository = storageBoxRepository;
        }

        public Task<StorageBox?> LocateItemAsync(string itemDescription, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(itemDescription))
                return Task.FromResult<StorageBox?>(null);

            return FindStorageBoxByItemAsync(itemDescription, cancellationToken);
        }

        private async Task<StorageBox?> FindStorageBoxByItemAsync(string itemDescription, CancellationToken cancellationToken)
        {
            var boxes = await _storageBoxRepository.GetAllAsync(cancellationToken);
            var normalizedInput = itemDescription.Trim();
            var tokens = ExtractRelevantTokens(normalizedInput);

            var bestMatch = boxes
                .Select(box => new
                {
                    Box = box,
                    Score = box.Items.Max(item => ScoreItem(item, normalizedInput, tokens))
                })
                .Where(candidate => candidate.Score > 0)
                .OrderByDescending(candidate => candidate.Score)
                .FirstOrDefault();

            return bestMatch?.Box;
        }

        private static int ScoreItem(StoredItem item, string normalizedInput, IReadOnlyCollection<string> tokens)
        {
            var score = 0;

            score += ScoreByField(item.ItemName, normalizedInput, tokens, 30, 10);
            score += ScoreByField(item.ItemDescription, normalizedInput, tokens, 20, 6);
            score += ScoreByField(item.Keywords, normalizedInput, tokens, 15, 5);

            return score;
        }

        private static int ScoreByField(string? fieldValue, string fullInput, IReadOnlyCollection<string> tokens, int fullMatchScore, int tokenMatchScore)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
                return 0;

            var score = 0;
            if (fieldValue.Contains(fullInput, StringComparison.OrdinalIgnoreCase))
                score += fullMatchScore;

            foreach (var token in tokens)
            {
                if (fieldValue.Contains(token, StringComparison.OrdinalIgnoreCase))
                    score += tokenMatchScore;
            }

            return score;
        }

        private static IReadOnlyCollection<string> ExtractRelevantTokens(string text)
        {
            return text
                .Split(TokenSeparators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(static token => token.ToLowerInvariant())
                .Where(token => token.Length >= 3)
                .Where(token => !StopWords.Contains(token))
                .Distinct(StringComparer.Ordinal)
                .ToArray();
        }
    }
}
