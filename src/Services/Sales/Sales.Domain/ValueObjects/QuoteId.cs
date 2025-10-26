namespace Sales.Domain.ValueObjects
{
    public record QuoteId
    {
        public Guid Value { get; }
        private QuoteId(Guid value) => Value = value;
        private QuoteId() { }
        public static QuoteId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new ArgumentException("QuoteId cannot be empty.");
            }
            return new QuoteId(value);
        }
    }
}
