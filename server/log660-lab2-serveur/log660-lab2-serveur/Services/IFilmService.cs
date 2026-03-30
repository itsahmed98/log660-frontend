using log660_lab2_serveur.Dtos;

namespace log660_lab2_serveur.Services
{
    public interface IFilmService
    {
        Task<FilmDto?> GetFilmById(int idFilm);
        public Task<IEnumerable<GenreDto>> GetAllGenres();
        public Task<IEnumerable<PaysDto>> GetAllPays();

        public Task<List<FilmSearchDto>> SearchFilmsByQuery(FilmSearchParams param);
    }
}
