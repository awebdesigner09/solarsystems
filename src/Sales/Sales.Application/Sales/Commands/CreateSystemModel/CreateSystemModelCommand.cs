using FluentValidation;
namespace Sales.Application.Sales.Commands.CreateSystemModel
{
    public record CreateSystemModelCommand(SystemModelDto SystemModel) : ICommand<CreateSystemModelResult>;
    public record CreateSystemModelResult(Guid Id);
    public class CreateSystemModelValidator : AbstractValidator<CreateSystemModelCommand>
    {
        public CreateSystemModelValidator()
        {
            RuleFor(x => x.SystemModel).NotNull();
            RuleFor(x => x.SystemModel.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.SystemModel.Name).MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
            RuleFor(x => x.SystemModel.PanelType).NotEmpty().WithMessage("PanelType cannot be empty").MaximumLength(50).WithMessage("PanelType cannot exceed 50 characters");
            RuleFor(x => x.SystemModel.BasePrice).GreaterThan(0).WithMessage("BasePrice must be greater than 0");
            RuleFor(x => x.SystemModel.CapacityKW).GreaterThan(0).WithMessage("CapacityKW must be greater than 0");
        }
    }
}
