using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("REALISATEUR")]
public class Realisateur
{
    [Key]
    [Column("IDPERSONNE")]
    [ForeignKey("Personne")]
    public int IdPersonne { get; set; }

    public Personne Personne { get; set; } = null!;
    public ICollection<Film> Films { get; set; } = new List<Film>();
}