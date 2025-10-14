using SQLite;

namespace SneakersShop.Local
{
    public class CachedDataModel
    {
        [PrimaryKey]
        public string CacheKey { get; set; }
        public string JsonData { get; set; }
        public DateTime CachedAt { get; set; }
    }
}
