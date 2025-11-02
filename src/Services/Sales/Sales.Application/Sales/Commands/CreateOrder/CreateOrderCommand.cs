using FluentValidation;

namespace Sales.Application.Sales.Commands.CreateOrder
{
    public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;
    public record CreateOrderResult(Guid Id);
    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.Order).NotNull();
            RuleFor(x => x.Order.QuoteId).NotNull().WithMessage("QuoteId is required.");
        }
    }
}
