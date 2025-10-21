namespace Sales.Domain.Models
{
    public class Order : Aggregate<OrderId>
    {
        public QuoteRequestId QuoteRequestId { get; private set; } = default!;
        public OrderStatus Status { get; private set; } = OrderStatus.Processing;

        public static Order Create(OrderId orderId, QuoteRequestId quoteRequestId)
        {
            var order = new Order
            {
                Id = orderId,
                QuoteRequestId = quoteRequestId,
                Status = OrderStatus.Processing
            };
            order.AddDomainEvent(new OrderCreatedEvent(order));

            return order;
        }

    }
}
