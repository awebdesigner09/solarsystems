namespace Sales.Domain.ValueObjects
{
    public record QuoteCustomOptions
    {
        public bool OptBattery { get; set; } = default!;
        public bool OptEVCharger { get; set; } = default!;
        protected QuoteCustomOptions() { }
        private QuoteCustomOptions(bool optBattery, bool optEVCharger)
        {
            OptBattery = optBattery;
            OptEVCharger = optEVCharger;
        }

        public static QuoteCustomOptions Of(bool optBattery, bool optEVCharger)
        {
            return new QuoteCustomOptions(optBattery, optEVCharger);
        }
    }
    
}
