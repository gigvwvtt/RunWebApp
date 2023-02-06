using RunWebApp.Models;

namespace RunWebApp.Interfaces
{
	public interface IClubRepository
	{
		Task<IEnumerable<Club>> GetAll();
		Task<Club?> GetByIdAsync(int id);
		Task<Club?> GetByIdAsyncNoTracking(int id);
		Task<IEnumerable<Club>> GetClubByCity(string city);
		Task<List<State>> GetAllStates();
		Task<List<City>> GetAllCitiesByState(string state);
		Task<int> GetCountAsync(); 
		bool Add(Club club);
		bool Update(Club club);
		bool Delete(Club club);
		bool Save();
	}
}