using SneakersShop.Data;
using SQLite;
using System.Text.Json;

namespace SneakersShop.Helpers
{
    public class LocalCacheService
    {
        private static SQLiteAsyncConnection _database;

        public static async Task InitAsync()
        {
            if (_database != null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "cache.db");

            _database = new SQLiteAsyncConnection(dbPath);
            await _database.CreateTableAsync<CachedProducts>();
        }

        public static async Task SaveAsync<T>(string key, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var cacheItem = new CachedProducts()
            {
                CacheKey = key,
                JsonData = json,
                CachedAt = DateTime.Now,
            };

            await _database.InsertOrReplaceAsync(cacheItem);
        }

        public static async Task<T?> GetAsync<T>(string key, TimeSpan validFor)
        {
            var item = await _database.Table<CachedProducts>()
                                      .Where(c => c.CacheKey == key)
                                      .FirstOrDefaultAsync();
            if (item == null || (DateTime.Now - item.CachedAt) > validFor)
                return default;

            return JsonSerializer.Deserialize<T>(item.JsonData);
        }

    }
}
