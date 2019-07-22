using System.Collections.Generic;
using System.Linq;
using MenoPreDieta.Entities;

namespace MenoPreDieta.Core
{
    public class NameCatalog : List<NameEntity>
    {
        public NameEntity NameWithId(int nameId) => this.Single(name => name.Id == nameId);
    }
}