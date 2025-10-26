using BuildingBlocks.Pagination;

namespace Sales.Application.Sales.Queries.GetSystemModels
{
    public class GetSystemModelsHandler(IApplicationDbContext dbContext) 
        : IQueryHandler<GetSystemModelsQuery, GetSystemModelsResult>
    {
        public async Task<GetSystemModelsResult> Handle(GetSystemModelsQuery request, CancellationToken cancellationToken)
        {
            var pageIndex = request.PaginationRequest.PageIndex;
            var pageSize = request.PaginationRequest.PageSize;
            var totalCount = await dbContext.SystemModels.LongCountAsync(cancellationToken);

            var systemModels = await dbContext.SystemModels
                .OrderBy(sm => sm.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

           return new GetSystemModelsResult(
                new PaginatedResult<SystemModelDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    systemModels.ToSystemModelDtoList()));
            
        }
    }
}
