using System.Collections.Generic;
using System.Threading.Tasks;
using MenoPreDieta.Entities;

namespace MenoPreDieta.Core
{
    public class UndefinedNames : Names
    {
        public override Task InitializeAsync() => 
            Task.CompletedTask;

        protected override Task<int> AddNamesToDatabase() => 
            Task.FromResult(0);

        protected override Task<int> AddToDatabase(List<INamePickEntity> pairs) => 
            Task.FromResult(0);

        public override Task ResetPairsAsync() => Task.CompletedTask;

        public override Task<int> UpdateDatabaseAsync(INamePickEntity pair) => 
            Task.FromResult(0);

        public override Task<int> DeleteFromDatabaseAsync(INamePickEntity pair) => 
            Task.FromResult(0);

        protected override Task<List<INameEntity>> GetNamesFromDatabase() =>
            Task.FromResult(new List<INameEntity>());

        protected override Task<List<INamePickEntity>> GetPairsFromDatabase() => 
            Task.FromResult(new List<INamePickEntity>());

        protected override INamePickEntity CreatePair(int firstId, int secondId) => 
            default;
    }
}