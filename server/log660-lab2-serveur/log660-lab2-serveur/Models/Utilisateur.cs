using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models;

[Table("UTILISATEUR")]
public class Utilisateur
{
    [Key]
    [Column("IDUTILISATEUR")]
    public int IdUtilisateur { get; set; }

    [Column("NOM")]
    [MaxLength(20)]
    [Required]
    public string Nom { get; set; } = string.Empty;

    [Column("PRENOM")]
    [MaxLength(20)]
    [Required]
    public string Prenom { get; set; } = string.Empty;

    [Column("COURRIEL")]
    [MaxLength(50)]
    [Required]
    public string Courriel { get; set; } = string.Empty;

    [Column("TELEPHONE")]
    [MaxLength(20)]
    [Required]
    public string Telephone { get; set; } = string.Empty;

    [Column("DATENAISSANCE")]
    [Required]
    public DateTime DateNaissance { get; set; }

    [Column("MOTDEPASSE")]
    [MaxLength(30)]
    [Required]
    public string MotDePasse { get; set; } = string.Empty;

    public Adresse? Adresse { get; set; }
    public Client? Client { get; set; }
    public Employe? Employe { get; set; }
}