using log660_lab2_serveur.Dtos;
using log660_lab2_serveur.Services;
using Microsoft.AspNetCore.Mvc;

namespace log660_lab2_serveur.Controllers
{
    /// <summary>
    /// Contrôleur API pour les films
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly IFilmService _filmService;

        public FilmsController(IFilmService filmService)
        {
            _filmService = filmService;
        }

        /// <summary>
        /// Récupère les informations détaillées d’un film à partir de son identifiant.
        /// </summary>
        /// <param name="idFilm">Identifiant unique du film à récupérer.</param>
        /// <returns>
        /// Retourne les informations du film si celui-ci existe.
        /// </returns>
        /// <response code="200">Le film a été trouvé et retourné avec succès.</response>
        /// <response code="404">Aucun film correspondant à l'identifiant fourni n'a été trouvé.</response>
        /// <response code="500">Une erreur interne est survenue lors de la récupération du film.</response>
        [HttpGet("{idFilm:int}")]
        [ProducesResponseType(typeof(FilmSearchDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFilmById(int idFilm)
        {
            try
            {
                var film = await _filmService.GetFilmById(idFilm);

                if (film == null)
                    return NotFound("Film introuvable dans le system.");
                return Ok(film);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur est survenue lors de la récupération du film: {ex.Message}");
            }
        }

        /// <summary>
        /// Recherche des films selon différents critères de recherche.
        /// </summary>
        /// <param name="param">
        /// Paramètres de recherche optionnels incluant le titre, acteur, réalisateur, genre,
        /// pays, langue ou intervalle d'années.
        /// Au moins un critère doit être fourni.
        /// </param>
        /// <returns>
        /// Une liste de films correspondant aux critères de recherche.
        /// </returns>
        /// <response code="200">La recherche a été exécutée avec succès.</response>
        /// <response code="400">Aucun critère de recherche n'a été fourni ou les paramètres sont invalides.</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<FilmSearchDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<FilmSearchDto>>> SearchFilms([FromQuery] FilmSearchParams param)
        {
            if (param.Titre == null && 
                param.Acteur == null && 
                param.Realisateur == null && 
                param.IdPays == null && 
                param.IdGenre == null && 
                param.Langue == null && 
                param.MinAnnee == null && 
                param.MaxAnnee == null
                )
                return BadRequest("Au moins un critère de recherche est requis.");

            if (param.MaxAnnee < param.MinAnnee)
                return BadRequest("La date de début ne peux pas être apres la date de fin.");

            var results = await _filmService.SearchFilmsByQuery(param);
            return Ok(results);
        }

        /// <summary>
        /// Récupère la liste des langues disponibles dans le système.
        /// </summary>
        /// <returns>
        /// Une liste de langues pouvant être utilisées pour la recherche de films.
        /// </returns>
        /// <response code="200">La liste des langues a été récupérée avec succès.</response>
        [HttpGet("langues")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        public ActionResult<List<String>> GetLangues()
        {
            return new List<string> { "English", "Frensh", "Portuguese", "Cantonese", "Japanese", "German"
                                    , "Italian", "Swedish", "Mandarin", "Spanish", "Korean", "Aramaic"};
        }

        /// <summary>
        /// Récupère la liste de tous les genres de films disponibles.
        /// </summary>
        /// <returns>
        /// Une collection contenant tous les genres enregistrés dans le système.
        /// </returns>
        /// <response code="200">La liste des genres a été récupérée avec succès.</response>
        /// <response code="500">Une erreur interne est survenue lors de la récupération des genres.</response>
        [HttpGet("genres")]
        [ProducesResponseType(typeof(IEnumerable<GenreDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<GenreDto>> GetAllGenres()
        {
            return await _filmService.GetAllGenres();
        }

        /// <summary>
        /// Récupère la liste de tous les pays disponibles dans le système.
        /// </summary>
        /// <returns>
        /// Une collection contenant tous les pays enregistrés dans le système.
        /// </returns>
        /// <response code="200">La liste des pays a été récupérée avec succès.</response>
        /// <response code="500">Une erreur interne est survenue lors de la récupération des pays.</response>
        [HttpGet("pays")]
        [ProducesResponseType(typeof(IEnumerable<PaysDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<PaysDto>> GetAllPays()
        {
            return await _filmService.GetAllPays();
        }
    }
}

