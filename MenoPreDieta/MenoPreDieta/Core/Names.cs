using System.Collections.Generic;
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
            Pairs = new List<INamePickEntity>();
        }

        public List<INameEntity> Catalog { get; }

        public List<INamePickEntity> Pairs { get; }

        public virtual async Task InitializeAsync()
        {
            await InitializeCatalogAsync();
            await InitializePairsAsync();
        }

        private async Task InitializeCatalogAsync()
        {
            Catalog.Clear();
            Catalog.AddRange(await GetNamesFromDatabase());
            if (!Catalog.Any())
            {
                await AddNamesToDatabase();
                Catalog.AddRange(await GetNamesFromDatabase());
            }
        }

        private async Task InitializePairsAsync()
        {
            Pairs.Clear();
            Pairs.AddRange(await GetPairsFromDatabase());
        }

        protected abstract Task AddNamesToDatabase();
        protected abstract Task<List<INameEntity>> GetNamesFromDatabase();
        protected abstract Task<List<INamePickEntity>> GetPairsFromDatabase();
    }
}
