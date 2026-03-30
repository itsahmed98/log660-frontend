using log660_lab2_serveur.Dtos;

namespace log660_lab2_serveur.Services
{
    public interface IPersonneService
    {
        Task<PersonneDto?> GetById(int idPersonne);
    }
}