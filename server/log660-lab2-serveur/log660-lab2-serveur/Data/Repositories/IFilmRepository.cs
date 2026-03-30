using log660_lab2_serveur.Dtos;
using log660_lab2_serveur.Models;

namespace log660_lab2_serveur.Data.Repositories
{
    public interface IFilmRepository
    {
        Task<Film> GetFilmById(int idFilm);
        public Task<IEnumerable<Genre>> GetAllGenres();
        public Task<Genre> GetGenre(int idGenre);
        public Task<IEnumerable<Pays>> GetAllPays();
        public Task<List<FilmSearchDto>> SearchFilmsByQuery(FilmSearchParams query);
        public Task<Personne> GetRealisateur(int idRealisateur);
        public Task<IEnumerable<Role>> GetActeurs(int idActeur);
        public Task<IEnumerable<CopieFilm>> GetCopiesFilm(int idFilm);
        public Task<IEnumerable<FilmGenre>> GetGenresFilm(int idFilm);
    }
}
