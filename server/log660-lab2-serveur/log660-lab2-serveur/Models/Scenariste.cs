using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("SCENARISTE")]
public class Scenariste
{
    [Key]
    [Column("IDPERSONNE")]
    [ForeignKey("Personne")]
    public int IdPersonne { get; set; }

    public Personne Personne { get; set; } = null!;
    public ICollection<FilmScenariste> FilmsEcrits { get; set; } = new List<FilmScenariste>();
}