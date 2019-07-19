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

        protected override INamePickEntity CreatePair(int firstId, int secondId) =>
            new BoyNamePickEntity { FirstNameId = firstId, SecondNameId = secondId };

        protected override Task<int> AddNamesToDatabase() => 
            App.Database.InsertBoyNamesAsync(new BoyNamesCatalog());

        protected override Task<int> AddToDatabase(List<INamePickEntity> pairs) => 
            App.Database.InsertBoyNamePicksAsync(pairs.OfType<BoyNamePickEntity>());
    }
}