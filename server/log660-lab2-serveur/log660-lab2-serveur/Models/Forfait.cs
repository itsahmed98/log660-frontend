using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("FORFAIT")]
public class Forfait
{
    [Key]
    [Column("CODE")]
    [MaxLength(1)]
    public char Code { get; set; }

    [Column("NOMFORFAIT")]
    [MaxLength(20)]
    [Required]
    public string NomForfait { get; set; } = string.Empty;

    [Column("COUTMENSUEL")]
    [Required]
    public decimal CoutMensuel { get; set; }

    [Column("LOCATIONMAX")]
    [Required]
    public int LocationMax { get; set; }

    [Column("DUREEMAX")]
    public int? DureeMax { get; set; }

    public ICollection<Client> Clients { get; set; } = new List<Client>();
}
