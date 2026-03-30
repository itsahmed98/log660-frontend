using log660_lab2_serveur.Dtos;
using log660_lab2_serveur.Models;

namespace log660_lab2_serveur.Services
{
    public interface IUtilisateurService
    {
        Task<UtilisateurDto?> LoginAsync(string courriel, string motDePasse);
    }
}
