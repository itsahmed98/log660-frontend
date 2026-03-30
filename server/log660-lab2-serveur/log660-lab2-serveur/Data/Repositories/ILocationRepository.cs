using log660_lab2_serveur.Models;

namespace log660_lab2_serveur.Data.Repositories
{
    public interface ILocationRepository
    {
        Task<Client?> GetClientAvecForfait(int idUtilisateur);
        Task<CopieFilm?> GetCopie(string idCopie);
        Task<int> CountLocationsActives(int idUtilisateur);

        Task AddLocation(Location location);
        Task SaveChanges();
    }
}