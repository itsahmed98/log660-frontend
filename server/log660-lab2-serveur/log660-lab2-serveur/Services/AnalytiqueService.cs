using log660_lab2_serveur.Data;
using Microsoft.EntityFrameworkCore;

namespace log660_lab2_serveur.Services
{
    public class AnalytiqueService : IAnalytiqueService
    {
        private readonly AppDbContext _context;

        public AnalytiqueService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetNombreLocations(
            string? groupeAge,
            string? province,
            string? jourSemaine,
            string? moisAnnee)
        {
            var query = _context.FaitsLocation
                .Include(f => f.DimClient)
                .Include(f => f.DimTemps)
                .AsQueryable();

            // Filtre groupe d'âge
            if (!string.IsNullOrEmpty(groupeAge) && groupeAge != "Tous")
                query = query.Where(f => f.DimClient.GroupeAge == groupeAge);

            // Filtre province
            if (!string.IsNullOrEmpty(province) && province != "Toutes")
                query = query.Where(f => f.DimClient.Province == province);

            // Filtre jour de la semaine
            if (!string.IsNullOrEmpty(jourSemaine) && jourSemaine != "Tous")
                query = query.Where(f => f.DimTemps.JourSemaine.Trim() == jourSemaine);

            // Filtre mois
            if (!string.IsNullOrEmpty(moisAnnee) && moisAnnee != "Tous")
                query = query.Where(f => f.DimTemps.MoisAnnee.Trim() == moisAnnee);

            return await query.SumAsync(f => f.NbLocations);
        }
    }
}
