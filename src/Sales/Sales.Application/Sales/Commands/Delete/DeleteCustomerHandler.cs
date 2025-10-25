namespace Sales.Application.Sales.Commands.Delete
{
    public class DeleteCustomerHandler(IApplicationDbContext dbContext)
        : ICommandHandler<DeleteCustomerCommand, DeleteCustomerResult>
    {
        public async Task<DeleteCustomerResult> Handle(
            DeleteCustomerCommand command,
            CancellationToken cancellationToken)
        {
            var customerId = CustomerId.Of(command.CustomerId);
            var customer = await dbContext.Customers
                .FindAsync([customerId], cancellationToken);
            if (customer is null)
            {
                throw new CustomerNotFoundException(command.CustomerId);
            }
            dbContext.Customers.Remove(customer);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new DeleteCustomerResult(true);
        }
    }
}
