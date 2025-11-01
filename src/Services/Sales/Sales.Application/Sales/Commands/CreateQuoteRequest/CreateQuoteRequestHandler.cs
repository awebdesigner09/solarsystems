using MassTransit;
using BuildingBlocks.Messaging.Events;

namespace Sales.Application.Sales.Commands.CreateQuoteRequest
{
   public class CreateQuoteRequestHandler(
       IApplicationDbContext dbContext, 
       IPublishEndpoint publishEndpoint,
       IQuoteRequestCounter counter
       ) :
        ICommandHandler<CreateQuoteRequestCommand, CreateQuoteRequestResult>
    {
        public async Task<CreateQuoteRequestResult> Handle(CreateQuoteRequestCommand command, CancellationToken cancellationToken)
        {  
            // Check if quote request count doesn't exceed 1 per model
           var requestCount = await counter.GetAsync(
                command.QuoteRequest.CustomerId.ToString(), 
                command.QuoteRequest.SystemModelId.ToString(), 
                cancellationToken);

            if (requestCount > 0)
                throw new QuoteRequestLimitExceededException("Customer");

            // Create new QuoteRequest entity
            // Save to database using context
            // Return result with new entity Id
            var quoteRequest = CreateNewQuoteRequest(command.QuoteRequest);
            dbContext.QuoteRequests.Add(quoteRequest);
            await dbContext.SaveChangesAsync(cancellationToken);
            // Publish integration event
            var quoteRequestEvent = new QuoteRequestEvent
            {
                QuoteRequestId = quoteRequest.Id.Value,
                CustomerId = quoteRequest.CustomerId.Value,
                SystemModelId = quoteRequest.SystemModelId.Value,
                InstallAddress1 = quoteRequest.InstallationAddress.AddressLine1,
                InstallAddress2 = quoteRequest.InstallationAddress.AddressLine2,
                City = quoteRequest.InstallationAddress.City,
                State = quoteRequest.InstallationAddress.State,
                PostalCode = quoteRequest.InstallationAddress.PostalCode,
                Country = quoteRequest.InstallationAddress.Country,
                optBattery = quoteRequest.QuoteCustomOptions.OptBattery,
                optEVCharger = quoteRequest.QuoteCustomOptions.OptEVCharger,
                addtionalNotes = quoteRequest.AdditonalNotes
            };
            await publishEndpoint.Publish(quoteRequestEvent, cancellationToken);
            await counter.UpAsync(
                command.QuoteRequest.CustomerId.ToString(),
                command.QuoteRequest.SystemModelId.ToString(),
                cancellationToken);

            return new CreateQuoteRequestResult(quoteRequest.Id.Value);
        }
        private QuoteRequest CreateNewQuoteRequest(QuoteRequestDto dto)
        {
            // Map DTO to Entity
            var newQuoteRequest = QuoteRequest.Create(
                id: QuoteRequestId.Of(Guid.NewGuid()),
                customerId: CustomerId.Of(dto.CustomerId),
                systemModelId: SystemModelId.Of(dto.SystemModelId),
                installationAddress: Address.Of(
                    dto.InstallationAddress.AddressLine1,
                    dto.InstallationAddress.AddressLine2,
                    dto.InstallationAddress.City,
                    dto.InstallationAddress.State,
                    dto.InstallationAddress.PostalCode,
                    dto.InstallationAddress.Country),
                quoteCustomOptions: QuoteCustomOptions.Of(
                    dto.QuoteCustomOptions.OptBattery, 
                    dto.QuoteCustomOptions.OptEVCharger),
                additonalNotes: dto.AdditonalNotes);

            return newQuoteRequest;
        }
         
    }
}
