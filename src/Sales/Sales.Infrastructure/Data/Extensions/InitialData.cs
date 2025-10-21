

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
                    Customer.Create(CustomerId.Of(new Guid("A1F5C8D2-3C4E-4F6A-8D2A-1B2C3D4E5F60")), "John Doe", "johndoe@email.com", address1),
                    Customer.Create(CustomerId.Of(new Guid("B2E6D9E3-4D5F-5G7B-9E3B-2C3D4E5F6G70")), "Jane Smith", "janesmih@email.com", address2)
                };
            }
        }

        public static IEnumerable<QuoteRequest> QuoteRequests =>
            new List<QuoteRequest>
            {
                QuoteRequest.Create(QuoteRequestId.Of(new Guid("C3F7E0F4-5G6H-6H8C-0F4C-3D4E5F6G7H80")), CustomerId.Of(new Guid("A1F5C8D2-3C4E-4F6A-8D2A-1B2C3D4E5F60")), SystemModelId.Of(new Guid("90DA9DFB-EB54-42C7-BCED-93E8549213FF"))),
                QuoteRequest.Create(QuoteRequestId.Of(new Guid("D4G8F1G5-6H7I-7I9D-1G5D-4E5F6G7H8I90")), CustomerId.Of(new Guid("B2E6D9E3-4D5F-5G7B-9E3B-2C3D4E5F6G70")), SystemModelId.Of(new Guid("7FBD0B40-35F0-442F-B2A7-B07AB6818278")), "Custom instructions")
            };

        public static IEnumerable<Order> Orders =>
            new List<Order>
            {
                Order.Create(OrderId.Of(new Guid("E5H9G2H6-7I8J-8J0E-2H6E-5F6G7H8I9J00")), QuoteRequestId.Of(new Guid("C3F7E0F4-5G6H-6H8C-0F4C-3D4E5F6G7H80"))),
                Order.Create(OrderId.Of(new Guid("F6I0H3I7-8J9K-9K1F-3I7F-6G7H8I9J0K10")), QuoteRequestId.Of(new Guid("D4G8F1G5-6H7I-7I9D-1G5D-4E5F6G7H8I90")))
            };
    }
}
