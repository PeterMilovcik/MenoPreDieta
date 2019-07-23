using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Data;
using MenoPreDieta.Entities;

namespace MenoPreDieta.Core
{
    public abstract class Names
    {
        protected Names(Database database)
        {
            Database = database;
            Catalog = new NameCatalog();
            Picks = new Picks();
        }

        protected Database Database { get; }

        public NameCatalog Catalog { get; }

        public Picks Picks { get; }

        public List<NameEntity> Processed => 
            Catalog.Where(name => name.IsProcessed).ToList();

        public List<NameEntity> NotProcessed =>
            Catalog.Where(name => !name.IsProcessed).ToList();

        public virtual Task InitializeAsync() => InitializeCatalogAsync();

        public async Task UpdateAsync(PickEntity pick) => await Database.UpdatePickAsync(pick);

        private async Task InitializeCatalogAsync()
        {
            Catalog.Clear();
            Catalog.AddRange(await Database.GetNamesAsync());
            if (!Catalog.Any())
            {
                await AddNamesToDatabase();
                Catalog.AddRange(await Database.GetNamesAsync());
            }
        }

        public async Task InitializePicksAsync()
        {
            Picks.Clear();
            Picks.AddRange(await Database.GetPicksAsync());
            if (!Picks.Any())
            {
                await App.Names.CreatePicksAsync();
            }
        }

        private async Task CreatePicksAsync()
        {
            var entities = Processed.Where(name => name.IsLiked).ToList();
            for (int i = 0; i < entities.Count; i++)
            {
                for (int j = i + 1; j < entities.Count; j++)
                {
                    var firstName = entities[i];
                    var secondName = entities[j];
                    var pick = App.Names.CreatePick(firstName.Id, secondName.Id);
                    Picks.Add(pick);
                }
            }

            if (Picks.Any())
            {
                await Database.AddPicksAsync(Picks);
            }
        }

        protected abstract Task<int> AddNamesToDatabase();

        protected static string DbPath(string fileName) =>
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), fileName);

        public virtual PickEntity CreatePick(int firstId, int secondId) =>
            new PickEntity { FirstNameId = firstId, SecondNameId = secondId };

        public virtual async Task ResetAsync()
        {
            var dropNamesTask = Database.ResetNamesAsync();
            var dropPicksTask = Database.ResetPicksAsync();
            await Task.WhenAll(dropNamesTask, dropPicksTask);
            await InitializeAsync();
        }

        public Task<int> ProcessAsync(NameEntity name)
        {
            name.IsProcessed = true;
            return UpdateAsync(name);
        }

        public Task<int> UpdateAsync(NameEntity name) => Database.UpdateNameAsync(name);
    }
}