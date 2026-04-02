using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models
{
    [Table("DIM_FILM")]
    public class DimFilm
    {
        [Key]
        [Column("ID_DIM_FILM")]
        public int IdDimFilm { get; set; }

        [Column("TITRE_COMPLET")]
        public string TitreComplet { get; set; } = string.Empty;

        [Column("ANNEE_SORTIE")]
        public int AnneeSortie { get; set; }

        [Column("ORIGINE")]
        public string Origine { get; set; } = string.Empty;

        [Column("IS_ACTION")] public int IsAction { get; set; }
        [Column("IS_ADVENTURE")] public int IsAdventure { get; set; }
        [Column("IS_COMEDY")] public int IsComedy { get; set; }
        [Column("IS_FAMILY")] public int IsFamily { get; set; }
        [Column("IS_ROMANCE")] public int IsRomance { get; set; }
        [Column("IS_DRAMA")] public int IsDrama { get; set; }
        [Column("IS_ANIMATION")] public int IsAnimation { get; set; }
        [Column("IS_FANTASY")] public int IsFantasy { get; set; }
        [Column("IS_BIOGRAPHY")] public int IsBiography { get; set; }
        [Column("IS_THRILLER")] public int IsThriller { get; set; }
        [Column("IS_SCIFI")] public int IsSciFi { get; set; }
        [Column("IS_CRIME")] public int IsCrime { get; set; }
        [Column("IS_SPORT")] public int IsSport { get; set; }
        [Column("IS_HORROR")] public int IsHorror { get; set; }
        [Column("IS_FILM_NOIR")] public int IsFilmNoir { get; set; }
        [Column("IS_MYSTERY")] public int IsMystery { get; set; }
        [Column("IS_WESTERN")] public int IsWestern { get; set; }
        [Column("IS_WAR")] public int IsWar { get; set; }
        [Column("IS_MUSICAL")] public int IsMusical { get; set; }
        [Column("IS_DOCUMENTARY")] public int IsDocumentary { get; set; }
        [Column("IS_HISTORY")] public int IsHistory { get; set; }
        [Column("IS_MUSIC")] public int IsMusic { get; set; }

        public ICollection<FaitLocation> FaitsLocation { get; set; } = new List<FaitLocation>();
    }
}
