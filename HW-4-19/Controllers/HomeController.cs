using HW_4_19.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAuthentication.Data;

namespace HW_4_19.Controllers
{
    public class HomeController : Controller
    {

        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=WebAuth; Integrated Security=true;";
        public IActionResult Index()
        {
            var currentUserEmail = User.Identity.Name;
            var repo = new UserRepository(_connectionString);
            var vm = new HomePageViewModel();

            vm.Ads = repo.GetAds();
            if(currentUserEmail != null)
            {
              vm.UserId = repo.GetUserId(currentUserEmail);
            }
               
                   
            return View(vm);

            
        }
        [Authorize]
        public IActionResult NewAd()
        {         
            return View();
        }
        [HttpPost]
        public IActionResult NewAd(Ad ad)
        {
            var currentUserEmail = User.Identity.Name;
            
            var repo = new UserRepository(_connectionString);
            ad.UserId = repo.GetUserId(currentUserEmail);
            ad.Name = repo.GetName(ad.UserId);
            ad.DateListed = DateTime.Now.AddDays(2);
            repo.AddAd(ad);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteAd(int id)
        {
           
            var repo = new UserRepository(_connectionString);
            repo.DeleteAd(id);
           
            return RedirectToAction("Index");
        }
        public IActionResult MyAccount()
        {
            var currentUserEmail = User.Identity.Name;
            var vm = new HomePageViewModel();
            var repo = new UserRepository(_connectionString);
            vm.UserId = repo.GetUserId(currentUserEmail);
            vm.Ads = repo.GetMyAds(vm.UserId);

            return View(vm);
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/home/index");
        }

    }
}