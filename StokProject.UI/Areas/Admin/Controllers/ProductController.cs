using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StokProject.Entities.Entities;
using StokProject.UI.Areas.Admin.Models;
using System.Data;
using System.Net.Http;
using System.Text;

namespace StokProject.UI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        string url = "https://localhost:7059";

        public async Task<IActionResult> Index()
        {
            List<Product> products = new List<Product>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{url}/api/Product/GetAllProducts"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    products = JsonConvert.DeserializeObject<List<Product>>(apiCevap);
                }
            }

            return View(products);
        }


        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            List<Category> _categories = new List<Category>();

            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{url}/api/Category/GetAllCategories"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    _categories = JsonConvert.DeserializeObject<List<Category>>(apiResult);
                }

            }

            List<Supplier> _supplier = new List<Supplier>();

            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{url}/api/Supplier/GetAllSuppliers"))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();
                    _supplier = JsonConvert.DeserializeObject<List<Supplier>>(apiResult);
                }

            }
            AddProductDTO productDTO = new AddProductDTO()
            {
                Categories = _categories,
                Suppliers = _supplier
            };
            return View(productDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductDTO productDto)
        {
            Product product = new Product()
            {
                CategoryID = productDto.Product.CategoryID,
                SupplierID = productDto.Product.SupplierID,
                UnitPrice = productDto.Product.UnitPrice,
                Stock = productDto.Product.Stock,
                ExpireDate = productDto.Product.ExpireDate,
                ProductName = productDto.Product.ProductName,
                IsActive = true,
            };
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                using (var answ = await httpClient.PostAsync($"{url}/api/Product/CreateProduct", content))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();

                }

            }
            return RedirectToAction("Index");
        }

        static List<Category> aktifKategoriler;
        static List<Supplier> aktifTedarikciler;
        static Product updatedProduct;

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{url}/api/Product/GetProductById/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    updatedProduct = JsonConvert.DeserializeObject<List<Product>>(apiResponse)[0];
                }
            }

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{url}/api/Category/GetActiveCategories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    aktifKategoriler = JsonConvert.DeserializeObject<List<Category>>(apiResponse);
                }

                using (var response = await httpClient.GetAsync($"{url}/api/Supplier/GetActiveSuppliers"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    aktifTedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiResponse);
                }
            }

            ViewBag.AktifKategoriler = new SelectList(aktifKategoriler, "ID", "CategoryName");
            ViewBag.AktifTedarikciler = new SelectList(aktifTedarikciler, "ID", "SupplierName");

            return View(updatedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(Product guncellenmisProduct)
        {
            using (var httpClient = new HttpClient())
            {
                //guncellenmisProduct.Product.IsActive = updatedProduct.IsActive;
                //guncellenmisProduct.Product.ExpireDate = updatedProduct.ExpireDate;

                StringContent content = new StringContent(JsonConvert.SerializeObject(guncellenmisProduct), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PutAsync($"{url}/api/Product/UpdateProduct/{guncellenmisProduct.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{url}/api/Product/DeleteProduct/{id}"))
                {

                }
            }

            return RedirectToAction("Index");
        }
    }
}

