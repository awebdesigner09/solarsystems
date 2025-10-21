namespace Sales.Domain.ValueObjects
{
    public record SystemModelId
    {
        public Guid Value { get; }
        public SystemModelId(Guid value) => Value = value;

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
