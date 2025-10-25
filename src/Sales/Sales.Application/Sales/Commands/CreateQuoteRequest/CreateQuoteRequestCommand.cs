using FluentValidation;

namespace Sales.Application.Sales.Commands.CreateQuoteRequest
{
    public record CreateQuoteRequestCommand(QuoteRequestDto QuoteRequest) : ICommand<CreateQuoteRequestResult>;
    public record CreateQuoteRequestResult(Guid Id);
    public class CreateQuoteRequestValidator : AbstractValidator<CreateQuoteRequestCommand>
    {
        public CreateQuoteRequestValidator()
        {
            RuleFor(x => x.QuoteRequest).NotNull();
            RuleFor(x => x.QuoteRequest.CustomerId).NotNull().WithMessage("CustomerId is required.");
            RuleFor(x => x.QuoteRequest.SystemModelId).NotNull().WithMessage("SystemModelId is required.");
        }
    }
}
