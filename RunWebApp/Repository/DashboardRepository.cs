using Microsoft.EntityFrameworkCore;
using RunWebApp.Data;
using RunWebApp.Interfaces;
using RunWebApp.Models;

namespace RunWebApp.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public DashboardRepository(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            this._httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<List<Club>> GetAllUserClubs()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userClubs = _context.Clubs.Where(u => u.AppUser.Id == currentUser);
            return userClubs.ToList();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userRaces = _context.Races.Where(u => u.AppUser.Id == currentUser);
            return userRaces.ToList();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetByIdNoTracking(string id)
        {
            return await _context.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }

        private bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
