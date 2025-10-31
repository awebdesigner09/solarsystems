//namespace Sales.Application.Extensions
//{
//    public static class OrderExtensions
//    {
//        public static IEnumerable<OrderSummaryDto> ToOrderDtoList(this IEnumerable<Order> orders)
//        {
//            return orders.Select(o => o.ToOrderDto());
//        }

//        public static OrderSummaryDto ToOrderDto(this Order order)
//        {
//            return order.ToOrderDto();
//        }
//        private static OrderSummaryDto DtoFromOrder(Order order)
//        {
//            return new OrderSummaryDto(
//                Id: order.Id,
//                QuoteId: order.QuoteId,
//                BaseModel: order.BaseModel,
//                City: order.City,
//                State: order.State,
//                TotalPrice: order.TotalPrice,
//                OrderDate: order.CreatedAt,
//                OrderStatus: order.Status
//                );
//        }
//    }
//}
