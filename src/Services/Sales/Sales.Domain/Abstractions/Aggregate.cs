namespace Sales.Domain.Abstractions
{
    public abstract class Aggregate<TId> : Entity<TId>, IAggregate
    {
        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        
        public IDomainEvent[] ClearDomainEvents()
        {
            var dequeuedEvens = _domainEvents.ToArray();

            _domainEvents.Clear();

            return dequeuedEvens;
        }
    }
}