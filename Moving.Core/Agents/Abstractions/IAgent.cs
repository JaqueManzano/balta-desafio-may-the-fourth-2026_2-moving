namespace Moving.Core.Agents.Abstractions
{
    public interface IAgent<TData, TResponse>
        where TData : class
        where TResponse : class
    {
        Task<TResponse> RunAsync(
            TData data, CancellationToken cancellationToken);
    }
}
