namespace Sales.Domain.Models
{
    public class Customer : Entity<CustomerId>
    {
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public Address Address { get; private set; } = default!;

        public static Customer Create(CustomerId id, string name, string email, Address address)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentNullException.ThrowIfNull(address);
            ArgumentException.ThrowIfNullOrWhiteSpace(address.AddressLine1);
            ArgumentException.ThrowIfNullOrWhiteSpace(address.City);
            ArgumentException.ThrowIfNullOrWhiteSpace(address.State);
            ArgumentException.ThrowIfNullOrWhiteSpace(address.PostalCode);
            var customer = new Customer
            {
                Id = id,
                Name = name,
                Email = email,
                Address = address
            };
            return customer;
        }

        public void Update(string name, string email, Address address)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentNullException.ThrowIfNull(address);
            ArgumentException.ThrowIfNullOrWhiteSpace(address.AddressLine1);
            ArgumentException.ThrowIfNullOrWhiteSpace(address.City);
            ArgumentException.ThrowIfNullOrWhiteSpace(address.State);
            ArgumentException.ThrowIfNullOrWhiteSpace(address.PostalCode);
            Name = name;
            Email = email;
            Address = address;
        }

    }
}
