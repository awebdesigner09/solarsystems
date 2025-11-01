using Sales.Domain.Enums;

namespace Sales.Application.Dtos
{
    public record QuoteRequestDto(
        Guid Id,
        Guid CustomerId,
        Guid SystemModelId,
        QuoteRequestStatus Status,
        AddressDto InstallationAddress,
        QuoteCustomOptionsDto QuoteCustomOptions,
        string? AdditonalNotes);
}
