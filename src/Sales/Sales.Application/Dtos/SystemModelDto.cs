namespace Sales.Application.Dtos
{
    public record SystemModelDto(
        Guid Id,
        string Name,
        string PanelType,
        int CapacityKW,
        decimal BasePrice
    );
}
