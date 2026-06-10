using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models
{
    [Table("wishlist")]
    public class WishList
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("korisnikID")]
        public int KorisnikID { get; set; }

        [Column("proizvodID")]
        public int ProizvodID { get; set; }

        [ForeignKey("ProizvodID")]
        public virtual Proizvod? Proizvod { get; set; }
    }
}