using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("PERSONNE")]
public class Personne
{
    [Key]
    [Column("IDPERSONNE")]
    public int IdPersonne { get; set; }

    [Column("NOM")]
    [MaxLength(100)]
    [Required]
    public string Nom { get; set; } = string.Empty;

    [Column("DATENAISSANCE")]
    public DateTime? DateNaissance { get; set; }

    [Column("LIEUNAISSANCE")]
    [MaxLength(100)]
    public string? LieuNaissance { get; set; }

    [Column("PHOTO")]
    [MaxLength(2000)]
    public string? Photo { get; set; }

    [Column("BIOGRAPHIE")]
    public string? Biographie { get; set; }

    public Acteur? Acteur { get; set; }
    public Realisateur? Realisateur { get; set; }
    public Scenariste? Scenariste { get; set; }
}