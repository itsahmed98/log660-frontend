using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("ACTEUR")]
public class Acteur
{
    [Key]
    [Column("IDPERSONNE")]
    [ForeignKey("Personne")]
    public int IdPersonne { get; set; }

    public Personne Personne { get; set; } = null!;
    public ICollection<Role> Roles { get; set; } = new List<Role>();
}