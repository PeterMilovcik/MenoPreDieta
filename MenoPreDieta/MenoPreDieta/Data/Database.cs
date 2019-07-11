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
            database.CreateTableAsync<NamePickEntity>().Wait();
        }

        public Task<List<NameEntity>> GetNamesAsync() => 
            database.Table<NameEntity>().ToListAsync();

        public Task<int> InsertNamesAsync(IEnumerable<NameEntity> names) => 
            database.InsertAllAsync(names);

        public Task<List<NamePickEntity>> GetNamePicksAsync() =>
            database.Table<NamePickEntity>().ToListAsync();

        public Task<int> InsertNamePicksAsync(IEnumerable<NamePickEntity> namePicks) =>
            database.InsertAllAsync(namePicks);

        public Task<int> UpdateNamePickAsync(NamePickEntity namePick) =>
            database.UpdateAsync(namePick);

        public Task<int> DeleteNamePickAsync(NamePickEntity namePick) => 
            database.DeleteAsync(namePick);
    }
}