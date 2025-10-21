namespace Sales.Domain.ValueObjects
{
    public record QuoteRequestId
    {
        public Guid Value { get; }
        public QuoteRequestId(Guid value) => Value = value;

        public static QuoteRequestId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new ArgumentException("QuoteRequestId cannot be empty.");
            }
            return new QuoteRequestId(value);
        }
    }
}
