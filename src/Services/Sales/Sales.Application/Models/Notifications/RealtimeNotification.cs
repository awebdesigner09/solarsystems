namespace Sales.Application.Models.Notifications
{
    public class RealtimeNotification<T>
    {
        public string EventType { get; }
        public T Payload { get; }

        public RealtimeNotification(string eventType, T payload)
        {
            EventType = eventType;
            Payload = payload;
        }
    }
}
