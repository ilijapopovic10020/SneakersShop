using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersShop.Models.Search
{
    public class ProductsSearch : PagedSearchKw
    {
        public int? CategoryId { get; set; }
        public List<int>? BrandId { get; set; }
        public List<string>? Color { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? Filter { get; set; }
    }
}
