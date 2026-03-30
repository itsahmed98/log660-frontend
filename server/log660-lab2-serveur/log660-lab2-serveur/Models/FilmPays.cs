using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("FILM_PAYS")]
public class FilmPays
{
    [Column("IDFILM")]
    public int IdFilm { get; set; }

    [Column("IDPAYS")]
    public int IdPays { get; set; }

    public Film Film { get; set; } = null!;
    public Pays Pays { get; set; } = null!;
}
