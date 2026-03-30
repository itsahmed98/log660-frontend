using log660_lab2_serveur.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Dtos
{
    public class RealisateurDto
    {
        public int IdPersonne { get; set; }
        public string Nom { get; set; } = string.Empty;
        public DateTime? DateNaissance { get; set; }
        public string? LieuNaissance { get; set; }
        public string? Photo { get; set; }
        public string? Biographie { get; set; }
    }
}
