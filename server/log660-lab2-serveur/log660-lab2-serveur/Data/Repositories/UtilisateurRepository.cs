using log660_lab2_serveur.Models;
using Microsoft.EntityFrameworkCore;

namespace log660_lab2_serveur.Data.Repositories
{
    public class UtilisateurRepository : IUtilisateurRepository
    {
        private readonly AppDbContext _appDbContext;

        public UtilisateurRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Utilisateur?> GetUtilisateurByEmail(string courriel, string motDePasse)
        {
            return await _appDbContext.Utilisateurs.FirstOrDefaultAsync(u => u.Courriel == courriel && u.MotDePasse == motDePasse);
        }
    }
}
