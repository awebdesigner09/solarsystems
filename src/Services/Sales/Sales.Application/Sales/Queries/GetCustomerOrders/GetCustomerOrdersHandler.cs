
using Sales.Domain.Models;
using Sales.Domain.ValueObjects;

namespace Sales.Application.Sales.Queries.GetCustomerOrders
{
    public class GetCustomerOrdersHandler(IApplicationDbContext dbContext) :
        IQueryHandler<GetCustomerOrdersQuery, GetCustomerOrdersResult>
    {
        public async Task<GetCustomerOrdersResult> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
        {
            var orderDetails = await (from order in dbContext.Orders
                                join quote in dbContext.Quotes on order.QuoteId equals quote.Id
                                join quotereq in dbContext.QuoteRequests on quote.QuoteRequestId equals quotereq.Id
                                join systemModel in dbContext.SystemModels on quotereq.SystemModelId equals systemModel.Id
                                join customer in dbContext.Customers on quotereq.CustomerId equals customer.Id
                                where customer.Id == CustomerId.Of(request.customerId)
                                select new 
                                {
                                    order.Id,
                                    order.QuoteId,
                                    systemModel.Name,
                                    customer.Address.City,
                                    customer.Address.State,
                                    quote.TotalPrice,
                                    order.CreatedAt,
                                    order.Status,
                                    order.LastModified
                                })
                                .ToListAsync(cancellationToken)
                                .ConfigureAwait(false);

            return new GetCustomerOrdersResult(orderDetails
                .Select(o => new OrderSummaryDto(
                    o.Id.Value,
                    o.QuoteId.Value,
                    o.Name,
                    o.City,
                    o.State,
                    o.TotalPrice,
                    o.CreatedAt,
                    o.Status,
                    o.LastModified
                ))
                .ToList());
        }
    }
}
