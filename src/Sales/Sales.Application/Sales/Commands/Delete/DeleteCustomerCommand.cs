using FluentValidation;

namespace Sales.Application.Sales.Commands.Delete
{
    public record DeleteCustomerCommand(Guid CustomerId) 
        : ICommand<DeleteCustomerResult>;
    public record DeleteCustomerResult(bool IsSuccess);
    public class DeleteCustomerValidator
        : AbstractValidator<DeleteCustomerCommand>   
    {
        public DeleteCustomerValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.");
        }
    }
}
