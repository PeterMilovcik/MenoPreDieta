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
            database.CreateTableAsync<BoyNameEntity>().Wait();
            database.CreateTableAsync<GirlNameEntity>().Wait();
            database.CreateTableAsync<BoyNamePickEntity>().Wait();
            database.CreateTableAsync<GirlNamePickEntity>().Wait();
        }

        public Task<List<BoyNameEntity>> GetBoyNamesAsync() => 
            database.Table<BoyNameEntity>().ToListAsync();

        public Task<List<GirlNameEntity>> GetGirlNamesAsync() =>
            database.Table<GirlNameEntity>().ToListAsync();

        public Task<int> InsertBoyNamesAsync(IEnumerable<BoyNameEntity> names) => 
            database.InsertAllAsync(names);

        public Task<int> InsertGirlNamesAsync(IEnumerable<GirlNameEntity> names) =>
            database.InsertAllAsync(names);

        public Task<List<BoyNamePickEntity>> GetBoyNamePicksAsync() =>
            database.Table<BoyNamePickEntity>().ToListAsync();

        public Task<List<GirlNamePickEntity>> GetGirlNamePicksAsync() =>
            database.Table<GirlNamePickEntity>().ToListAsync();

        public Task<int> InsertBoyNamePicksAsync(IEnumerable<BoyNamePickEntity> namePicks) =>
            database.InsertAllAsync(namePicks);

        public Task<int> InsertGirlNamePicksAsync(IEnumerable<GirlNamePickEntity> namePicks) =>
            database.InsertAllAsync(namePicks);

        public Task<int> UpdateBoyNamePickAsync(BoyNamePickEntity namePick) =>
            database.UpdateAsync(namePick);

        public Task<int> UpdateGirlNamePickAsync(GirlNamePickEntity namePick) =>
            database.UpdateAsync(namePick);

        public Task<int> DeleteBoyNamePickAsync(BoyNamePickEntity namePick) => 
            database.DeleteAsync(namePick);

        public Task<int> DeleteGirlNamePickAsync(GirlNamePickEntity namePick) =>
            database.DeleteAsync(namePick);

        public async Task RecreateBoyNamesTableAsync()
        {
            await database.DropTableAsync<BoyNamePickEntity>();
            await database.CreateTableAsync<BoyNamePickEntity>();
        }

        public async Task RecreateGirlNamesTableAsync()
        {
            await database.DropTableAsync<GirlNamePickEntity>();
            await database.CreateTableAsync<GirlNamePickEntity>();
        }
    }
}