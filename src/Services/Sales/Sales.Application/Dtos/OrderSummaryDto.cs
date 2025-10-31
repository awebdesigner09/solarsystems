using Sales.Domain.Enums;

namespace Sales.Application.Dtos
{
    public record OrderSummaryDto
    (
        Guid Id,
        Guid QuoteId,
        string BaseModel,
        string City,
        string State,
        decimal TotalPrice,
        DateTime? OrderDate,
        OrderStatus OrderStatus,
        DateTime? StatusDate
    );
}
