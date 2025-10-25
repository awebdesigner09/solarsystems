using BuildingBlocks.Exceptions;

namespace Sales.Application.Exceptions
{
    public class CustomerNotFoundException : NotFoundException
    {
        public CustomerNotFoundException(Guid id) : base("Customer", id)
        {
        }
    }
}
