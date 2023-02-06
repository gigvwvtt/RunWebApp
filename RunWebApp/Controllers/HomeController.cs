﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RunWebApp.Helpers;
using RunWebApp.Interfaces;
using RunWebApp.Models;
using RunWebApp.ViewModels;
using System.Diagnostics;
using System.Globalization;
using System.Net;

namespace RunWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClubRepository _clubRepository;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IClubRepository clubRepository, IConfiguration config)
        {
            _logger = logger;
            _clubRepository = clubRepository;
            _config = config;
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
                else
                {
                    homeViewModel.Clubs = null;
                }
                return View(homeViewModel);
            }
            catch (Exception)
            {
                homeViewModel.Clubs = null;
            }
            return View(homeViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}