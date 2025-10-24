using BuildingBlocks.Pagination;

namespace Sales.Application.Sales.Queries.GetSystemModels
{
    public record GetSystemModelsQuery(PaginationRequest PaginationRequest) 
        : IQuery<GetSystemModelsResult>;
    public record GetSystemModelsResult(PaginatedResult<SystemModelDto> SystemModels);
}
