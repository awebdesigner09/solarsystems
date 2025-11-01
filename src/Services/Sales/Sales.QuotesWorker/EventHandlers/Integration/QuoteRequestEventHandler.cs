using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Sales.Application.Dtos;
using Sales.Domain.Enums;
using Sales.QuotesWorker.Commands.CreateQuote;

namespace Sales.QuotesWorker.EventHandlers.Integration;
public class QuoteRequestEventHandler
    (ISender sender, ILogger<QuoteRequestEventHandler> logger)
    : IConsumer<QuoteRequestEvent>
{
    public async Task Consume(ConsumeContext<QuoteRequestEvent> context)
    {
        
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);
        // Create Quote
        var command = MapToCreateOrderCommand(context.Message.QuoteRequestId,context.Message.CustomerId, context.Message.SystemModelId, 
            context.Message.InstallAddress1, context.Message.InstallAddress2, context.Message.City, context.Message.State, context.Message.PostalCode, context.Message.Country,
            context.Message.optBattery, context.Message.optEVCharger, context.Message.addtionalNotes);
        await sender.Send(command);
    }

    private CreateQuoteCommand MapToCreateOrderCommand(Guid quoteRequestId, Guid customerId, Guid systemModelId, 
        string address1, string? address2, string city, string state, string postalCode, string country, 
        bool optBattery, bool optEVCharger, string? additionalNotes)
    {
        var quoteRequestDto = new QuoteRequestDto(
            quoteRequestId,
            customerId,
            systemModelId,
            QuoteRequestStatus.Processing,
            new AddressDto(address1, address2, city, state, postalCode, country),
            new QuoteCustomOptionsDto(optBattery, optEVCharger),
            additionalNotes
        );
        return new CreateQuoteCommand(quoteRequestDto);
    }
}
