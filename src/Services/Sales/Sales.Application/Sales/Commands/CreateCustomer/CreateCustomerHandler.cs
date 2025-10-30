﻿namespace Sales.Application.Sales.Commands.CreateCustomer
{
    public class CreateCustomerHandler(IApplicationDbContext dbContext) :
        ICommandHandler<CreateCustomerCommand, CreateCustomerResult>
    {
        public async Task<CreateCustomerResult> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            // Create new Customer entity
            // Save to database using context
            // Return result with new entity Id
            var customer = CreateNewCustomer(command.Customer, command.UserId);
            dbContext.Customers.Add(customer);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new CreateCustomerResult(customer.Id.Value);
        }
        private Customer CreateNewCustomer(CustomerDto dto, string userId)
        {
            // Map DTO to Entity
            var newCustomer = Customer.Create(
                id: CustomerId.Of(Guid.NewGuid()),
                name: dto.Name,
                email: dto.Email,
                address: Address.Of(dto.Address.AddressLine1, dto.Address.AddressLine2, dto.Address.City, dto.Address.State, dto.Address.ZipCode, dto.Address.Country),
                userId: userId);

            return newCustomer;
        }
    }
}
