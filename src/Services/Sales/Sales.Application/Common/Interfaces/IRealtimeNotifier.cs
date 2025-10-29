namespace Sales.Application.Common.Interfaces
{
    public interface IRealtimeNotifier
    {
        Task PublishAsync<T>(string channel, T notification, CancellationToken cancellationToken);
    }
}
