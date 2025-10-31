namespace Sales.Application.Dtos
{
    public record SystemModelDto(
        Guid Id,
        string Name,
        string PanelType,
        decimal CapacityKW,
        decimal BasePrice,
        string Description,
        string ImageUrl
    );
}
