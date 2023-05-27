using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokProject.Entities.Entities
{
    public class Category : BaseEntity
    {
        public Category()
        {
            Products = new List<Product>();         //null referance hatası vermemesi için
        }
        public string CategoryName { get; set; }
        public string Description { get; set; }



        //navigation property

        //bir kategorinin birden fazla ürünü olabilir
        public virtual List<Product> Products { get; set; }
    }
}
