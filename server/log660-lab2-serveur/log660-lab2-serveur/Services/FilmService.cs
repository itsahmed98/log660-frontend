using log660_lab2_serveur.Data.Repositories;
using log660_lab2_serveur.Dtos;
using Microsoft.Extensions.Caching.Memory;

namespace log660_lab2_serveur.Services
{
    public class FilmService : IFilmService
    {
        private readonly IFilmRepository _filmRepository;
        private readonly IMemoryCache _cache;

        public FilmService(IFilmRepository filmRepository, IMemoryCache cache)
        {
            _filmRepository = filmRepository;
            _cache = cache;
        }

        public async Task<FilmDto?> GetFilmById(int idFilm)
        {
            string cacheKey = $"film_{idFilm}";
            if (_cache.TryGetValue(cacheKey, out FilmDto? cachedFilm))
            {
                return cachedFilm;
            }

            var film = await _filmRepository.GetFilmById(idFilm);
            if (film is null) return null;

            var realisateur = new RealisateurDto
            {
                IdPersonne = film.Realisateur.Personne.IdPersonne,
                Nom = film.Realisateur.Personne.Nom,
                Biographie = film.Realisateur.Personne.Biographie,
                DateNaissance = film.Realisateur.Personne.DateNaissance,
                LieuNaissance = film.Realisateur.Personne.LieuNaissance,
                Photo = film.Realisateur.Personne.Photo
            };

            var copies = film.CopiesFilm.Select(c => new CopieFilmDto
            {
                IdCopie = c.IdCopie,
                IdFilm = c.IdFilm,
                Statut = c.Statut
            }).ToList();

            var genresDto = film.FilmsGenres.Select(fg => new FilmGenreDto
            {
                IdGenre = fg.IdGenre,
                NomGenre = fg.Genre.NomGenre
            }).ToList();

            var paysDto = film.FilmsPays.Select(fp => new FilmPaysDto
            {
                IdPays = fp.IdPays,
                NomPays = fp.Pays.NomPays
            }).ToList();

            var acteursDto = film.Roles.Select(r => new ActeurDto
            {
                IdPersonne = r.Acteur.Personne.IdPersonne,
                Nom = r.Acteur.Personne.Nom,
                Photo = r.Acteur.Personne.Photo,
                NomPersonnage = r.NomPersonnage
            }).ToList();

            var filmDto = new FilmDto
            {
                IdFilm = film.IdFilm,
                Titre = film.Titre,
                Annee = film.Annee,
                Langue = film.Langue,
                Duree = film.Duree,
                Resume = film.Resume,
                Affiche = film.Affiche,
                BandeAnnonce = film.BandeAnnonce,
                Realisateur = realisateur,
                CopiesFilm = copies,
                FilmsGenres = genresDto,
                FilmsPays = paysDto,
                Acteurs = acteursDto
            };

            _cache.Set(cacheKey, filmDto, TimeSpan.FromMinutes(5));

            return filmDto;
        }

        public async Task<List<FilmSearchDto>> SearchFilmsByQuery(FilmSearchParams param)
        {
            return await _filmRepository.SearchFilmsByQuery(param);
        }

        public async Task<IEnumerable<GenreDto>> GetAllGenres()
        {
            var genres = await _filmRepository.GetAllGenres();
            var genreDtos = genres.OrderBy(g => g.IdGenre).Select(g => new GenreDto
            {
                IdGenre = g.IdGenre,
                NomGenre = g.NomGenre
            });

            return genreDtos;
        }

        public async Task<IEnumerable<PaysDto>> GetAllPays()
        {
            var pays = await _filmRepository.GetAllPays();
            var paysDtos = pays.OrderBy(g => g.IdPays).Select(p => new PaysDto
            {
                IdPays = p.IdPays,
                NomPays = p.NomPays
            });

            return paysDtos;
        }
    }
}