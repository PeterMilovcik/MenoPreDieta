using System.Threading.Tasks;

namespace MenoPreDieta.Core
{
    public class UndefinedNames : Names
    {
        public UndefinedNames() : base(null)
        {
        }

        public override Task InitializeAsync() => 
            Task.CompletedTask;

        protected override Task<int> AddNamesToDatabase() => 
            Task.FromResult(0);
    }
}