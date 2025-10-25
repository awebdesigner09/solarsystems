namespace Sales.Application.Dtos
{
    public record AddressDto(
        string AddressLine1,
        string? AddressLine2,
        string City,
        string State,
        string ZipCode,
        string Country);
}
