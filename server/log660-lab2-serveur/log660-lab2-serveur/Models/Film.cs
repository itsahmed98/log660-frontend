using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("FILM")]
public class Film
{
    [Key]
    [Column("IDFILM")]
    public int IdFilm { get; set; }

    [Column("TITRE")]
    [MaxLength(200)]
    [Required]
    public string Titre { get; set; } = string.Empty;

    [Column("ANNEE")]
    [Required]
    public int Annee { get; set; }

    [Column("LANGUE")]
    [MaxLength(30)]
    [Required]
    public string Langue { get; set; } = string.Empty;

    [Column("DUREE")]
    [Required]
    public int Duree { get; set; }

    [Column("RESUME")]
    public string? Resume { get; set; }

    [Column("AFFICHE")]
    [MaxLength(200)]
    public string? Affiche { get; set; }

    [Column("BANDEANNONCE")]
    public string? BandeAnnonce { get; set; }

    [Column("IDREALISATEUR")]
    [Required]
    public int IdRealisateur { get; set; }

    public Realisateur Realisateur { get; set; } = null!;
    public ICollection<CopieFilm> CopiesFilm { get; set; } = new List<CopieFilm>();
    public ICollection<FilmPays> FilmsPays { get; set; } = new List<FilmPays>();
    public ICollection<FilmGenre> FilmsGenres { get; set; } = new List<FilmGenre>();
    public ICollection<Role> Roles { get; set; } = new List<Role>();
    public ICollection<FilmScenariste> FilmsScenaristes { get; set; } = new List<FilmScenariste>();
}