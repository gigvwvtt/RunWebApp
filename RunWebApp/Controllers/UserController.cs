using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunWebApp.Interfaces;
using RunWebApp.Models;
using RunWebApp.ViewModels;

namespace RunWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPhotoService _photoService;


		public UserController(IUserRepository userRepository, UserManager<AppUser> userManager, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _photoService = photoService;
        }

		[HttpGet("users")]
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
                    Mileage = user.Mileage,
                    ProfileProfileImageUrl = user.ProfileImageUrl
                };
                result.Add(userVM);
			}
            
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null) return RedirectToAction("Index", "User");


            var userDetailVM = new UserDetailViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl
            };
            return View(userDetailVM);
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null) return View("Error");
            
            var editUserVM = new EditProfileViewModel()
            {
                Pace = user.Pace,
                Mileage = user.Mileage,
                City = user.City,
                State = user.State,
                ProfileImageUrl = user.ProfileImageUrl
            };
            return View(editUserVM);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditProfileViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to error profile");
                return View("EditProfile", editVM);
            }
            
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return View("Error");


            if(editVM.Image != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                if (photoResult.Error != null)
                {
                    ModelState.AddModelError("Image", "Failed to upload image");
                }

                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    _ = _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }

                user.ProfileImageUrl = photoResult.Url.ToString();
                editVM.ProfileImageUrl = user.ProfileImageUrl;

                await _userManager.UpdateAsync(user);

                return View(editVM);
            }

            user.City = editVM.City;
            user.State = editVM.State;
            user.Pace = editVM.Pace;
            user.Mileage = editVM.Mileage;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Detail", "User", new { user.Id });
        }
    }
}