using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("STATUT")]
public class Statut
{
    [Key]
    [Column("STATUT")]
    [MaxLength(20)]
    public string StatutValue { get; set; } = string.Empty;

    public ICollection<CopieFilm> CopiesFilm { get; set; } = new List<CopieFilm>();
}