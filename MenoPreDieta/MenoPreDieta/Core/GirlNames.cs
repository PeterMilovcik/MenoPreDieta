using System.Threading.Tasks;
using MenoPreDieta.Data;
using MenoPreDieta.Entities;

namespace MenoPreDieta.Core
{
    public class GirlNames : Names
    {
        public GirlNames() : base(new Database(DbPath("girls.db3")))
        {
        }

        public override PickEntity CreatePick(int firstId, int secondId) => 
            new PickEntity { FirstNameId = firstId, SecondNameId = secondId };

        protected override Task<int> AddNamesToDatabase() => Database.AddNamesAsync(new GirlNamesCatalog());
    }
}