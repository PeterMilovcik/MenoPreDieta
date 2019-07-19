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

        protected override async Task AddNamesToDatabase() =>
            await App.Database.InsertGirlNamesAsync(new GirlNamesCatalog());
    }
}