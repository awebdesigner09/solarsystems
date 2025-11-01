namespace Sales.Application.Sales.Commands.UpdateCustomer
{
    public class UpdateCustomerHandler(IApplicationDbContext dbContext):
        ICommandHandler<UpdateCustomerCommand, UpdateCustomerResult>
    {
        public async Task<UpdateCustomerResult> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            var customerId = CustomerId.Of(command.Customer.Id);
            var customer = await dbContext.Customers
                .FindAsync([customerId], cancellationToken);
            if (customer == null)
            {
                throw new CustomerNotFoundException(command.Customer.Id);
            }
            UpdateCustomerWithValues(customer, command.Customer);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new UpdateCustomerResult(true);
        }
        private void UpdateCustomerWithValues(Customer customer, CustomerDto dto)
        {
            customer.Update(
                name: dto.Name,
                email: dto.Email,
                address: Address.Of(
                    dto.Address.AddressLine1,
                    dto.Address.AddressLine2,
                    dto.Address.City,
                    dto.Address.State,
                    dto.Address.PostalCode,
                    dto.Address.Country));
        }
    }
}
