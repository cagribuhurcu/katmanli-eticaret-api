using StokProject.Entities.Entities;

namespace StokProject.UI.Areas.Admin.Models
{
    public class AddProductDTO
    {
        public AddProductDTO()
        {
            Categories = new List<Category>();
            Suppliers = new List<Supplier>();
        }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public int Stock { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime ExpireDate { get; set; }
        public string ProductName { get; set; }
        public bool IsAvtive { get; set; }

        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
        public List<Supplier> Suppliers { get; set; }
    }
}
