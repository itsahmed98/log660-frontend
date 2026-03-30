using log660_lab2_serveur.Data.Repositories;
using log660_lab2_serveur.Dtos;
using log660_lab2_serveur.Models;

namespace log660_lab2_serveur.Services
{
    public class UtilisateurService: IUtilisateurService
    {
        private readonly IUtilisateurRepository _utilisateurRepository;

        public UtilisateurService(IUtilisateurRepository utilisateurRepository)
        {
            _utilisateurRepository = utilisateurRepository;
        }

        public async Task<UtilisateurDto?> LoginAsync(string courriel, string motDePasse)
        {
            var user = await _utilisateurRepository.GetUtilisateurByEmail(courriel, motDePasse);

            if (user is null)
                return null;

            return new UtilisateurDto
            {
                IdUtilisateur = user.IdUtilisateur,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Courriel = user.Courriel,
                Telephone = user.Telephone,
                DateNaissance = user.DateNaissance
            };
        }
    }
}
