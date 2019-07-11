using System;
using MenoPreDieta.Annotations;

namespace MenoPreDieta.Models
{
    public class NameModel
    {
        public NameModel([NotNull] string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }
    }
}