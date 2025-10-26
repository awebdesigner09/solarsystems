using BuildingBlocks.Exceptions;
namespace Sales.Application.Exceptions
{
    public class QuoteRequestNotFoundException :NotFoundException
    {
        public QuoteRequestNotFoundException(Guid id) : base("QuoteRequest", id)
        {
        }
    }
}
