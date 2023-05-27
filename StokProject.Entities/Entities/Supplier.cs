using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokProject.Entities.Entities
{
    public class Supplier : BaseEntity
    {
        public Supplier()
        {
            Urunler = new List<Product>();
        }
        public string SupplierName { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }


        //navigation propperty
        //bir tedarikçinin birden fazla ürünü olabilir
        public virtual List<Product> Urunler { get; set; }
    }
}
