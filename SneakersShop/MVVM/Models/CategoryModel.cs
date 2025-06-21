using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersShop.MVVM.Models
{
    public class CategoryModel : BaseModel
    {
        public string? Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
