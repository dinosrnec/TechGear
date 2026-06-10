using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models
{
    [Table("narudzba")]
    public class Narudzba
    {
        [Key]
        public int NarudzbaID { get; set; }

        public int KorisnikID { get; set; }

        public DateTime DatumNarudžbe { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(10,2)")]
        public decimal UkupnaCijena { get; set; }

        [ForeignKey("KorisnikID")]
        public Korisnik? Korisnik { get; set; }

        public List<NarudzbaStavka> Stavke { get; set; } = new();
    }
}
