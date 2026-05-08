using Moving.Ai.Providers.Abstractions;

namespace Moving.Ai.Providers
{
    public class FilePromptProvider : IPromptProvider
    {
        public async Task<string> GetPromptAsync(string agentName, CancellationToken cancellationToken)
        {
            var assembly = typeof(FilePromptProvider).Assembly;

            var resourceName = $"Moving.Ai.Prompts.{agentName}.md";

            await using var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new FileNotFoundException($"Prompt for {agentName} not found: {resourceName}");
        
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync(cancellationToken: cancellationToken);
        }
    }
}
