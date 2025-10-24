namespace Sales.Application.Sales.Commands.DeleteSystemModel
{
    public class DeleteSystemModelHandler(IApplicationDbContext dbContext)
        : ICommandHandler<DeleteSystemModelCommand, DeleteSystemModelResult>
    {
        // Delete SystemModel entity from command object.
        // Saves changes to database using context.
        // Returns result indicating success.

        public async Task<DeleteSystemModelResult> Handle(
            DeleteSystemModelCommand command,
            CancellationToken cancellationToken)
        {
            var systemModelId = SystemModelId.Of(command.SystemModelId);

            var systemModel = await dbContext.SystemModels
                .FindAsync([systemModelId], cancellationToken);
            if (systemModel is null)
            {
                throw new SystemModelNotFoundException(command.SystemModelId);
            }
            dbContext.SystemModels.Remove(systemModel);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new DeleteSystemModelResult(true);
        }
    }
}
