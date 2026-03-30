using log660_lab2_serveur.Dtos;
using log660_lab2_serveur.Services;
using Microsoft.AspNetCore.Mvc;

namespace log660_lab2_serveur.Controllers
{
    /// <summary>
    /// Contrôleur API pour l'authentification
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUtilisateurService _utiliateurService;

        public AuthController(IUtilisateurService utilisateurService)
        {
            _utiliateurService = utilisateurService;
        }

        /// <summary>
        /// Authenticates a user based on the provided login credentials.
        /// </summary>
        /// <param name="request">
        /// The login request containing the user's email address and password.
        /// </param>
        /// <returns>User information if authentication succeeds.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UtilisateurDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Courriel) || string.IsNullOrWhiteSpace(request.MotDePasse))
            {
                return BadRequest("Courriel et mot de passe requis.");
            }

            var user = await _utiliateurService.LoginAsync(request.Courriel, request.MotDePasse);

            if (user is null)
                return Unauthorized("Utilisateur introuvable ou mot de passe incorrect.");

            return Ok(user);
        }
    }
}

