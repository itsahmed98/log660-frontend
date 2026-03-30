using log660_lab2_serveur.Dtos;

namespace log660_lab2_serveur.Services
{
    public interface ILocationService
    {
        Task<ResponseDto> LouerAsync(int idUtilisateur, string idCopie);
    }
}