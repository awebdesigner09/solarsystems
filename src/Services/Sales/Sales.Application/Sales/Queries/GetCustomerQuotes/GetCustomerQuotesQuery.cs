namespace Sales.Application.Sales.Queries.GetCustomerQuotes
{
    public record GetCustomerQuotesQuery(Guid customerId) : IQuery<GetCustomerQuotesResult>;
    public record GetCustomerQuotesResult(IEnumerable<QuoteDto> Quotes);

}
