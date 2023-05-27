using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace StokProject.UI.Areas.SupplierArea.Controllers
{
    public class SupplierController : Controller
    {
        [Area("Supplier"), Authorize(Roles = "Supplier")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
