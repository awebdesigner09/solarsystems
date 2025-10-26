namespace Sales.Domain.ValueObjects
{
    public record OrderId
    {
        public Guid Value { get; }
        private OrderId(Guid value) => Value = value;

        private OrderId() { }
        public static OrderId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new ArgumentException("OrderId cannot be empty.");
            }
            return new OrderId(value);
        }

    }
}
