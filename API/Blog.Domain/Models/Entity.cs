using System;
using System.Collections.Generic;

namespace Blog.Domain.Models
{
    public abstract class Entity
    {
        private int _hashCode;
        private List<IDomainEvent> _domainEvents;

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        public Guid Id { get; private set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        protected Entity(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Entity ID cannot be equal to empty guid!");
            }

            Id = id;
        }

        private int HashCode
        {
            get
            {
                if (IsTransient())
                {
                    return base.GetHashCode();
                }

                if (_hashCode == default)
                {
                    _hashCode = Id.GetHashCode() ^ 31;
                }

                return _hashCode;
            }
        }

        protected void AddDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents ??= new List<IDomainEvent>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents?.Clear();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Entity))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            var item = (Entity)obj;

            if (item.IsTransient() || IsTransient())
            {
                return false;
            }

            return item.Id == Id;
        }

        public override int GetHashCode() => HashCode;

        public static bool operator ==(Entity left, Entity right) => left?.Equals(right) ?? Equals(right, null);
        public static bool operator !=(Entity left, Entity right) => !(left == right);

        private bool IsTransient() => Id == default;
    }
}
