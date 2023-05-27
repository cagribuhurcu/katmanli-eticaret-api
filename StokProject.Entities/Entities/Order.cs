using StokProject.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace StokProject.Entities.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            SiparisDetaylari = new List<OrderDetails>();
        }
        public Status Status { get; set; }

        //navigation properties
        //bir siparişi bir kullanıcı verir

        [ForeignKey("Kullanici")]
        public int UserID { get; set; }
        public virtual User Kullanici { get; set; }

        //bir siparişin içinde birden fazla ürün olabileceği için birden fazla sipariş detayı olabilir
        public virtual List<OrderDetails> SiparisDetaylari { get; set; }
    }
}
