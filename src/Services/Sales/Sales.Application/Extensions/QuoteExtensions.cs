namespace Sales.Application.Extensions
{
    public static class QuoteExtensions
    {
        public static IEnumerable<QuoteDto> ToQuoteDtoList(this IEnumerable<Quote> quotes)
        {
            return quotes.Select(q => q.ToQuoteDto());
        }
        public static QuoteDto ToQuoteDto(this Quote quote)
        {
            return DtoFromQuote(quote);
        }
        private static QuoteDto DtoFromQuote(Quote quote)
        {
            return new QuoteDto(
                Id: quote.Id.Value,
                QuoteRequestId: quote.QuoteRequestId.Value,
                IssuedOn: quote.IssuedOn,
                ValidUntil: quote.ValidUntil,
                Components: new ComponentsDto(
                    NoOfPanels: quote.Components.NoOfPanels,
                    NoOfInverters: quote.Components.NoOfInverters,
                    NoOfMoutingSystems: quote.Components.NoOfMoutingSystems,
                    NoOfBatteries: quote.Components.NoOfBatteries
                    ),
                BasePrice: quote.BasePrice,
                Tax1: quote.Tax1,
                Tax2: quote.Tax2,
                TotalPrice: quote.TotalPrice
                );
        }
    }
}
