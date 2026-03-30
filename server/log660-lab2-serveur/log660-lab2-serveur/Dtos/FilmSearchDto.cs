namespace log660_lab2_serveur.Dtos
{
    public class FilmSearchDto
    {
        public int IdFilm { get; set; }
        public string Titre { get; set; } = string.Empty;
        public int Annee { get; set; }
        public string Langue { get; set; } = string.Empty;
        public int Duree { get; set; }
        public string? Resume { get; set; }
        public string? Affiche { get; set; }
        public string? BandeAnnonce { get; set; }
    }
}
