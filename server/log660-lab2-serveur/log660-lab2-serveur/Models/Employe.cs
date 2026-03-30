using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("EMPLOYE")]
public class Employe
{
    [Key]
    [Column("IDUTILISATEUR")]
    [ForeignKey("Utilisateur")]
    public int IdUtilisateur { get; set; }

    [Column("MATRICULE")]
    [Required]
    [MaxLength(7)]
    public string Matricule { get; set; } = string.Empty;

    public Utilisateur Utilisateur { get; set; } = null!;
}