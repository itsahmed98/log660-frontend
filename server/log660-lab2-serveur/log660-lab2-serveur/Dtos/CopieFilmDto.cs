using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Dtos
{
    public class CopieFilmDto
    {
        public string IdCopie { get; set; } = string.Empty;
        public string Statut { get; set; } = string.Empty;
        public int IdFilm { get; set; }
    }
}
