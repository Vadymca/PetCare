namespace PetCare.Domain.Common
{
    /// <summary>
    /// Base class for entities that are not aggregate roots
    /// </summary>
    public abstract class Entity : BaseEntity
    {
        protected Entity() : base() { }
        protected Entity(Guid id) : base(id) { }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity? left, Entity? right)
        {
            return left?.Equals(right) ?? right is null;
        }

        public static bool operator !=(Entity? left, Entity? right)
        {
            return !(left == right);
        }
    }
}
