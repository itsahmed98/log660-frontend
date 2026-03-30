using log660_lab2_serveur.Data.Repositories;
using log660_lab2_serveur.Dtos;

namespace log660_lab2_serveur.Services
{
    public class PersonneService : IPersonneService
    {
        private readonly IPersonneRepository _repo;

        public PersonneService(IPersonneRepository repo)
        {
            _repo = repo;
        }

        public async Task<PersonneDto?> GetById(int idPersonne)
        {
            var p = await _repo.GetById(idPersonne);
            if (p is null) return null;

            return new PersonneDto
            {
                IdPersonne = p.IdPersonne,
                Nom = p.Nom,
                DateNaissance = p.DateNaissance,
                LieuNaissance = p.LieuNaissance,
                Photo = p.Photo,
                Biographie = p.Biographie,
                EstActeur = p.Acteur != null,
                EstRealisateur = p.Realisateur != null
            };
        }
    }
}