namespace Sales.Domain.ValueObjects
{
    public record CustomerId
    {
        public Guid Value { get; init; }
        private CustomerId(Guid value) => Value = value;

        private CustomerId() { }

        public static CustomerId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if(value == Guid.Empty)
            {
                throw new ArgumentException("CustomerId cannot be empty.");
            }
            return new CustomerId(value);
        }
    }
}
