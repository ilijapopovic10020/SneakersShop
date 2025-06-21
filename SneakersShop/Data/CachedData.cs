using SQLite;

namespace SneakersShop.Data
{
    public class CachedProducts
    {
        [PrimaryKey]
        public string CacheKey { get; set; }
        public string JsonData { get; set; }
        public DateTime CachedAt { get; set; }
    }
}
