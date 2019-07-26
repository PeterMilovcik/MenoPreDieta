using System.Linq;
using System.Threading.Tasks;
using MenoPreDieta.Data;
using MenoPreDieta.Entities;

namespace MenoPreDieta.Core
{
    public class GirlNames : Names
    {
        private readonly GirlNamesCatalog initialNames;

        public GirlNames() : base(new Database(DbPath("girls.db3")))
        {
            initialNames = new GirlNamesCatalog();
        }

        public override PickEntity CreatePick(int firstId, int secondId) => 
            new PickEntity { FirstNameId = firstId, SecondNameId = secondId };

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