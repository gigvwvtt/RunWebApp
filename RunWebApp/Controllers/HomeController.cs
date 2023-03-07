using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RunWebApp.Helpers;
using RunWebApp.Interfaces;
using RunWebApp.Models;
using RunWebApp.ViewModels;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Identity;
using RunWebApp.Data;

namespace RunWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClubRepository _clubRepository;
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, IClubRepository clubRepository, 
            IConfiguration config, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _clubRepository = clubRepository;
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var ipInfo = new IPInfo();
            var homeViewModel = new HomeViewModel();
            try
            {
                var ipInfoToken = _config.GetSection("ipInfoToken");
                var url = $"https://ipinfo.io/?token={ipInfoToken.Value}";
                var info = new WebClient().DownloadString(url);
                ipInfo = JsonConvert.DeserializeObject<IPInfo>(info);
                var regionInfo = new RegionInfo(ipInfo.Country);
                ipInfo.Country = regionInfo.EnglishName;
                homeViewModel.City = ipInfo.City;
                homeViewModel.State = ipInfo.Region;
                if (homeViewModel.City != null)
                {
                    homeViewModel.Clubs = await _clubRepository.GetClubByCity(homeViewModel.City);
                }
                return View(homeViewModel);
            }
            catch (Exception)
            {
                homeViewModel.Clubs = null;
            }
            return View(homeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomeViewModel homeViewModel)
        {
            var createViewModel = homeViewModel.Register;
            if (!ModelState.IsValid) return View(homeViewModel);

            var user = await _userManager.FindByEmailAsync(createViewModel.Email);
            if (user != null)
            {
                ModelState.AddModelError("Register.Email", "This email address is already in use");
                return View(homeViewModel);
            }

            var newUser = new AppUser
            {
                UserName = createViewModel.UserName,
                Email = createViewModel.Email,
                Address = new Address()
                {
                    ZipCode = createViewModel.ZipCode ?? 0
                }
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, createViewModel.Password);

            if (newUserResponse.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, false);
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}