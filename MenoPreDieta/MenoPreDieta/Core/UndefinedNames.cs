using System.Collections.Generic;
using System.Threading.Tasks;
using MenoPreDieta.Entities;

namespace MenoPreDieta.Core
{
    public class UndefinedNames : Names
    {
        public override Task InitializeAsync() => 
            Task.CompletedTask;

        protected override Task AddNamesToDatabase() => 
            Task.CompletedTask;

        protected override Task<List<INameEntity>> GetNamesFromDatabase() =>
            Task.FromResult(new List<INameEntity>());

        protected override Task<List<INamePickEntity>> GetPairsFromDatabase() => 
            Task.FromResult(new List<INamePickEntity>());
    }
}