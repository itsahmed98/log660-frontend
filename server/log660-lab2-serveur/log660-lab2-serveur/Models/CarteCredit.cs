using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("CARTECREDIT")]
public class CarteCredit
{
    [Key]
    [Column("NOCARTE")]
    [MaxLength(20)]
    public string NoCarte { get; set; } = string.Empty;

    [Column("TYPE")]
    [MaxLength(20)]
    [Required]
    public string Type { get; set; } = string.Empty;

    [Column("CVV")]
    [MaxLength(4)]
    public string? Cvv { get; set; }

    [Column("EXPMOIS")]
    [Required]
    public int ExpMois { get; set; }

    [Column("EXPANNEE")]
    [Required]
    public int ExpAnnee { get; set; }

    public TypeCarte TypeCarte { get; set; } = null!;
    public Client? Client { get; set; }
}