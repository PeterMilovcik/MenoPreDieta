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
            Catalog = new NameCatalog();
            Pairs = new Pairs();
            UpdateQueue = new List<INamePickEntity>();
            DeleteQueue = new List<INamePickEntity>();
        }

        public NameCatalog Catalog { get; }

        public Pairs Pairs { get; }

        public List<INamePickEntity> UpdateQueue { get; }

        public List<INamePickEntity> DeleteQueue { get; }

        public virtual async Task InitializeAsync()
        {
            await InitializeCatalogAsync();
            await InitializePairsAsync();
        }

        public void Update(INamePickEntity pair) => UpdateQueue.Add(pair);

        public void Delete(INamePickEntity pair)
        {
            App.Names.Pairs.Remove(pair);
            DeleteQueue.Add(pair);
        }

        public async Task ProcessUpdateQueueAsync()
        {
            if (UpdateQueue.Any())
            {
                var item = UpdateQueue.First();
                await UpdateDatabaseAsync(item);
                UpdateQueue.Remove(item);
                await ProcessUpdateQueueAsync();
            }
        }

        public async Task ProcessDeleteQueueAsync()
        {
            if (DeleteQueue.Any())
            {
                var item = DeleteQueue.First();
                await DeleteFromDatabaseAsync(item);
                DeleteQueue.Remove(item);
                await ProcessDeleteQueueAsync();
            }
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

        public abstract Task ResetPairsAsync();

        public abstract Task<int> UpdateDatabaseAsync(INamePickEntity pair);

        public abstract Task<int> DeleteFromDatabaseAsync(INamePickEntity pair);
    }
}