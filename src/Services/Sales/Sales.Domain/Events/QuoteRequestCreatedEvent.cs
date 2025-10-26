namespace Sales.Domain.Events
{
    public record QuoteRequestCreatedEvent(QuoteRequest quoteRequest) : IDomainEvent;
}
