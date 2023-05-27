using Microsoft.AspNetCore.Mvc;
using StokProject.Entities.Entities;
using StokProject.UI.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using StokProject.UI.Models.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using StokProject.Entities;
using StokProject.Entities.Enums;

namespace StokProject.UI.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        string url = "https://localhost:7059"; //web API'nin çalıştığı sunucu portu ile birlikte olacak
        /*https://localhost:7059/api/User/Login?email=aa%40aa&password=1234*/
        [HttpPost]
        public async Task<IActionResult> Index(LoginDTO dto)
        {
            User logged = new User();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{url}/api/User/Login?email={dto.Email}&password={dto.Password}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    logged = JsonConvert.DeserializeObject<User>(apiCevap);
                }
            }

            if (logged != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim("ID", logged.ID.ToString()),
                    new Claim("PhotoURL", logged.PhotoURL),
                    new Claim(ClaimTypes.Name, logged.FirstName),
                    new Claim(ClaimTypes.Surname, logged.LastName),
                    new Claim(ClaimTypes.Email, logged.Email),
                    new Claim(ClaimTypes.Role, logged.Role.ToString()),
                };

                var userIdentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
            }
            else
            {
                return View(dto);
            }

            switch (logged.Role)
            {
                case UserRole.Admin:
                    return RedirectToAction("Index", "Admin", new { Area = "Admin" });
                case UserRole.Supplier:
                    return RedirectToAction("Index", "Supplier", new { Area = "Supplier" }); ;
                case UserRole.User:
                    return RedirectToAction("Index", "User", new { Area = "User" }); ;
                default:
                    return View(dto);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}