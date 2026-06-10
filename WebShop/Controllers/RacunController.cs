using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class RacunController : Controller
    {
        private readonly BazaDbContext _context;

        public RacunController(BazaDbContext context)
        {
            _context = context;
        }

        public IActionResult Registracija()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registracija(Korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                _context.Korisnici.Add(korisnik);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(korisnik);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string lozinka)
        {
            var korisnik = await _context.Korisnici
                .FirstOrDefaultAsync(k => k.Email == email && k.Lozinka == lozinka);

            if (korisnik != null)
            {
                HttpContext.Session.SetString("KorisnikIme", korisnik.Ime ?? korisnik.Email);
                HttpContext.Session.SetString("KorisnikUloga", korisnik.Uloga ?? "Kupac");
                HttpContext.Session.SetInt32("KorisnikID", korisnik.KorisnikID);

                return RedirectToAction("Index", "Proizvodi");
            }

            ModelState.AddModelError("", "Neispravan email ili lozinka.");
            return View();
        }

        public IActionResult Odjava()
        {
            HttpContext.Session.Remove("KorisnikIme");
            HttpContext.Session.Remove("KorisnikUloga");
            HttpContext.Session.Remove("KorisnikID");
            return RedirectToAction("Index", "Home");
        }
    }
}