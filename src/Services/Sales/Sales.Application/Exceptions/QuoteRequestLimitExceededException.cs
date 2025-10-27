using BuildingBlocks.Exceptions;

namespace Sales.Application.Exceptions
{
    public class QuoteRequestLimitExceededException : LimitExceededExcption
    {
        public QuoteRequestLimitExceededException(string id) : base("QuoteRequest", id)
        {
        }
    }
}
