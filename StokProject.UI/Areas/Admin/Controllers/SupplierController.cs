using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokProject.Entities.Entities;
using System.Data;
using System.Text;

namespace StokProject.UI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class SupplierController : Controller
    {
        string url = "https://localhost:7059";

        public async Task<IActionResult> Index()
        {
            List<Supplier> tedarikci = new List<Supplier>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{url}/api/Supplier/GetAllSuppliers"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    tedarikci = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }

            return View(tedarikci);
        }

        [HttpGet]
        public IActionResult AddSupplier()
        {
            return View(); // sadece view gösterecek
        }

        [HttpPost]
        public async Task<IActionResult> AddSupplier(Supplier supplier)
        {
            //buraya gelen kategori nesnesi serileştirerek (JSON'a çevirilerek) API'a gönderilir

            supplier.IsActive = true;

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(supplier), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PostAsync($"{url}/api/Supplier/CreateSupplier", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSupplier(int id)
        {
            Supplier supplier = null;

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{url}/api/Supplier/GetSupplierById/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    supplier = JsonConvert.DeserializeObject<Supplier>(apiCevap);
                }
            }

            return View(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSupplier(Supplier supplier)
        {
            supplier.IsActive = true;

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(supplier), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PutAsync($"{url}/api/Supplier/UpdateSupplier/{supplier.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{url}/api/Supplier/DeleteSupplier/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ActivateSupplier(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{url}/api/Supplier/TedarikciAktiflestir/{id}"))
                {

                }
            }

            return RedirectToAction("Index");
        }
    }
}
