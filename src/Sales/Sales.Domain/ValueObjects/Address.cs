using System.Diagnostics.Metrics;

namespace Sales.Domain.ValueObjects
{
    public record Address
    {
        public string AddressLine1 { get; } = default!;
        public string? AddressLine2 { get; }
        public string City { get; } = default!;
        public string State { get; } = default!;
        public string PostalCode { get; } = default!;
        public string Country { get; } = "USA";

        protected Address() { }

        private Address(string addressLine1, string? addressLine2, string city, string state, string postalCode, string country)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            State = state;
            PostalCode = postalCode;
            Country = country?? "USA";
        }

        public static Address Of(string addressLine1, string? addressLine2, string city, string state, string postalCode, string country)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(addressLine1);
            ArgumentException.ThrowIfNullOrWhiteSpace(city);
            ArgumentException.ThrowIfNullOrWhiteSpace(state);
            ArgumentException.ThrowIfNullOrWhiteSpace(postalCode);
            
            return new Address(addressLine1, addressLine2, city, state, postalCode, country);
        }

    }
}
