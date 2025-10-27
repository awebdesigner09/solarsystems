using BuildingBlocks.Exceptions;
namespace Sales.Application.Exceptions
{
    public class QuoteNotFoundException :NotFoundException
    {
        public QuoteNotFoundException(Guid id) : base("Quote", id)
        {
        }
    }
}
