using System;

namespace MenoPreDieta.Core
{
    public class Pick : IEquatable<Pick>
    {
        public Pick(int id, Pair pair, Name picked)
        {
            Id = id;
            Pair = pair;
            Picked = picked;
        }

        public int Id { get; }

        public Pair Pair { get; }

        public Name Picked { get; }

        public bool Equals(Pick other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Equals(Pair, other.Pair) && Equals(Picked, other.Picked);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Pick) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (Pair != null ? Pair.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Picked != null ? Picked.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Pick left, Pick right) => Equals(left, right);

        public static bool operator !=(Pick left, Pick right) => !Equals(left, right);
    }
}