namespace Sales.Domain.Models
{
    public class QuoteRequest : Aggregate<QuoteRequestId>
    {
        public CustomerId CustomerId { get; private set; } = default!;
        public SystemModelId SystemModelId { get; private set; } = default!;
        public QuoteRequestStatus Status { get; private set; } = QuoteRequestStatus.Pending;
        public string? CustomConfig { get; private set; } = default!;

        public static QuoteRequest Create(QuoteRequestId id, CustomerId customerId, SystemModelId systemModelId, string? customConfig = null)
        {   
            var quoteRequest = new QuoteRequest
            {
                Id = id,
                CustomerId = customerId,
                SystemModelId = systemModelId,
                CustomConfig = customConfig,
                Status = QuoteRequestStatus.Pending
            };

            quoteRequest.AddDomainEvent(new QuoteRequestCreatedEvent(quoteRequest));

            return quoteRequest;
        }

    }
}
