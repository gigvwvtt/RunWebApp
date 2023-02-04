﻿using Microsoft.AspNetCore.Mvc;
using RunWebApp.Interfaces;
using RunWebApp.Models;
using RunWebApp.Repository;
using RunWebApp.Services;
using RunWebApp.ViewModels;

namespace RunWebApp.Controllers
{
    public class RaceController : Controller
    {
		private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;

        public RaceController(IRaceRepository raceRepository, IPhotoService photoService)
        {
			_raceRepository = raceRepository;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            var races = await _raceRepository.GetAll();
            return View(races);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            return View(race);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceVM.Image);

                var race = new Race
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        Street = raceVM.Address.Street,
                        City = raceVM.Address.City,
                        State = raceVM.Address.State
                    }
                };
                _raceRepository.Add(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(raceVM);
        }
    }
}