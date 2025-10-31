﻿

namespace Sales.Infrastructure.Data.Extensions
{
    internal class InitialData
    {
        public static IEnumerable<SystemModel> SystemModels =>
            new List<SystemModel>
            {
                SystemModel.Create(SystemModelId.Of(new Guid("90DA9DFB-EB54-42C7-BCED-93E8549213FF")), "Mono PERC", "Monocrystalline", 6m, 15000m,
                    "A monocrystalline solar panel is made from a single, high-purity silicon crystal, giving it a uniform black color and high efficiency. These panels are known for being the most space-efficient type, making them ideal for limited areas, and they generally have a longer lifespan. While traditionally more expensive, their cost is decreasing, and they are very durable and perform well in low-light conditions.",
                    "monocrystalline.png"),
                SystemModel.Create(SystemModelId.Of(new Guid("7FBD0B40-35F0-442F-B2A7-B07AB6818278")), "Polycrystalline", "Polycrystalline", 7.5m, 22500,
                    "A polycrystalline solar panel is a type of solar panel made from multiple silicon crystals that are melted and fused together. This process creates a less uniform, mosaic-like surface and makes them more affordable and cost-effective to produce than monocrystalline panels. While they have a lower efficiency rate than monocrystalline panels, they are still a durable and long-lasting renewable energy source.  \r\n",
                    "polycrystalline.png")
            };

        public static IEnumerable<Customer> Customers
        {
            get
            {
                var address1 = Address.Of("123 Main St", "", "San Jose", "CA", "12345", "USA");
                var address2 = Address.Of("456 Oak Ave", "Apt 2", "Los Angeles", "CA", "67890", "USA");

                return new List<Customer>
                {
                    Customer.Create(CustomerId.Of(new Guid("F019DE72-D441-4C63-AE1B-E3A93A0976AF")), "John Doe", "johndoe@email.com", address1, "a18be9c0-aa65-4af8-bd17-00bd9344e576"),
                    Customer.Create(CustomerId.Of(new Guid("5CD5BD3C-2B96-4E6D-8D5B-53DD00CD6D56")), "Jane Smith", "janesmith@email.com", address2, "a18be9c0-aa65-4af8-bd17-00bd9344e577")
                };
            }
        }

        public static IEnumerable<QuoteRequest> QuoteRequests =>
            new List<QuoteRequest>
            {
                QuoteRequest.Create(QuoteRequestId.Of(new Guid("0B062865-7015-446E-858E-FF23071D0DB7")), CustomerId.Of(new Guid("F019DE72-D441-4C63-AE1B-E3A93A0976AF")), SystemModelId.Of(new Guid("90DA9DFB-EB54-42C7-BCED-93E8549213FF"))),
                QuoteRequest.Create(QuoteRequestId.Of(new Guid("B12137BE-7C40-4C68-B208-06745EBDBBE3")), CustomerId.Of(new Guid("5CD5BD3C-2B96-4E6D-8D5B-53DD00CD6D56")), SystemModelId.Of(new Guid("7FBD0B40-35F0-442F-B2A7-B07AB6818278")), "Custom instructions")
            };

        
        public static IEnumerable<Quote> Quotes =>
            new List<Quote>
            { Quote.Create(
                    QuoteId.Of(new Guid("833F7738-BD64-49DA-B4F4-82FCCF1E68FB")),
                    QuoteRequestId.Of(new Guid("0B062865-7015-446E-858E-FF23071D0DB7")),
                    new DateTime(2025, 11, 23, 10, 51, 17, DateTimeKind.Utc),
                    Components.Of(20, 1, 1, 1),
                    15000m,
                    1200m,
                    300m,
                    16500m),
                Quote.Create(
                    QuoteId.Of(new Guid("11813284-63BB-494E-A2B7-BE34381FEF8B")),
                    QuoteRequestId.Of(new Guid("B12137BE-7C40-4C68-B208-06745EBDBBE3")),
                    new DateTime(2025, 11, 25, 17, 18, 58, DateTimeKind.Utc),
                    Components.Of(25, 1, 1, 2),
                    22500m,
                    1800m,
                    450m,
                    24750m)
            };
        public static IEnumerable<Order> Orders =>
            new List<Order>
            {
                Order.Create(OrderId.Of(new Guid("D23E08E0-D751-4520-8910-1AA928D2290C")), QuoteId.Of(new Guid("833F7738-BD64-49DA-B4F4-82FCCF1E68FB"))),
                Order.Create(OrderId.Of(new Guid("FF7CDE8C-2F95-4313-AFAA-7A7198FD64FC")), QuoteId.Of(new Guid("11813284-63BB-494E-A2B7-BE34381FEF8B")))
            };

    }
}
