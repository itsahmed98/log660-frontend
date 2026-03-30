using log660_lab2_serveur.Data;
using log660_lab2_serveur.Data.Repositories;
using log660_lab2_serveur.Dtos;
using log660_lab2_serveur.Models;

namespace log660_lab2_serveur.Services
{
    public class LocationService : ILocationService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILocationRepository _repo;

        private const string STATUT_DISPONIBLE = "Disponible";
        private const string STATUT_LOUEE = "Louee";

        public LocationService(AppDbContext db, ILocationRepository repo)
        {
            _appDbContext = db;
            _repo = repo;
        }

        public async Task<ResponseDto> LouerAsync(int idUtilisateur, string idCopie)
        {
            await using var tx = await _appDbContext.Database.BeginTransactionAsync();

            // 1) Client + forfait
            var client = await _repo.GetClientAvecForfait(idUtilisateur);


            // 2) Copie existe ?  
            var copie = await _repo.GetCopie(idCopie);

            if (copie?.Statut == "Louee")
            {
                return new ResponseDto
                {
                    ResponseCode = 2,
                    Message = "Copie n'est pas disponible."
                };
            }

            // 3) Forfait le permet ?
            var actives = await _repo.CountLocationsActives(idUtilisateur);
            if (actives >= client?.Forfait.LocationMax)
            {
                return new ResponseDto
                {
                    ResponseCode = 3,
                    Message = $"Votre forfait ne permet pas de louer plus de films. Locations max: {client.Forfait.LocationMax}"
                };
            }

            // 4) Copie dispo ?
            var statut = (copie.Statut ?? "");
            if (statut != STATUT_DISPONIBLE)
            {
                return new ResponseDto
                {
                    ResponseCode = 2,
                    Message = "Aucune copie disponible pour ce film."
                };
            }

            // 5) Calcul DateRetourMax
            var dateDebut = DateTime.Now;
            DateTime? dateRetourMax = null;
            if (client.Forfait.DureeMax.HasValue)
                dateRetourMax = dateDebut.AddDays(client.Forfait.DureeMax.Value);

            // 6) Insert + Update statut
            var location = new Location
            {
                IdUtilisateur = idUtilisateur,
                IdCopie = idCopie,
                DateDebut = dateDebut,
                DateRetour = null,
                DateRetourMax = dateRetourMax
            };

            await _repo.AddLocation(location);
            copie.Statut = STATUT_LOUEE;

            await _repo.SaveChanges();
            await tx.CommitAsync();

            return new ResponseDto
            {
                ResponseCode = 1,
                Message = "Film loué avec succès."
            };
        }
    }
}