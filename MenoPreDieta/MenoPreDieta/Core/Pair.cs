using System;

namespace MenoPreDieta.Core
{
    public class Pair : IEquatable<Pair>
    {
        public Pair(int id, Name first, Name second)
        {
            Id = id;
            First = first;
            Second = second;
        }

        public int Id { get; }

        public Name First { get; }

        public Name Second { get; }

        public Pick PickFirst() => new Pick(Id, this, First);

        public Pick PickSecond() => new Pick(Id, this, Second);

        public bool Equals(Pair other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Equals(First, other.First) && Equals(Second, other.Second);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Pair) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (First != null ? First.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Second != null ? Second.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Pair left, Pair right) => Equals(left, right);

        public static bool operator !=(Pair left, Pair right) => !Equals(left, right);
    }
}