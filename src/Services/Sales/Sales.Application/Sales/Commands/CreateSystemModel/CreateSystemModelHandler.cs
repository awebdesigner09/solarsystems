
namespace Sales.Application.Sales.Commands.CreateSystemModel
{
    public class CreateSystemModelHandler(IApplicationDbContext dbContext) :
        ICommandHandler<CreateSystemModelCommand, CreateSystemModelResult>
    {
        public async Task<CreateSystemModelResult> Handle(CreateSystemModelCommand command, CancellationToken cancellationToken)
        {
            // Create new SystemModel entity
            // Save to database using context
            // Return result with new entity Id

            var systemModel = CreateNewSystemModel(command.SystemModel);
            dbContext.SystemModels.Add(systemModel);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new CreateSystemModelResult(systemModel.Id.Value);
        }

        private SystemModel CreateNewSystemModel(SystemModelDto dto)
        {
            // Map DTO to Entity
            var newSystemModel = SystemModel.Create(
                id: SystemModelId.Of(Guid.NewGuid()),
                name: dto.Name,
                panelType: dto.PanelType,
                capacityKW: dto.CapacityKW,
                basePrice: dto.BasePrice,
                description: dto.Description,
                imageUrl: dto.ImageUrl
                );
            return newSystemModel;
        }
    }
}
