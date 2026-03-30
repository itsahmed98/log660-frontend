using log660_lab2_serveur.Models;
using Microsoft.EntityFrameworkCore;

namespace log660_lab2_serveur.Data.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly AppDbContext _appDbContext;

        public LocationRepository(AppDbContext db)
        {
            _appDbContext = db;
        }

        public async Task<Client?> GetClientAvecForfait(int idUtilisateur)
        {
            return await _appDbContext.Clients
                .Include(c => c.Forfait)
                .FirstOrDefaultAsync(c => c.IdUtilisateur == idUtilisateur);
        }

        public async Task<CopieFilm?> GetCopie(string idCopie)
        {
            return await _appDbContext.CopiesFilm
                .FirstOrDefaultAsync(c => c.IdCopie == idCopie);
        }

        public async Task<int> CountLocationsActives(int idUtilisateur)
        {
            return await _appDbContext.Locations
                .CountAsync(l => l.IdUtilisateur == idUtilisateur && l.DateRetour == null);
        }

        public async Task AddLocation(Location location)
        {
            await _appDbContext.Locations.AddAsync(location);
        }

        public async Task SaveChanges()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}