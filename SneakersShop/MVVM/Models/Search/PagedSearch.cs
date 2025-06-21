namespace SneakersShop.MVVM.Models.Search
{
    public abstract class PagedSearch
    {
        public int? PerPage { get; set; }
        public int? Page { get; set; }
    }
}
