using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("CLIENT")]
public class Client
{
    [Key]
    [Column("IDUTILISATEUR")]
    [ForeignKey("Utilisateur")]
    public int IdUtilisateur { get; set; }

    [Column("NOCARTE")]
    [MaxLength(20)]
    [Required]
    public string NoCarte { get; set; } = string.Empty;

    [Column("CODEFORFAIT")]
    [Required]
    public char CodeForfait { get; set; }

    public Utilisateur Utilisateur { get; set; } = null!;
    public CarteCredit CarteCredit { get; set; }
    public Forfait Forfait { get; set; } = null!;
    public ICollection<Location> Locations { get; set; } = new List<Location>();
}