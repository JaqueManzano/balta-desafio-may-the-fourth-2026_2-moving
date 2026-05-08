using Microsoft.Extensions.DependencyInjection;
using Moving.Ai.Models;
using Moving.Ai.Providers.Abstractions;
using Moving.Core.Agents.Abstractions;
using Moving.Core.Enums;
using OllamaSharp;
using OllamaSharp.Models;

namespace Moving.Ai.Agents
{
    public class ItemLocatorAgent : IAgent<string, string>
    {
        private const string AgentName = "ItemLocatorAgent";

        private readonly IPromptProvider _promptProvider;
        private readonly OllamaApiClient _client;
        private const float Temperature = 0.7f;

        public ItemLocatorAgent([FromKeyedServices(PromptProvider.File)] IPromptProvider promptProvider)
        {
            _promptProvider = promptProvider;

            _client = new OllamaApiClient(
                new Uri("http://localhost:11434"),
                AiModels.Phi3Mini);
        }

        public async Task<string> RunAsync(string data, CancellationToken cancellationToken)
        {
            var instructions = await _promptProvider.GetPromptAsync(AgentName, cancellationToken);
            var prompt = $"""
                {instructions}
                User request:
                {data}
                """;

            var finalResponse = string.Empty;

            await foreach (var chunk in _client.GenerateAsync(
                               new GenerateRequest()
                               {
                                   Prompt = prompt,
                                   Options = new RequestOptions
                                   {
                                       Temperature = Temperature
                                   }
                               },
                               cancellationToken))
            {
                if (!string.IsNullOrWhiteSpace(chunk?.Response))
                    finalResponse += chunk.Response;
            }

           return finalResponse;

        }
    }
}
