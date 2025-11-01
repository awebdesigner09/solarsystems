namespace BuildingBlocks.Messaging.Events
{
    public record QuoteRequestEvent : IntegrationEvent
    {
        public Guid QuoteRequestId { get; set; } = default!;
        public Guid CustomerId { get; set; } = default!;
        public Guid SystemModelId { get; set; } = default!;
        public string InstallAddress1 { get; set; } = default!;
        public string? InstallAddress2 { get; set;} = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string PostalCode { get; set; } = default!;
        public string Country { get; set; } = default!;
        public bool optBattery { get; set; } = default!;
        public bool optEVCharger { get; set; } = default!;
        public string? addtionalNotes { get; set; } = default!;
    }
}
