namespace log660_lab2_serveur.Dtos
{
    public class FilmDto
    {
        public int IdFilm { get; set; }
        public string Titre { get; set; } = string.Empty;
        public int Annee { get; set; }
        public string Langue { get; set; } = string.Empty;
        public int Duree { get; set; }
        public string? Resume { get; set; }
        public string? Affiche { get; set; }
        public string? BandeAnnonce { get; set; }
        public RealisateurDto Realisateur { get; set; } = null!;
        public List<CopieFilmDto> CopiesFilm { get; set; } = new List<CopieFilmDto>();
        public IEnumerable<FilmGenreDto> FilmsGenres { get; set; } = new List<FilmGenreDto>();
        public IEnumerable<RoleDto> Roles { get; set; } = new List<RoleDto>();
        public IEnumerable<FilmPaysDto> FilmsPays { get; set; } = new List<FilmPaysDto>();
        public IEnumerable<ActeurDto> Acteurs { get; set; } = new List<ActeurDto>();
    }
}