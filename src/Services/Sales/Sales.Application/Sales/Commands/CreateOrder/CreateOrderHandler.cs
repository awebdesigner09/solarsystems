namespace Sales.Application.Sales.Commands.CreateOrder
{
    public class CreateOrderHandler(IApplicationDbContext dbContext) :
        ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var Order = CreateNewOrder(command.Order);
            dbContext.Orders.Add(Order);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new CreateOrderResult(Order.Id.Value);
        }
        private Order CreateNewOrder(OrderDto dto)
        {
            // Map DTO to Entity
            var newOrder = Order.Create(
                orderId: OrderId.Of(Guid.NewGuid()),
                quoteId: QuoteId.Of(dto.QuoteId)
                );

            return newOrder;
        }
    }
}
