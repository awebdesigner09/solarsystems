namespace BuildingBlocks.Messaging.Events
{
    public record QuoteRequestEvent : IntegrationEvent
    {
        public string UserName { get; set; } = default!;
        public Guid QuoteRequestId { get; set; } = default!;
        public Guid CustomerId { get; set; } = default!;
        public Guid SystemModelId { get; set; } = default!;
        public string? CustomConfig { get; set; } = default!;
    }
}
