namespace BuildingBlocks.Exceptions
{
    public class LimitExceededExcption : Exception
    {
        public LimitExceededExcption(string message) : base(message)
        {
        }

        public LimitExceededExcption(string name, object key) : base($"{key} {name} limit exceeded.")
        {
        }
    }
}
