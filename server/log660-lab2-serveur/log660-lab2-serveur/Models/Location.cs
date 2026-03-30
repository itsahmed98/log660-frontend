using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("LOCATION")]
public class Location
{
    [Column("IDUTILISATEUR")]
    public int IdUtilisateur { get; set; }

    [Column("IDCOPIE")]
    [MaxLength(10)]
    public string IdCopie { get; set; } = string.Empty;

    [Column("DATEDEBUT")]
    [Required]
    public DateTime DateDebut { get; set; }

    [Column("DATERETOUR")]
    public DateTime? DateRetour { get; set; }

    [Column("DATERETOURMAX")]
    public DateTime? DateRetourMax { get; set; }

    public Client Client { get; set; } = null!;
    public CopieFilm CopieFilm { get; set; } = null!;
}