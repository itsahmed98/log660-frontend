using log660_lab2_serveur.Dtos;
using log660_lab2_serveur.Services;
using Microsoft.AspNetCore.Mvc;

namespace log660_lab2_serveur.Controllers
{
    [ApiController]
    [Route("api/locations")]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _service;

        public LocationsController(ILocationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Permet à un utilisateur de louer une copie d’un film.
        /// </summary>
        /// <param name="idUtilisateur">Identifiant de l'utilisateur effectuant la location.</param>
        /// <param name="idCopie">Identifiant de la copie du film à louer.</param>
        /// <returns>
        /// Retourne le résultat de l'opération de location.
        /// </returns>
        /// <response code="200">La location a été effectuée avec succès.</response>
        /// <response code="400">Les paramètres fournis sont invalides ou la location est impossible.</response>
        /// <response code="404">L'utilisateur ou la copie demandée n'existe pas.</response>
        /// <response code="500">Une erreur interne est survenue lors du traitement de la location.</response>
        [HttpPost("louer")]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseDto>> Louer([FromQuery] int idUtilisateur, [FromQuery] string idCopie)
        {
            var res = await _service.LouerAsync(idUtilisateur, idCopie);
            return Ok(res);
        }
    }
}