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
            if (!Pairs.Any())
            {
                await App.Names.CreatePairsAsync();
            }
        }

        private async Task CreatePairsAsync()
        {
            for (int i = 0; i < Catalog.Count; i++)
            {
                for (int j = i + 1; j < Catalog.Count; j++)
                {
                    var firstName = Catalog[i];
                    var secondName = Catalog[j];
                    var pair = App.Names.CreatePair(firstName.Id, secondName.Id);
                    if (pair != null) Pairs.Add(pair);
                }
            }
            await AddToDatabase(Pairs);
        }

        protected abstract Task<int> AddNamesToDatabase();

        protected abstract Task<List<INameEntity>> GetNamesFromDatabase();

        protected abstract Task<List<INamePickEntity>> GetPairsFromDatabase();

        protected abstract INamePickEntity CreatePair(int firstId, int secondId);

        protected abstract Task<int> AddToDatabase(List<INamePickEntity> pairs);

        public abstract Task<int> UpdateAsync(INamePickEntity pair);
    }
}
