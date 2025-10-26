namespace Sales.Application.Dtos
{
    public record CustomerDto(
        Guid Id,
        string Name,
        string Email,
        AddressDto Address);
}
