namespace Sales.Domain.Models
{
    public class SystemModel : Entity<SystemModelId>
    {
        public string Name { get; private set; } = default!;
        public string PanelType { get; private set; } = default!;
        public int CapacityKW { get; private set; } = default!;
        public decimal BasePrice { get; private set; } = default!;

        public static SystemModel Create(SystemModelId systemModelId, string name, string panelType, int capacityKW, decimal basePrice)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(panelType);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacityKW);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(basePrice);

            var systemModel = new SystemModel
            {
                Id = systemModelId,
                Name = name,
                PanelType = panelType,
                CapacityKW = capacityKW,
                BasePrice = basePrice
            };

            return systemModel;
        }

    }
}
