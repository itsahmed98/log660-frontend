using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("COPIEFILM")]
public class CopieFilm
{
    [Key]
    [Column("IDCOPIE")]
    [MaxLength(30)]
    public string IdCopie { get; set; } = string.Empty;

    [Column("STATUT")]
    [MaxLength(20)]
    [Required]
    public string Statut { get; set; } = string.Empty;

    [Column("IDFILM")]
    [Required]
    public int IdFilm { get; set; }

    public Film Film { get; set; } = null!;
    public Statut StatutCopie { get; set; } = null!;
    public ICollection<Location> Locations { get; set; } = new List<Location>();
}