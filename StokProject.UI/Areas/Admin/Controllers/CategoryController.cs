using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokProject.Entities.Entities;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace StokProject.UI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        string url = "https://localhost:7059";

        public async Task<IActionResult> Index()
        {
            List<Category> kategoriler = new List<Category>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{url}/api/Category/GetAllCategories"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    kategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
            }

            return View(kategoriler);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View(); // sadece view gösterecek
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            //buraya gelen kategori nesnesi serileştirerek (JSON'a çevirilerek) API'a gönderilir

            category.IsActive = true;

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PostAsync($"{url}/api/Category/CreateCategory", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
        }

        static Category updatedCategory;
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            using (var httpClient = new HttpClient())
            { 
                using (var cevap = await httpClient.GetAsync($"{url}/api/Category/GetCategoryById/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedCategory = JsonConvert.DeserializeObject<Category>(apiCevap);
                }
            }

            return View(updatedCategory);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(Category guncellenmisKategori)
        {
            using (var httpClient = new HttpClient())
            {
                //guncellenmisKategori.IsActive = updatedCategory.IsActive;
                //guncellenmisKategori.AddedDate = updatedCategory.AddedDate;

                StringContent content = new StringContent(JsonConvert.SerializeObject(guncellenmisKategori), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PutAsync($"{url}/api/Category/UpdateCategory/{guncellenmisKategori.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{url}/api/Category/DeleteCategory/{id}"))
                {
                    
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ActivateCategory(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{url}/api/Category/KategoriAktiflestir/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
    }
}
