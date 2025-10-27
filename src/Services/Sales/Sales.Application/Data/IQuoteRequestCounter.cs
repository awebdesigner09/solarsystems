namespace Sales.Application.Data
{
    public interface IQuoteRequestCounter
    {
        Task<int> GetAsync(string customerId, string systemModelId, CancellationToken cancellationToken = default);
        Task UpAsync(string customerId, string systemModelId, CancellationToken cancellationToken = default);
        Task DownAsync(string customerId, string systemModelId, CancellationToken cancellationToken = default);
        Task DeleteAsync(string customerId, string systemModelId, CancellationToken cancellationToken = default);
    }
}
