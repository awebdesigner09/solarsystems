using System.Data;

namespace Sales.Domain.Models
{
    public class SystemModel : Entity<SystemModelId>
    {
        public string Name { get; private set; } = default!;
        public string PanelType { get; private set; } = default!;
        public decimal CapacityKW { get; private set; } = default!;
        public decimal BasePrice { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public string ImageUrl { get; private set; } = default!;

        public static SystemModel Create(SystemModelId id, string name, string panelType, decimal capacityKW, decimal basePrice,string description,string imageUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(panelType);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacityKW);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(basePrice);

            var systemModel = new SystemModel
            {
                Id = id,
                Name = name,
                PanelType = panelType,
                CapacityKW = capacityKW,
                BasePrice = basePrice,
                Description = description,
                ImageUrl = imageUrl
            };

            return systemModel;
        }
        public void Update(string name, string panelType, decimal capacityKW, decimal basePrice, string description, string imageUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(panelType);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacityKW);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(basePrice);
            Name = name;
            PanelType = panelType;
            CapacityKW = capacityKW;
            BasePrice = basePrice;
            Description = description;
            ImageUrl = imageUrl;
        }

    }
}
