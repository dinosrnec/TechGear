using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models
{
    [Table("recenzija")]
    public class Recenzija
    {
        [Key]
        [Column("recenzijaID")]
        public int RecenzijaID { get; set; }

        [Required]
        [StringLength(100)]
        [Column("korisnikIme")]
        public string KorisnikIme { get; set; } = "";

        [Required]
        [StringLength(1000)]
        [Column("tekst")]
        public string Tekst { get; set; } = "";

        [Required]
        [Range(1, 5)]
        [Column("ocjena")]
        public int Ocjena { get; set; }

        [Column("datumKreiranja")]
        public DateTime DatumKreiranja { get; set; }
    }
}
