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
        }

        public Task<List<NameEntity>> GetNamesAsync() => 
            database.Table<NameEntity>().ToListAsync();

        public Task<int> InsertNamesAsync(IEnumerable<NameEntity> names) => 
            database.InsertAllAsync(names);
    }
}