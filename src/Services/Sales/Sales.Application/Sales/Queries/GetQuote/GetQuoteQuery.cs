namespace Sales.Application.Sales.Queries.GetQuote
{
    public record GetQuoteQuery(Guid Id) : IQuery<GetQuoteResult>;
    public record GetQuoteResult(QuoteDto Quote);
}
