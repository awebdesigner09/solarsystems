namespace Sales.Domain.ValueObjects
{
    public record Address
    {
        public string AddressLine1 { get; } = default!;
        public string? AddressLine2 { get; }
        public string City { get; } = default!;
        public string Country { get; } = "USA";
        public string State { get; } = default!;
        public string PostalCode { get; } = default!;

        protected Address() { }

        private Address(string addressLine1, string? addressLine2, string city, string country, string state, string postalCode)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            Country = country;
            State = state;
            PostalCode = postalCode;
        }

        public static Address Of(string addressLine1, string? addressLine2, string city, string country, string state, string postalCode)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(addressLine1);
            ArgumentException.ThrowIfNullOrWhiteSpace(city);
            ArgumentException.ThrowIfNullOrWhiteSpace(state);
            ArgumentException.ThrowIfNullOrWhiteSpace(postalCode);
            
            return new Address(addressLine1, addressLine2, city, country, state, postalCode);
        }

    }
}
