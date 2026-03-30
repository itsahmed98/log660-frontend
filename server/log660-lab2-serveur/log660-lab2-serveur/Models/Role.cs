using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("ROLE")]
public class Role
{
    [Column("IDFILM")]
    public int IdFilm { get; set; }

    [Column("IDACTEUR")]
    public int IdActeur { get; set; }

    [Column("NOMPERSONNAGE")]
    [MaxLength(250)]
    [Required]
    public string NomPersonnage { get; set; } = string.Empty;

    public Film Film { get; set; } = null!;
    public Acteur Acteur { get; set; } = null!;
}