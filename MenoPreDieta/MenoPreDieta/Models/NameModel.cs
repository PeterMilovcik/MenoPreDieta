using System;
using MenoPreDieta.Annotations;

namespace MenoPreDieta.Models
{
    public class NameModel
    {
        public NameModel(int id, [NotNull] string name, string nameDay)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            NameDay = nameDay;
        }

        public int Id { get; }
        public string Name { get; }
        public string NameDay { get; }
    }
}