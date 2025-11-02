namespace Sales.Application.Dtos
{
    public record OrderDto(
        Guid Id,
        Guid QuoteId
        );
}
