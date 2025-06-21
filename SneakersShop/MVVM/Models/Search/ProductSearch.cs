using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersShop.MVVM.Models.Search
{
    public class ProductSearch : PagedSearchKw
    {
        public int? CategoryId { get; set; }
        public List<int>? BrandId { get; set; }
        public List<string>? Color { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public int? Filter { get; set; }
    }
}
