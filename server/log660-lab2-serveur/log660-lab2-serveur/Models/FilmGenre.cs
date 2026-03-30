using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("FILM_GENRE")]
public class FilmGenre
{
    [Column("IDFILM")]
    public int IdFilm { get; set; }

    [Column("IDGENRE")]
    public int IdGenre { get; set; }

    public Film Film { get; set; } = null!;
    public Genre Genre { get; set; } = null!;
}