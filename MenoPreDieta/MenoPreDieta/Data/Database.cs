using System.Collections.Generic;
using System.Threading.Tasks;
using MenoPreDieta.Entities;
using SQLite;

namespace MenoPreDieta.Data
{
    public class Database
    {
        private readonly SQLiteAsyncConnection database;

        public Database(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<NameEntity>().Wait();
            database.CreateTableAsync<PickEntity>().Wait();
        }

        public Task<List<NameEntity>> GetNamesAsync() => 
            database.Table<NameEntity>().ToListAsync();
        public Task<List<PickEntity>> GetPicksAsync() => 
            database.Table<PickEntity>().ToListAsync();

        public Task<int> AddNamesAsync(IEnumerable<NameEntity> names) => 
            database.InsertAllAsync(names);
        public Task<int> AddPicksAsync(IEnumerable<PickEntity> picks) =>
            database.InsertAllAsync(picks);

        public Task<int> UpdateNameAsync(NameEntity name) => 
            database.UpdateAsync(name);
        public Task<int> UpdatePickAsync(PickEntity pick) => 
            database.UpdateAsync(pick);

        public Task<int> DeleteNameAsync(NameEntity name) => 
            database.DeleteAsync(name);
        public Task<int> DeletePickAsync(PickEntity pick) => 
            database.DeleteAsync(pick);

        public Task ResetNamesAsync() =>
            database.DropTableAsync<NameEntity>()
                .ContinueWith(task => database.CreateTableAsync<NameEntity>());
        public Task ResetPicksAsync() =>
            database.DropTableAsync<PickEntity>()
                .ContinueWith(task => database.CreateTableAsync<PickEntity>());
    }
}