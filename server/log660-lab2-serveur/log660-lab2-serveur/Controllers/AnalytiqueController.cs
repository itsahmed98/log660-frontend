using log660_lab2_serveur.Data;
using log660_lab2_serveur.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace log660_lab2_serveur.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalytiqueController : ControllerBase
    {
        private readonly IAnalytiqueService _analytiqueService;

        public AnalytiqueController(IAnalytiqueService analytiqueService)
        {
            _analytiqueService = analytiqueService;
        }

        /// <summary>
        /// Retourne le nombre de locations selon 4 filtres optionnels.
        /// Valeur "Tous" ou "Toutes" = pas de filtre pour cet attribut.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetNombreLocations(
            [FromQuery] string groupeAge = "Tous",
            [FromQuery] string province = "Toutes",
            [FromQuery] string jourSemaine = "Tous",
            [FromQuery] string moisAnnee = "Tous")
        {
            var total = await _analytiqueService.GetNombreLocations(
                groupeAge, province, jourSemaine, moisAnnee);

            return Ok(new
            {
                Filtres = new
                {
                    GroupeAge = groupeAge,
                    Province = province,
                    JourSemaine = jourSemaine,
                    MoisAnnee = moisAnnee
                },
                NombreLocations = total
            });
        }

        /// <summary>
        /// Retourne les valeurs possibles pour chaque filtre
        /// (pour peupler les dropdowns côté client)
        /// </summary>
        [HttpGet("filtres")]
        public async Task<IActionResult> GetValeursFiltre(
            [FromServices] AppDbContext context)
        {
            var groupesAge = await context.DimClients
                .Select(c => c.GroupeAge)
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();

            var provinces = await context.DimClients
                .Select(c => c.Province)
                .Distinct()
                .OrderBy(p => p)
                .ToListAsync();

            var joursSemaine = await context.DimTemps
                .Select(t => t.JourSemaine.Trim())
                .Distinct()
                .ToListAsync();

            var mois = await context.DimTemps
                .Select(t => t.MoisAnnee.Trim())
                .Distinct()
                .ToListAsync();

            return Ok(new
            {
                GroupesAge = groupesAge,
                Provinces = provinces,
                JoursSemaine = joursSemaine,
                Mois = mois
            });
        }
    }
}
