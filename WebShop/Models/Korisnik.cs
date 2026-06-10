using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models
{
    [Table("korisnik")] // Povezuje se s tablicom 'korisnik' u XAMPP-u
    public class Korisnik
    {
        [Key]
        public int KorisnikID { get; set; }

        [Required(ErrorMessage = "Email je obavezan")]
        [EmailAddress(ErrorMessage = "Neispravan format email adrese")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lozinka je obavezna")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; } = string.Empty;

        public string? Ime { get; set; }
        public string? Prezime { get; set; }

        // Zadana uloga je 'Kupac', admina ćemo postaviti ručno u bazi ako zatreba
        public string Uloga { get; set; } = "Kupac";
    }
}