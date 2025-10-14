using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersShop.Models.Search
{
    public abstract class PagedSearchKw : PagedSearch
    {
        public string? Keyword { get; set; }
    }
}
