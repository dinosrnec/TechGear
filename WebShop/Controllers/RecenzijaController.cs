using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class RecenzijaController : Controller
    {
        private readonly BazaDbContext _context;

        public RecenzijaController(BazaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var recenzije = await _context.Recenzije
                .OrderByDescending(r => r.DatumKreiranja)
                .ToListAsync();

            double prosjek = recenzije.Any() ? recenzije.Average(r => r.Ocjena) : 0;
            ViewBag.Prosjek = prosjek;
            ViewBag.Ukupno = recenzije.Count;

            return View(recenzije);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(string tekst, int ocjena)
        {
            string? ime = HttpContext.Session.GetString("KorisnikIme");
            if (ime == null)
            {
                TempData["Greska"] = "Morate biti prijavljeni kako biste ostavili recenziju.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(tekst) || ocjena < 1 || ocjena > 5)
            {
                TempData["Greska"] = "Unesite tekst recenzije i odaberite ocjenu od 1 do 5.";
                return RedirectToAction("Index");
            }

            var recenzija = new Recenzija
            {
                KorisnikIme   = ime,
                Tekst         = tekst.Trim(),
                Ocjena        = ocjena,
                DatumKreiranja = DateTime.Now
            };

            _context.Recenzije.Add(recenzija);
            await _context.SaveChangesAsync();

            TempData["Poruka"] = "Hvala! Vaša recenzija je objavljena.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Obrisi(int id)
        {
            if (HttpContext.Session.GetString("KorisnikUloga") != "Admin")
                return RedirectToAction("Index");

            var r = await _context.Recenzije.FindAsync(id);
            if (r != null)
            {
                _context.Recenzije.Remove(r);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
