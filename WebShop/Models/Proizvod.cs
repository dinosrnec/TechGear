using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models
{
    [Table("proizvod")]
    public class Proizvod
    {
        [Key]
        [Column("proizvodID")]
        public int ProizvodID { get; set; }

        [Required]
        [StringLength(100)]
        [Column("naziv")]
        public string? Naziv { get; set; }

        [StringLength(500)]
        [Column("opis")]
        public string? Opis { get; set; }

        [Required]
        [Column("cijena", TypeName = "decimal(10,2)")]
        public decimal Cijena { get; set; }

        [StringLength(100)]
        [Column("kategorija")]
        public string? Kategorija { get; set; }

        [StringLength(255)]
        [Column("slikaUrl")]
        public string? SlikaUrl { get; set; }

        [Required]
        [Column("Lager")]
        public int Lager { get; set; }
    }
}