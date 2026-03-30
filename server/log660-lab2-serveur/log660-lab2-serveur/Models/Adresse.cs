using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("ADRESSE")]
public class Adresse
{
    [Key]
    [Column("IDUTILISATEUR")]
    [ForeignKey("Utilisateur")]
    public int IdUtilisateur { get; set; }

    [Column("NUMCIVIC")]
    [MaxLength(20)]
    [Required]
    public string NumCivic { get; set; } = string.Empty;

    [Column("RUE")]
    [MaxLength(100)]
    [Required]
    public string Rue { get; set; } = string.Empty;

    [Column("VILLE")]
    [MaxLength(100)]
    [Required]
    public string Ville { get; set; } = string.Empty;

    [Column("PROVINCE")]
    [MaxLength(40)]
    [Required]
    public string Province { get; set; } = string.Empty;

    [Column("CODEPOSTAL")]
    [MaxLength(10)]
    [Required]
    public string CodePostal { get; set; } = string.Empty;

    public Utilisateur Utilisateur { get; set; } = null!;
}