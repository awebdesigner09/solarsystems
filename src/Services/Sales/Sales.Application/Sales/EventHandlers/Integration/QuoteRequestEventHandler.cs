using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Sales.Application.Sales.Commands.CreateQuote;
using Sales.Domain.Enums;

namespace Sales.QuotesWorker.EventHandlers.Integration;
public class QuoteRequestEventHandler
    (ISender sender, ILogger<QuoteRequestEventHandler> logger)
    : IConsumer<QuoteRequestEvent>
{
    public async Task Consume(ConsumeContext<QuoteRequestEvent> context)
    {
        
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);
        // Create Quote
        var command = MapToCreateOrderCommand(context.Message.QuoteRequestId,context.Message.CustomerId, context.Message.SystemModelId,context.Message.CustomConfig);
        await sender.Send(command);
    }

    private CreateQuoteCommand MapToCreateOrderCommand(Guid quoteRequestId, Guid customerId, Guid systemModelId, string? customConfig)
    {
        var quoteRequestDto = new QuoteRequestDto(
            quoteRequestId,
            customerId,
            systemModelId,
            QuoteRequestStatus.Processing,
            customConfig
        );
        return new CreateQuoteCommand(quoteRequestDto);
    }
}
