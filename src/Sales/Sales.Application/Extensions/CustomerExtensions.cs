namespace Sales.Application.Extensions
{
    public static class CustomerExtensions
    {
        public static IEnumerable<CustomerDto> ToCustomerDtoList(this IEnumerable<Customer> customers)
        {
            return customers.Select(c => c.ToCustomerDto());
        }
        public static CustomerDto ToCustomerDto(this Customer customer)
        {
            return DtoFromCustomer(customer);
        }
        private static CustomerDto DtoFromCustomer(Customer customer)
        {
            return new CustomerDto(
                Id: customer.Id.Value,
                Name: customer.Name,
                Email: customer.Email,
                Address: new AddressDto(customer.Address.AddressLine1, customer.Address.AddressLine2, customer.Address.City, customer.Address.State,customer.Address.PostalCode,customer.Address.Country)
                );
        }
    }
}
