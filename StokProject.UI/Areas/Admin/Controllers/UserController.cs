using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StokProject.Entities.Entities;
using StokProject.Entities.Enums;
using StokProject.UI.Areas.Admin.Models;
using System.Data;
using System.Text;

namespace StokProject.UI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        string url = "https://localhost:7059";
        private readonly IWebHostEnvironment _environment;

        public UserController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            List<User> users = new List<User>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{url}/api/User/GetAllUsers"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    users = JsonConvert.DeserializeObject<List<User>>(apiCevap);
                }
            }

            return View(users);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user, List<IFormFile> files)
        {
            user.IsActive = true;
            user.Password = "12345A.";

            if (user.Role == 0)
                user.Role = UserRole.User;

            string returnedMessage = Upload.ImageUpload(files, _environment, out bool imgResult);

            if (imgResult)
            {
                user.PhotoURL = returnedMessage; //eğer ImageUpload'dan fırlatılan değer true ise returnedMessage bana foto url dönecek.
            }
            else
            {
                ViewBag.Message = returnedMessage;
                return View(user);
            }

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                using (var answ = await httpClient.PostAsync($"{url}/api/User/CreateUser", content))
                {
                    string apiResult = await answ.Content.ReadAsStringAsync();

                }
            }
            return RedirectToAction("Index");
        }
       
        static User updatedUser;
        
        [HttpGet]
        public async Task<IActionResult> UpdateUser(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{url}/api/User/GetUserById/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    updatedUser = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }

            return View(updatedUser);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(User guncellenmisUser, List<IFormFile> files)
        {
            if(files.Count == 0) //güncelleme sırasında foto seçmez ise
            {
                guncellenmisUser.PhotoURL = updatedUser.PhotoURL;
            }
            else
            {
                string returnedMessage = Upload.ImageUpload(files, _environment, out bool imgResult);

                if (imgResult)
                {
                    guncellenmisUser.PhotoURL = returnedMessage; //eğer ImageUpload'dan fırlatılan değer true ise returnedMessage bana foto url dönecek.
                }
                else
                {
                    ViewBag.Message = returnedMessage;
                    return View(guncellenmisUser);
                }
            }

            using (var httpClient = new HttpClient())
            {
                guncellenmisUser.IsActive = updatedUser.IsActive;
                guncellenmisUser.AddedDate = updatedUser.AddedDate;
                guncellenmisUser.Password = updatedUser.Password;

                StringContent content = new StringContent(JsonConvert.SerializeObject(guncellenmisUser), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PutAsync($"{url}/api/User/UpdateUser/{guncellenmisUser.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{url}/api/User/DeleteUser/{id}"))
                {

                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ActivateUser(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{url}/api/User/KullaniciAktiflestir/{id}"))
                {

                }
            }

            return RedirectToAction("Index");
        }
    }
}
