using StokProject.Entities.Entities;

namespace StokProject.UI.Areas.Admin.Models
{
    public class ProductViewModel
    {

        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
        public List<Entities.Entities.Supplier> Suppliers { get; set; }
    }
}
