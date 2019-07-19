using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Entities;

namespace MenoPreDieta.Core
{
    public abstract class Names
    {
        protected Names()
        {
            Catalog = new List<INameEntity>();
        }

        public List<INameEntity> Catalog { get; }

        public virtual async Task InitializeAsync()
        {
            Catalog.AddRange(await GetNamesFromDatabase());
            if (!Catalog.Any())
            {
                await AddNamesToDatabase();
                Catalog.AddRange(await GetNamesFromDatabase());
            }
        }

        protected abstract Task AddNamesToDatabase();

        protected abstract Task<List<INameEntity>> GetNamesFromDatabase();
    }
}
