namespace Sales.Application.Sales.Commands.UpdateSystemModel
{
    public class UpdateSysetmModelHandler(IApplicationDbContext dbContext) :
        ICommandHandler<UpdateSystemModelCommand, UpdateSystemModelResult>
    {
        public async Task<UpdateSystemModelResult> Handle(UpdateSystemModelCommand command, CancellationToken cancellationToken)
        {
            var systemModelId = SystemModelId.Of(command.SystemModel.Id);

            var systemModel = await dbContext.SystemModels
                .FindAsync([systemModelId], cancellationToken);

            if (systemModel is null)
            {
                throw new SystemModelNotFoundException(command.SystemModel.Id);
            }
            UpdateSystemModelWithValues(systemModel, command.SystemModel);
            dbContext.SystemModels.Update(systemModel);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new UpdateSystemModelResult(true);
        }
        private void UpdateSystemModelWithValues(SystemModel systemModel, SystemModelDto dto)
        {
            systemModel.Update(
                name: dto.Name,
                panelType: dto.PanelType,
                capacityKW: dto.CapacityKW,
                basePrice: dto.BasePrice,
                description: dto.Description,
                imageUrl: dto.ImageUrl
                );
        }

    }
}
