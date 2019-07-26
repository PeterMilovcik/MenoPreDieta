using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Data;

namespace MenoPreDieta.Core
{
    public class BoyNames : Names
    {
        private readonly BoyNamesCatalog initialNames;

        public BoyNames() : base(new Database(DbPath("boys.db3")))
        {
            initialNames = new BoyNamesCatalog();
        }

        protected override Task<int> AddNamesToDatabase() => Database.AddNamesAsync(initialNames);

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            foreach (var name in Catalog)
            {
                if (string.IsNullOrEmpty(name.NameDay))
                {
                    name.NameDay = initialNames.SingleOrDefault(n => n.Value == name.Value)?.NameDay;
                }
                if (string.IsNullOrEmpty(name.Description))
                {
                    name.Description = initialNames.SingleOrDefault(n => n.Value == name.Value)?.Description;
                }
            }
        }
    }
}