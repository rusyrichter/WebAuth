
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAuthentication.Data;

namespace HW_4_19.Controllers
{
    public class AccountController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=WebAuth;Integrated Security=true;";
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(User user)
        {
            var repo = new UserRepository(_connectionString);
            repo.AddUser(user);
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var repo = new UserRepository(_connectionString);
            var user = repo.Login(email, password);
            if(user == null)
            {
                TempData["Message"] = "Invalid Login";
                return Redirect("/Account/Login");
            }
            
                var claims = new List<Claim>
            {
                new Claim("user", email) 
            };

                HttpContext.SignInAsync(new ClaimsPrincipal(
                    new ClaimsIdentity(claims, "Cookies", "user", "role")))
                    .Wait();

            
            return Redirect("/Home/Index");
        }
    }
}
