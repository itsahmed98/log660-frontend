using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("FILM_SCENARISTE")]
public class FilmScenariste
{
    [Column("IDFILM")]
    public int IdFilm { get; set; }

    [Column("IDSCENARISTE")]
    public int IdScenariste { get; set; }

    public Film Film { get; set; } = null!;
    public Scenariste Scenariste { get; set; } = null!;
}