using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StokProject.Entities.Entities;
using StokProject.UI.Areas.Admin.Models;
using System.Data;
using System.Text;

namespace StokProject.UI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        string url = "https://localhost:7059";

        public async Task<IActionResult> Index()
        {
            List<Order> orders = new List<Order>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{url}/api/Order/GetAllOrders"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    orders = JsonConvert.DeserializeObject<List<Order>>(apiCevap);
                }
            }

            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using(var response = await httpClient.GetAsync($"{url}/api/Order/SiparisOnayla/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Sipariş onaylama işlemi başarısız oldu.";

                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> CancelOrder(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{url}/api/Order/SiparisReddet/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Sipariş onaylama işlemi başarısız oldu.";
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DetailsOrder(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{url}/api/Order/SiparisDetayiGetir/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        var orderDetails = JsonConvert.DeserializeObject<List<OrderDetails>>(apiResponse);
                        return Json(orderDetails);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Sipariş detayları getirme işlemi başarısız oldu.";
                        return Json(null);
                    }
                }
            }
        }
    }
}
