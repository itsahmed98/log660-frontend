using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Dtos
{
    public class GenreDto
    {
        public int IdGenre { get; set; }
        public string NomGenre { get; set; } = string.Empty;
    }
}
