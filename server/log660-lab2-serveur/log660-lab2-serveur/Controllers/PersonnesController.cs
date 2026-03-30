using log660_lab2_serveur.Dtos;
using log660_lab2_serveur.Services;
using Microsoft.AspNetCore.Mvc;

namespace log660_lab2_serveur.Controllers
{
    [ApiController]
    [Route("api/personnes")]
    public class PersonnesController : ControllerBase
    {
        private readonly IPersonneService _service;

        public PersonnesController(IPersonneService service)
        {
            _service = service;
        }

        /// <summary>
        /// Récupère les informations d'une personne à partir de son identifiant.
        /// </summary>
        /// <param name="idPersonne">Identifiant unique de la personne à récupérer.</param>
        /// <returns>
        /// Retourne les informations de la personne si elle existe.
        /// </returns>
        /// <response code="200">La personne a été trouvée et retournée avec succès.</response>
        /// <response code="404">Aucune personne correspondant à l'identifiant fourni n'a été trouvée.</response>
        [HttpGet("{idPersonne:int}")]
        [ProducesResponseType(typeof(PersonneDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int idPersonne)
        {
            var personne = await _service.GetById(idPersonne);
            if (personne is null) return NotFound("Personne introuvable.");
            return Ok(personne);
        }
    }
}