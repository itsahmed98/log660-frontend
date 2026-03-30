namespace log660_lab2_serveur.Dtos
{
    public class RoleDto
    {
        public int IdFilm { get; set; }
        public int IdActeur { get; set; }
        public string NomPersonnage { get; set; } = string.Empty;

        public int IdPersonne { get; set; }
        public string NomActeur { get; set; } = string.Empty;
        public string? PhotoActeur { get; set; }
    }
}