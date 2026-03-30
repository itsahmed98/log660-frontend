using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("GENRE")]
public class Genre
{
    [Key]
    [Column("IDGENRE")]
    public int IdGenre { get; set; }

    [Column("NOMGENRE")]
    [MaxLength(100)]
    [Required]
    public string NomGenre { get; set; } = string.Empty;

    public ICollection<FilmGenre> FilmsGenres { get; set; } = new List<FilmGenre>();
}