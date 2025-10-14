using SneakersShop.Local;
using SneakersShop.Models;
using SQLite;
using System.Text.Json;

namespace SneakersShop.Helpers
{
    public static class LocalCacheService
    {
        private static SQLiteAsyncConnection _database;

        public static async Task InitAsync()
        {
            if (_database != null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "cache.db");

            _database = new SQLiteAsyncConnection(dbPath);
            await _database.CreateTableAsync<CachedDataModel>();
        }

        public static async Task SaveAsync<T>(string key, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var cacheItem = new CachedDataModel()
            {
                CacheKey = key,
                JsonData = json,
                CachedAt = DateTime.Now,
            };

            await _database.InsertOrReplaceAsync(cacheItem);
        }

        public static async Task<T?> GetAsync<T>(string key, TimeSpan validFor)
        {
            var item = await _database.Table<CachedDataModel>()
                                      .Where(c => c.CacheKey == key)
                                      .FirstOrDefaultAsync();
            if (item == null || (DateTime.Now - item.CachedAt) > validFor)
                return default;

            return JsonSerializer.Deserialize<T>(item.JsonData);
        }
    }

    public static class LastCheckedProductsCache
    {
        private const string CacheKey = "LastCheckedProducts";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromDays(1);

        public static async Task SaveLastCheckedProducts(ProductModel product)
        {
            await LocalCacheService.InitAsync();

            var current = await LocalCacheService.GetAsync<List<ProductModel>>(CacheKey, CacheDuration) ?? [];

            current.RemoveAll(p => p.Id == product.Id);

            current.Insert(0, product);

            if (current.Count > 6)
                current = [.. current.Take(6)];

            await LocalCacheService.SaveAsync(CacheKey, current);
        }

        public static async Task<List<ProductModel>> GetLastCheckedProducts()
        {
            await LocalCacheService.InitAsync();
            return await LocalCacheService.GetAsync<List<ProductModel>>(CacheKey, CacheDuration) ?? [];
        }

    }
}
