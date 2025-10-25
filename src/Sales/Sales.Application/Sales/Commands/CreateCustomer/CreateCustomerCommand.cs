using FluentValidation;

namespace Sales.Application.Sales.Commands.CreateCustomer
{
  public record CreateCustomerCommand(CustomerDto Customer) : ICommand<CreateCustomerResult>;
    public record CreateCustomerResult(Guid Id);
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.Customer).NotNull();
            RuleFor(x => x.Customer.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.Customer.Name).MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
            RuleFor(x => x.Customer.Email).NotEmpty().WithMessage("Email cannot be empty").EmailAddress().WithMessage("Invalid email format");
            RuleFor(x => x.Customer.Address).NotNull().WithMessage("Address is required.");
            RuleFor(x => x.Customer.Address.AddressLine1).NotEmpty().WithMessage("Address Line 1 cannot be empty");
            RuleFor(x => x.Customer.Address.City).NotEmpty().WithMessage("City cannot be empty");
            RuleFor(x => x.Customer.Address.State).NotEmpty().WithMessage("State cannot be empty");
            RuleFor(x => x.Customer.Address.ZipCode).NotEmpty().WithMessage("Zip Code cannot be empty");

        }
    }
}
