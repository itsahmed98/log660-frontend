namespace log660_lab2_serveur.Dtos
{
    public class ActeurDto
    {
        public int IdPersonne { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public string NomPersonnage { get; set; } = string.Empty;
    }
}