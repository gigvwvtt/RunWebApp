﻿using Microsoft.AspNetCore.Mvc;
using RunWebApp.Interfaces;
using RunWebApp.ViewModels;

namespace RunWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

		public UserController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		[HttpGet("Runners")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers();
            var result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userVM = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Pace = user.Pace,
                    Mileage = user.Mileage
                };
			}
            
            return View(result);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepository.GetUserById(id);
            var userDetailVM = new UserDetailViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Pace = user.Pace,
                Mileage = user.Mileage
            };
            return View(userDetailVM);
        }
    }
}