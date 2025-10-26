namespace Sales.Domain.ValueObjects
{
    public record SystemModelId
    {
        public Guid Value { get; }
        private SystemModelId(Guid value) => Value = value;

        private SystemModelId() { }

        public static SystemModelId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new ArgumentException("SystemModelId cannot be empty.");
            }
            return new SystemModelId(value);
        }
    }
}
