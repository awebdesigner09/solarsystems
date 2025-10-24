using BuildingBlocks.Exceptions;

namespace Sales.Application.Exceptions
{
    public class SystemModelNotFoundException : NotFoundException
    {
        public SystemModelNotFoundException(Guid id) : base("SystemModel", id)
        {
        }
    }
}
