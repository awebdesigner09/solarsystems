namespace Sales.Domain.Models
{
    public class Customer : Entity<CustomerId>
    {
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public Address Address { get; private set; } = default!;

        public static Customer Create(CustomerId customerId, string name, string email, Address address)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentNullException.ThrowIfNull(address);
            var customer = new Customer
            {
                Id = customerId,
                Name = name,
                Email = email,
                Address = address
            };
            return customer;
        }

    }
}
