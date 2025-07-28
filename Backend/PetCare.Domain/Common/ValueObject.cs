namespace PetCare.Domain.Common
{
    /// <summary>
    /// Base class for value objects with proper equality semantics
    /// </summary>
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object?> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != GetType())
                return false;

            var other = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Where(x => x != null)
                .Aggregate(1, (current, obj) => HashCode.Combine(current, obj));
        }

        public static bool operator ==(ValueObject? left, ValueObject? right)
        {
            return left?.Equals(right) ?? right is null;
        }

        public static bool operator !=(ValueObject? left, ValueObject? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Creates a copy of this value object
        /// </summary>
        public virtual ValueObject Copy()
        {
            return (ValueObject)MemberwiseClone();
        }
    }
}
