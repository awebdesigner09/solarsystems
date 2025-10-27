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
                CustomConfig = quoteRequest.CustomConfig
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
                customConfig: dto.CustomConfig);

            return newQuoteRequest;
        }
         
    }
}
