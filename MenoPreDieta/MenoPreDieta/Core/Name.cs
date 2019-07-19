using System;
using MenoPreDieta.Entities;

namespace MenoPreDieta.Core
{
    public class Name : IEquatable<Name>
    {
        public Name(INameEntity entity)
        {
            Id = entity.Id;
            Value = entity.Value;
        }

        public int Id { get; }

        public string Value { get; }

        public bool Equals(Name other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Name) obj);
        }

        public override int GetHashCode() => Id;

        public static bool operator ==(Name left, Name right) => Equals(left, right);

        public static bool operator !=(Name left, Name right) => !Equals(left, right);
    }
}