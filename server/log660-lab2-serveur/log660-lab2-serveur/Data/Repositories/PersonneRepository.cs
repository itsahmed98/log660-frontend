using log660_lab2_serveur.Models;
using Microsoft.EntityFrameworkCore;

namespace log660_lab2_serveur.Data.Repositories
{
    public class PersonneRepository : IPersonneRepository
    {
        private readonly AppDbContext _appDbContext;

        public PersonneRepository(AppDbContext db)
        {
            _appDbContext = db;
        }

        public async Task<Personne?> GetById(int idPersonne)
        {
            return await _appDbContext.Personnes
                .AsNoTracking()
                .Include(p => p.Acteur)
                .Include(p => p.Realisateur)
                .FirstOrDefaultAsync(p => p.IdPersonne == idPersonne);
        }
    }
}