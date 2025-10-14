namespace SneakersShop.Models.Search
{
    public abstract class PagedSearch
    {
        public int? PerPage { get; set; }
        public int? Page { get; set; }
    }
}
