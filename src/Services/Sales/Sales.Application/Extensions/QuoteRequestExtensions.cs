namespace Sales.Application.Extensions
{
   public static class QuoteRequestExtensions
   {
        public static IEnumerable<QuoteRequestDto> ToQuoteRequestDtoList(this IEnumerable<QuoteRequest> quoteRequests)
        {
            return quoteRequests.Select(qr => qr.ToQuoteRequesDto());
        }
        public static QuoteRequestDto ToQuoteRequesDto(this QuoteRequest quoteRequest)
        {
            return DtoFromQuoteRequest(quoteRequest);
        }
        private static QuoteRequestDto DtoFromQuoteRequest(QuoteRequest quoteRequest)
        {
            return new QuoteRequestDto(
                Id: quoteRequest.Id.Value,
                CustomerId: quoteRequest.CustomerId.Value,
                SystemModelId: quoteRequest.SystemModelId.Value,
                Status: quoteRequest.Status,
                InstallationAddress: new AddressDto(
                    quoteRequest.InstallationAddress.AddressLine1, 
                    quoteRequest.InstallationAddress.AddressLine2, 
                    quoteRequest.InstallationAddress.City,
                    quoteRequest.InstallationAddress.State,
                    quoteRequest.InstallationAddress.PostalCode,
                    quoteRequest.InstallationAddress.Country),
                QuoteCustomOptions: new QuoteCustomOptionsDto(
                    quoteRequest.QuoteCustomOptions.OptBattery,
                    quoteRequest.QuoteCustomOptions.OptEVCharger),
                AdditonalNotes: quoteRequest.AdditonalNotes
            );
        }
    }
}
