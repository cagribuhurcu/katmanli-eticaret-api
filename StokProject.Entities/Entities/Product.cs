using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokProject.Entities.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
            SiparisDetaylari = new List<OrderDetails>();
        }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Stock { get; set; }
        public DateTime? ExpireDate { get; set; }



        //navigation properties

        //bir ürünün bir kategorisi olur

        [ForeignKey("Kategori")]
        public int CategoryID { get; set; }
        public virtual Category? Kategori { get; set; }

        //bir ürünün bir tedarikçisi olur

        [ForeignKey("Tedarikci")]
        public int SupplierID { get; set; }
        public virtual Supplier? Tedarikci { get; set; }

        //bir ürün birden fazla sipariş detayında bulunabilir

        public virtual List<OrderDetails> SiparisDetaylari { get; set; }
    }
}
