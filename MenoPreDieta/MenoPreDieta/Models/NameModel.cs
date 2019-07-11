using System;
using MenoPreDieta.Annotations;

namespace MenoPreDieta.Models
{
    public class NameModel
    {
        public NameModel(int id, [NotNull] string name)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public int Id { get; }
        public string Name { get; }
    }
}