using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models
{
    [Table("FAIT_LOCATION")]
    public class FaitLocation
    {
        [Key]
        [Column("ID_FAIT_LOCATION")]
        public int IdFaitLocation { get; set; }

        [Column("ID_DIM_CLIENT")]
        public int IdDimClient { get; set; }

        [Column("ID_DIM_TEMPS")]
        public int IdDimTemps { get; set; }

        [Column("ID_DIM_FILM")]
        public int IdDimFilm { get; set; }

        [Column("NB_LOCATIONS")]
        public int NbLocations { get; set; } = 1;

        public DimClient DimClient { get; set; } = null!;
        public DimTemps DimTemps { get; set; } = null!;
        public DimFilm DimFilm { get; set; } = null!;
    }
}
