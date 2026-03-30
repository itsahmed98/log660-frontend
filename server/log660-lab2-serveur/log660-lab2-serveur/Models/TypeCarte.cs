using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("TYPECARTE")]
public class TypeCarte
{
    [Key]
    [Column("TYPE")]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    public ICollection<CarteCredit> CartesCredit { get; set; } = new List<CarteCredit>();
}