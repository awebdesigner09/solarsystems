namespace Sales.Application.Dtos
{
    public record QuoteDto(
        Guid? Id,
        Guid QuoteRequestId,
        DateTime? IssuedOn,
        DateTime? ValidUntil,
        ComponentsDto? Components,
        decimal? BasePrice,
        decimal? Tax1,
        decimal? Tax2,
        decimal? TotalPrice
        );
}
