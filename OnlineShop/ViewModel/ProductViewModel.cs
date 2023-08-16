using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.ViewModel
{
    public class ProductViewModel
    {
        public long ID { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string CateName { get; set; }
        public string CateMetatitle { get; set; }
        public string MetaTitle { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}