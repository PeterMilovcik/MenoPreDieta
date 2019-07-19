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

        protected override async Task AddNamesToDatabase() => 
            await App.Database.InsertBoyNamesAsync(new BoyNamesCatalog());
    }
}