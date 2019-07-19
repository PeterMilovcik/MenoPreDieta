using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Data;
using MenoPreDieta.Entities;

namespace MenoPreDieta.Core
{
    public class GirlNames : Names
    {
        protected override async Task<List<INameEntity>> GetNamesFromDatabase() => 
            (await App.Database.GetGirlNamesAsync()).OfType<INameEntity>().ToList();

        protected override async Task<List<INamePickEntity>> GetPairsFromDatabase() =>
            (await App.Database.GetGirlNamePicksAsync()).OfType<INamePickEntity>().ToList();

        protected override INamePickEntity CreatePair(int firstId, int secondId) => 
            new GirlNamePickEntity { FirstNameId = firstId, SecondNameId = secondId };

        protected override Task<int> AddToDatabase(List<INamePickEntity> pairs) => 
            App.Database.InsertGirlNamePicksAsync(pairs.OfType<GirlNamePickEntity>());

        public override async Task ResetPairsAsync()
        {
            await App.Database.RecreateGirlNamesTableAsync();
            await InitializePairsAsync();
        }

        protected override Task<int> AddNamesToDatabase() =>
            App.Database.InsertGirlNamesAsync(new GirlNamesCatalog());

        public override Task<int> UpdateDatabaseAsync(INamePickEntity pair) =>
            App.Database.UpdateGirlNamePickAsync(pair as GirlNamePickEntity);

        public override Task<int> DeleteFromDatabaseAsync(INamePickEntity pair) =>
            App.Database.DeleteGirlNamePickAsync(pair as GirlNamePickEntity);
    }
}