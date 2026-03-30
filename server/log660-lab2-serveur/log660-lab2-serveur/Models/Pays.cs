using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("PAYS")]
public class Pays
{
    [Key]
    [Column("IDPAYS")]
    public int IdPays { get; set; }

    [Column("NOMPAYS")]
    [MaxLength(100)]
    [Required]
    public string NomPays { get; set; } = string.Empty;

    public ICollection<FilmPays> FilmsPays { get; set; } = new List<FilmPays>();
}