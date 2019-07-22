using System.Threading.Tasks;
using MenoPreDieta.Data;

namespace MenoPreDieta.Core
{
    public class BoyNames : Names
    {
        public BoyNames() : base(new Database(DbPath("boys.db3")))
        {
        }

        protected override Task<int> AddNamesToDatabase() => Database.AddNamesAsync(new BoyNamesCatalog());
    }
}