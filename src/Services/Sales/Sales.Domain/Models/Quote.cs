namespace Sales.Domain.Models
{
    public class Quote: Aggregate<QuoteId>
    {
        public QuoteRequestId QuoteRequestId { get; private set; } = default!;
        public DateTime IssuedOn { get; private set; } = DateTime.UtcNow;
        public DateTime ValidUntil { get; private set; } = default!;
        public Components Components { get; private set; } = default!;
        public decimal BasePrice { get; private set; } = default!;
        public decimal Tax1 { get; private set; } = default!;
        public decimal Tax2 { get; private set; } = default!;
        public decimal TotalPrice { get; private set; } = default!;

        public static Quote Create(QuoteId id, QuoteRequestId quoteRequestId, DateTime validUntil,
            Components components, decimal basePrice, decimal tax1, decimal tax2, decimal totalPrice)
        {
            if (validUntil <= DateTime.UtcNow)
            {
                throw new ArgumentException("Valid until date must be in the future.");
            }
            if (basePrice < 0)
            {
                throw new ArgumentException("Base price cannot be negative.");
            }
            if (tax1 < 0)
            {
                throw new ArgumentException("Tax1 cannot be negative.");
            }
            if (tax2 < 0)
            {
                throw new ArgumentException("Tax2 cannot be negative.");
            }
            if (totalPrice < 0)
            {
                throw new ArgumentException("Total price cannot be negative.");
            }
            var quote = new Quote
            {
                Id = id,
                QuoteRequestId = quoteRequestId,
                ValidUntil = validUntil,
                Components = components,
                BasePrice = basePrice,
                Tax1 = tax1,
                Tax2 = tax2,
                TotalPrice = totalPrice
            };
            return quote;
        }
    }
}
