using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models
{
    [Table("narudzba_stavka")]
    public class NarudzbaStavka
    {
        [Key]
        public int NarudzbaStavkaID { get; set; }

        public int NarudzbaID { get; set; }

        public int ProizvodID { get; set; }

        [StringLength(100)]
        public string Naziv { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Cijena { get; set; }

        [ForeignKey("NarudzbaID")]
        public Narudzba? Narudzba { get; set; }
    }
}
