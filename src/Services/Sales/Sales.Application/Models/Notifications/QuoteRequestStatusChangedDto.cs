namespace Sales.Application.Models.Notifications
{
    public class QuoteRequestStatusChangedDto
    {
        public Guid QuoteRequestId { get; set; }
        public Guid CustomerId { get; set; }
        public string NewStatus { get; set; }
        public string OldStatus { get; set; }
    }
}
