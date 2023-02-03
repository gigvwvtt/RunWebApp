using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunWebApp.Interfaces;

namespace RunWebApp.Controllers
{
    public class RaceController : Controller
    {
		private readonly IRaceRepository _raceRepository;

		public RaceController(IRaceRepository raceRepository)
        {
			this._raceRepository = raceRepository;
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
    }
}
