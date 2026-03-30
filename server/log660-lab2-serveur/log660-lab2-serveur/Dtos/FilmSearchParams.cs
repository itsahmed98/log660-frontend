namespace log660_lab2_serveur.Dtos
{
    public class FilmSearchParams
    {
        public string? Titre { get; set; }

        public string? Realisateur { get; set; }
        public string? Acteur { get; set; }

        public int? IdGenre { get; set; }
        public int? IdPays { get; set; }

        public string? Langue { get; set; }

        public int? MinAnnee { get; set; }
        public int? MaxAnnee { get; set; }
    }
}
