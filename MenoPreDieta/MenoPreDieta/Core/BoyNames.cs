using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Data;
using MenoPreDieta.Entities;

namespace MenoPreDieta.Core
{
    public class BoyNames : Names
    {
        protected override async Task<List<INameEntity>> GetNamesFromDatabase() => 
            (await App.Database.GetBoyNamesAsync()).OfType<INameEntity>().ToList();

        protected override async Task<List<INamePickEntity>> GetPairsFromDatabase() =>
            (await App.Database.GetBoyNamePicksAsync()).OfType<INamePickEntity>().ToList();

        public override INamePickEntity CreatePair(int firstId, int secondId) =>
            new BoyNamePickEntity { FirstNameId = firstId, SecondNameId = secondId };

        protected override Task<int> AddNamesToDatabase() => 
            App.Database.InsertBoyNamesAsync(new BoyNamesCatalog());

        public override Task<int> AddToDatabase(List<INamePickEntity> pairs) => 
            App.Database.InsertBoyNamePicksAsync(pairs.OfType<BoyNamePickEntity>());

        public override async Task ResetPairsAsync()
        {
            await App.Database.RecreateBoyNamesTableAsync();
            await InitializePairsAsync();
        }

        public override Task<int> UpdateDatabaseAsync(INamePickEntity pair) => 
            App.Database.UpdateBoyNamePickAsync(pair as BoyNamePickEntity);

        public override Task<int> DeleteFromDatabaseAsync(INamePickEntity namePickEntity) => 
            App.Database.DeleteBoyNamePickAsync(namePickEntity as BoyNamePickEntity);
    }
}