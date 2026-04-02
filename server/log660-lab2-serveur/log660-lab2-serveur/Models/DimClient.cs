using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace log660_lab2_serveur.Models
{
    [Table("DIM_CLIENT")]
    public class DimClient
    {
        [Key]
        [Column("ID_DIM_CLIENT")]
        public int IdDimClient { get; set; }

        [Column("NOM_COMPLET")]
        public string NomComplet { get; set; } = string.Empty;

        [Column("GROUPE_AGE")]
        public string GroupeAge { get; set; } = string.Empty;

        [Column("ANCIENNETE")]
        public DateTime? Anciennete { get; set; }

        [Column("CODE_POSTAL")]
        public string CodePostal { get; set; } = string.Empty;

        [Column("VILLE")]
        public string Ville { get; set; } = string.Empty;

        [Column("PROVINCE")]
        public string Province { get; set; } = string.Empty;

        public ICollection<FaitLocation> FaitsLocation { get; set; } = new List<FaitLocation>();
    }
}
