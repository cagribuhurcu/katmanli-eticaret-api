using StokProject.Entities.Entities;

namespace StokProject.UI.Areas.Admin.Models
{
    public class UpdateProductDTO
    {
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
        public List<Supplier> Suppliers { get; set; }
    }
}
