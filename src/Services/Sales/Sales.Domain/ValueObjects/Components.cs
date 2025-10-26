namespace Sales.Domain.ValueObjects
{
    public record Components
    {
        public int NoOfPanels { get; }
        public int NoOfInverters { get; }
        public int NoOfMoutingSystems { get; }
        public int NoOfBatteries { get; }
        private Components(int noOfPanels, int noOfInverters, int noOfMoutingSystems, int noOfBatteries)
        {
            NoOfPanels = noOfPanels;
            NoOfInverters = noOfInverters;
            NoOfMoutingSystems = noOfMoutingSystems;
            NoOfBatteries = noOfBatteries;
        }
        public static Components Of(int noOfPanels, int noOfInverters, int noOfMoutingSystems, int noOfBatteries)
        { 
            if (noOfPanels <= 0)
            {
                throw new ArgumentException("Number of panels cannot be negative.");
            }
            if (noOfInverters <= 0)
            {
                throw new ArgumentException("Number of inverters cannot be negative.");
            }
            if (noOfMoutingSystems <= 0)
            {
                throw new ArgumentException("Number of mounting systems cannot be negative.");
            }
            if(noOfBatteries < 0)
            {
                throw new ArgumentException("Number of batteries cannot be negative.");
            }
            return new Components(noOfPanels, noOfInverters, noOfMoutingSystems, noOfBatteries);
        }
    }
}
