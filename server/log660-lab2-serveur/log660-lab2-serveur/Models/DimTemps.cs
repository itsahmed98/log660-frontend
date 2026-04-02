using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models
{
    [Table("DIM_TEMPS")]
    public class DimTemps
    {
        [Key]
        [Column("ID_DIM_TEMPS")]
        public int IdDimTemps { get; set; }

        [Column("DATE_COMPLETE")]
        public DateTime DateComplete { get; set; }

        [Column("HEURE")]
        public int Heure { get; set; }

        [Column("JOUR_SEMAINE")]
        public string JourSemaine { get; set; } = string.Empty;

        [Column("MOIS_ANNEE")]
        public string MoisAnnee { get; set; } = string.Empty;

        [Column("ANNEE")]
        public int Annee { get; set; }

        public ICollection<FaitLocation> FaitsLocation { get; set; } = new List<FaitLocation>();
    }
}
