using log660_lab2_serveur.Dtos;
using log660_lab2_serveur.Models;
using Microsoft.EntityFrameworkCore;

namespace log660_lab2_serveur.Data.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        private readonly AppDbContext _appDbContext;

        public FilmRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Film?> GetFilmById(int idFilm)
        {
            return await _appDbContext.Films
                .AsNoTracking()
                .AsSplitQuery()
                .Include(f => f.Realisateur)
                    .ThenInclude(r => r.Personne)
                .Include(f => f.CopiesFilm)
                .Include(f => f.FilmsGenres)
                    .ThenInclude(fg => fg.Genre)
                .Include(f => f.FilmsPays)
                    .ThenInclude(fp => fp.Pays)
                .Include(f => f.Roles)
                    .ThenInclude(r => r.Acteur)
                        .ThenInclude(a => a.Personne)
                .FirstOrDefaultAsync(f => f.IdFilm == idFilm);
        }

        public async Task<IEnumerable<Genre>> GetAllGenres()
        {
            return await _appDbContext.Genres.ToListAsync();
        }

        public async Task<Genre> GetGenre(int idGenre)
        {
            return await _appDbContext.Genres.FirstOrDefaultAsync(g => g.IdGenre == idGenre);
        }

        public async Task<IEnumerable<Pays>> GetAllPays()
        {
            return await _appDbContext.Pays.ToListAsync();
        }

        public async Task<Personne> GetRealisateur(int idRealisateur)
        {
            return await _appDbContext.Personnes.FirstOrDefaultAsync(r => r.IdPersonne == idRealisateur);
        }
        public async Task<IEnumerable<CopieFilm>> GetCopiesFilm(int idFilm)
        {
            return await _appDbContext.CopiesFilm.Where(cf => cf.IdFilm == idFilm).ToListAsync();
        }

        public async Task<IEnumerable<FilmGenre>> GetGenresFilm(int idFilm)
        {
            return await _appDbContext.FilmsGenres.Where(cf => cf.IdFilm == idFilm).ToListAsync();
        }

        public async Task<IEnumerable<Role>> GetActeurs(int idFilm)
        {
            return await _appDbContext.Roles.Where(r => r.IdFilm == idFilm).ToListAsync();
        }

        public async Task<List<FilmSearchDto>> SearchFilmsByQuery(FilmSearchParams p)
        {
            IQueryable<Film> query = _appDbContext.Films.AsNoTracking();
                

            // 1) Titre
            if (!string.IsNullOrWhiteSpace(p.Titre))
            {
                var searchTerm = Normalize(p.Titre).Replace(" ", "");
                query = query.Where(f => (f.Titre ?? "").ToUpper().Replace(" ", "").Contains(searchTerm));
            }

            // 2) Langue
            if (!string.IsNullOrWhiteSpace(p.Langue))
            {
                var langue = Normalize(p.Langue);
                query = query.Where(f => (f.Langue ?? "").ToUpper() == langue);
            }

            // 3) Année min/max
            if (p.MinAnnee.HasValue)
                query = query.Where(f => f.Annee >= p.MinAnnee.Value);

            if (p.MaxAnnee.HasValue)
                query = query.Where(f => f.Annee <= p.MaxAnnee.Value);

            // 4) Genre (via table N-N FILM_GENRE)
            if (p.IdGenre.HasValue)
                query = query.Where(f =>
                    _appDbContext.FilmsGenres
                        .Any(fg => fg.IdFilm == f.IdFilm && fg.IdGenre == p.IdGenre.Value));

            // 5) Pays (via table N-N FILM_PAYS)
            if (p.IdPays.HasValue)
                query = query.Where(f =>
                    _appDbContext.FilmsPays
                        .Any(fp => fp.IdFilm == f.IdFilm && fp.IdPays == p.IdPays.Value));

            // 6) Réalisateur (nom/prénom via Personne)
            if (!string.IsNullOrWhiteSpace(p.Realisateur))
            {
                var q = Normalize(p.Realisateur);
                query = query.Where(f =>
                    _appDbContext.Realisateurs
                        .Any(r => r.IdPersonne == f.IdRealisateur &&
                                  _appDbContext.Personnes
                                      .Any(p2 => p2.IdPersonne == r.IdPersonne &&
                                                 p2.Nom.ToUpper().Contains(q))));
            }

            // 7) Acteur (via ROLE -> ACTEUR -> PERSONNE)
            if (!string.IsNullOrWhiteSpace(p.Acteur))
            {
                var q = Normalize(p.Acteur);
                query = query.Where(f =>
                    _appDbContext.Roles
                        .Any(r => r.IdFilm == f.IdFilm &&
                                  _appDbContext.Acteurs
                                      .Any(a => a.IdPersonne == r.IdActeur &&
                                                _appDbContext.Personnes
                                                    .Any(p2 => p2.IdPersonne == a.IdPersonne &&
                                                               p2.Nom.ToUpper().Contains(q)))));
            }

            query = query.OrderByDescending(f => f.Annee).ThenBy(f => f.Titre);

            return await query
                .OrderByDescending(f => f.Annee)
                .ThenBy(f => f.Titre)
                .Take(200)
                .Select(f => new FilmSearchDto
                {
                    IdFilm = f.IdFilm,
                    Titre = f.Titre,
                    Annee = f.Annee,
                    Langue = f.Langue,
                    Duree = f.Duree,
                    Resume = f.Resume,
                    Affiche = f.Affiche,
                    BandeAnnonce = f.BandeAnnonce
                })
                .ToListAsync();
        }

        private static string Normalize(string s) => s.Trim().ToUpper();
    }
}
