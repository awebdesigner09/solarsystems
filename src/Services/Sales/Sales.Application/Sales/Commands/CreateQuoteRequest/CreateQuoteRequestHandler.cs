using MassTransit;
using BuildingBlocks.Messaging.Events;

namespace Sales.Application.Sales.Commands.CreateQuoteRequest
{
   public class CreateQuoteRequestHandler(
       IApplicationDbContext dbContext, 
       IPublishEndpoint publishEndpoint) :
        ICommandHandler<CreateQuoteRequestCommand, CreateQuoteRequestResult>
    {
        public async Task<CreateQuoteRequestResult> Handle(CreateQuoteRequestCommand command, CancellationToken cancellationToken)
        {
            // Create new QuoteRequest entity
            // Save to database using context
            // Return result with new entity Id
            var quoteRequest = CreateNewQuoteRequest(command.QuoteRequest);
            dbContext.QuoteRequests.Add(quoteRequest);
            await dbContext.SaveChangesAsync(cancellationToken);
            // Publish integration event
            var quoteRequestEvent = new QuoteRequestEvent
            {
                UserName = "System", // Replace with actual user context if available
                QuoteRequestId = quoteRequest.Id.Value,
                CustomerId = quoteRequest.CustomerId.Value,
                SystemModelId = quoteRequest.SystemModelId.Value,
                CustomConfig = quoteRequest.CustomConfig
            };
            await publishEndpoint.Publish(quoteRequestEvent, cancellationToken);

            return new CreateQuoteRequestResult(quoteRequest.Id.Value);
        }
        private QuoteRequest CreateNewQuoteRequest(QuoteRequestDto dto)
        {
            // Map DTO to Entity
            var newQuoteRequest = QuoteRequest.Create(
                id: QuoteRequestId.Of(Guid.NewGuid()),
                customerId: CustomerId.Of(dto.CustomerId),
                systemModelId: SystemModelId.Of(dto.SystemModelId),
                customConfig: dto.CustomConfig);

            return newQuoteRequest;
        }
         
    }
}
