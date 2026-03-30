namespace log660_lab2_serveur.Dtos
{
    public class PersonneDto
    {
        public int IdPersonne { get; set; }
        public string Nom { get; set; } = string.Empty;
        public DateTime? DateNaissance { get; set; }
        public string? LieuNaissance { get; set; }
        public string? Photo { get; set; }
        public string? Biographie { get; set; }

        public bool EstActeur { get; set; }
        public bool EstRealisateur { get; set; }
    }
}