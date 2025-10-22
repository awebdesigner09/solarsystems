

namespace Sales.Infrastructure.Data.Extensions
{
    internal class InitialData
    {
        public static IEnumerable<SystemModel> SystemModels =>
            new List<SystemModel>
            {
                SystemModel.Create(SystemModelId.Of(new Guid("90DA9DFB-EB54-42C7-BCED-93E8549213FF")), "name", "panel type", 100, 100),
                SystemModel.Create(SystemModelId.Of(new Guid("7FBD0B40-35F0-442F-B2A7-B07AB6818278")), "name", "panel type", 100, 100)
            };

        public static IEnumerable<Customer> Customers
        {
            get
            {
                var address1 = Address.Of("123 Main St", "", "San Jose", "CA", "12345","USA");
                var address2 = Address.Of("456 Oak Ave", "Apt 2", "Los Angeles", "CA", "67890","USA");

                return new List<Customer>
                {
                    Customer.Create(CustomerId.Of(new Guid("F019DE72-D441-4C63-AE1B-E3A93A0976AF")), "John Doe", "johndoe@email.com", address1),
                    Customer.Create(CustomerId.Of(new Guid("5CD5BD3C-2B96-4E6D-8D5B-53DD00CD6D56")), "Jane Smith", "janesmih@email.com", address2)
                };
            }
        }

        public static IEnumerable<QuoteRequest> QuoteRequests =>
            new List<QuoteRequest>
            {
                QuoteRequest.Create(QuoteRequestId.Of(new Guid("0B062865-7015-446E-858E-FF23071D0DB7")), CustomerId.Of(new Guid("F019DE72-D441-4C63-AE1B-E3A93A0976AF")), SystemModelId.Of(new Guid("90DA9DFB-EB54-42C7-BCED-93E8549213FF"))),
                QuoteRequest.Create(QuoteRequestId.Of(new Guid("B12137BE-7C40-4C68-B208-06745EBDBBE3")), CustomerId.Of(new Guid("5CD5BD3C-2B96-4E6D-8D5B-53DD00CD6D56")), SystemModelId.Of(new Guid("7FBD0B40-35F0-442F-B2A7-B07AB6818278")), "Custom instructions")
            };

        public static IEnumerable<Order> Orders =>
            new List<Order>
            {
                Order.Create(OrderId.Of(new Guid("D23E08E0-D751-4520-8910-1AA928D2290C")), QuoteRequestId.Of(new Guid("0B062865-7015-446E-858E-FF23071D0DB7"))),
                Order.Create(OrderId.Of(new Guid("FF7CDE8C-2F95-4313-AFAA-7A7198FD64FC")), QuoteRequestId.Of(new Guid("B12137BE-7C40-4C68-B208-06745EBDBBE3")))
            };
    }
}
