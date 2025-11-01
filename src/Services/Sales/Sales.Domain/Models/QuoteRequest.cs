using Sales.Domain.ValueObjects;

namespace Sales.Domain.Models
{
    public class QuoteRequest : Aggregate<QuoteRequestId>
    {
        public CustomerId CustomerId { get; private set; } = default!;
        public SystemModelId SystemModelId { get; private set; } = default!;
        public QuoteRequestStatus Status { get; private set; } = QuoteRequestStatus.Pending;
        public Address InstallationAddress { get; private set; } = default!;
        public QuoteCustomOptions QuoteCustomOptions { get; private set; } = default!;
        public string? AdditonalNotes { get; private set; } = default!;

        public static QuoteRequest Create(QuoteRequestId id, CustomerId customerId, SystemModelId systemModelId, Address installationAddress, QuoteCustomOptions quoteCustomOptions, string? additonalNotes = null)
        {   
            var quoteRequest = new QuoteRequest
            {
                Id = id,
                CustomerId = customerId,
                SystemModelId = systemModelId,
                InstallationAddress = installationAddress,
                QuoteCustomOptions = quoteCustomOptions,
                AdditonalNotes = additonalNotes,
                Status = QuoteRequestStatus.Pending
            };

            quoteRequest.AddDomainEvent(new QuoteRequestCreatedEvent(quoteRequest));

            return quoteRequest;
        }

        public void UpdateStatus(QuoteRequestStatus newStatus)
        {
            if (Status != newStatus)
            {
                Status = newStatus;
                //AddDomainEvent(new QuoteRequestStatusUpdatedEvent(this, newStatus));
            }
        }

    }
}
