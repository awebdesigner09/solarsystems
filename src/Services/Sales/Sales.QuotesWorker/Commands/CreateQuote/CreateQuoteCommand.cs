using BuildingBlocks.CQRS;
using FluentValidation;
using Sales.Application.Dtos;

namespace Sales.QuotesWorker.Commands.CreateQuote
{
    public record CreateQuoteCommand(QuoteRequestDto QuoteRequest): ICommand<CreateQuoteResult>;
    public record CreateQuoteResult(Guid QuoteId);
    public class CreateQuoteValidator: AbstractValidator<CreateQuoteCommand>
    {
        public CreateQuoteValidator()
        {
            RuleFor(x => x.QuoteRequest).NotNull();
            RuleFor(x => x.QuoteRequest.CustomerId).NotNull();
            RuleFor(x => x.QuoteRequest.SystemModelId).NotNull();
        }
    }
}
