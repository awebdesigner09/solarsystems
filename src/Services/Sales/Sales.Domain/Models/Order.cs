namespace Sales.Domain.Models
{
    public class Order : Aggregate<OrderId>
    {
        public QuoteId QuoteId { get; private set; } = default!;
        public OrderStatus Status { get; private set; } = OrderStatus.Processing;

        public static Order Create(OrderId orderId, QuoteId quoteId)
        {
            var order = new Order
            {
                Id = orderId,
                QuoteId = quoteId,
                Status = OrderStatus.Processing
            };
            order.AddDomainEvent(new OrderCreatedEvent(order));

            return order;
        }

    }
}
