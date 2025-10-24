using FluentValidation;
using FluentValidation.AspNetCore;

namespace Sales.Application.Sales.Commands.DeleteSystemModel
{
    public record DeleteSystemModelCommand(Guid SystemModelId) 
        : ICommand<DeleteSystemModelResult>;
    public record DeleteSystemModelResult(bool IsSuccess);
    public class DeleteSystemModelValidator
        : AbstractValidator<DeleteSystemModelCommand>   
    {
        public DeleteSystemModelValidator()
        {
            RuleFor(x => x.SystemModelId)
                .NotEmpty().WithMessage("SystemModelId is required.");
        }
    }
}
