using Microsoft.Extensions.DependencyInjection;
using Moving.Ai.Agents;
using Moving.Ai.Providers;
using Moving.Ai.Providers.Abstractions;
using Moving.Core.Agents.Abstractions;
using Moving.Core.Enums;

namespace Moving.Ai
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAgents(this IServiceCollection services)
        {
            services.AddKeyedTransient<IAgent<string, string>, ItemLocatorAgent>(AgentType.ItemLocatorAgent);
            services.AddKeyedTransient<IPromptProvider, FilePromptProvider>(PromptProvider.File);
            return services;
        }
    }
}
