using log660_lab2_serveur.Models;

namespace log660_lab2_serveur.Data.Repositories
{
    public interface IUtilisateurRepository
    {
        Task<Utilisateur> GetUtilisateurByEmail(string courriel, string motDePasse);
    }
}
