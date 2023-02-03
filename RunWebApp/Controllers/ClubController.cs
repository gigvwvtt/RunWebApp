﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunWebApp.Data;

namespace RunWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClubController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var clubs = _context.Clubs.ToList();
            return View(clubs);
        }

        public IActionResult Detail(int id)
        {
            var club = _context.Clubs.Include(a => a.Address).FirstOrDefault(x => x.Id == id);
            return View(club);
        }
    }
}