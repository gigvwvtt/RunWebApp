using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunWebApp.Data;
using RunWebApp.Models;
using RunWebApp.ViewModels;

namespace RunWebApp.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ApplicationDbContext _context;

		public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_context = context;
		}

		public IActionResult Login()
		{
			var response = new LoginViewModel();
			return View(response);
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{
			if (!ModelState.IsValid) return View(loginViewModel);

			var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

			if (user != null) 
			{
				//user is found, check password
				var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
				if (passwordCheck)
				{
					//password correct, sign in
					var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
					if (result.Succeeded)
					{
						return RedirectToAction("Index", "Race");
					}
				}
				//password is incorrect
				TempData["Error"] = "Wrong credentials. Pleasy, try again";
				return View(loginViewModel);
			}
			//user not found
			TempData["Error"] = "Wrong credentials. Pleasy, try again";
			return View(loginViewModel);
		}
	}
}
