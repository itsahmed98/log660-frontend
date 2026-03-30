namespace log660_lab2_serveur.Dtos
{
    public class UtilisateurDto
    {
        public int IdUtilisateur { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Courriel { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public DateTime DateNaissance { get; set; }
    }
}
