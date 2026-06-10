using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class WishlistController : Controller
    {
        private readonly BazaDbContext _context;

        public WishlistController(BazaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult BrojStavki()
        {
            int? korisnikId = HttpContext.Session.GetInt32("KorisnikID");
            if (korisnikId == null) return Json(new { count = 0 });
            var count = _context.WishLists.Count(w => w.KorisnikID == korisnikId);
            return Json(new { count });
        }

        [HttpGet]
        public IActionResult MojeId()
        {
            int? korisnikId = HttpContext.Session.GetInt32("KorisnikID");
            if (korisnikId == null) return Json(Array.Empty<int>());
            var ids = _context.WishLists
                .Where(w => w.KorisnikID == korisnikId)
                .Select(w => w.ProizvodID)
                .ToList();
            return Json(ids);
        }

        public IActionResult Index()
        {
            int? korisnikId = HttpContext.Session.GetInt32("KorisnikID");
            if (korisnikId == null)
            {
                return RedirectToAction("Login", "Racun");
            }

            var stavke = _context.WishLists
                                 .Where(w => w.KorisnikID == korisnikId)
                                 .Include(w => w.Proizvod)
                                 .Select(w => w.Proizvod)
                                 .ToList();

            return View(stavke);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DodajUlistu(int id)
        {
            int? korisnikId = HttpContext.Session.GetInt32("KorisnikID");
            if (korisnikId == null)
            {
                return RedirectToAction("Login", "Racun");
            }

            var postoji = _context.WishLists.Any(w => w.KorisnikID == korisnikId && w.ProizvodID == id);
            if (!postoji)
            {
                var novaStavka = new WishList
                {
                    KorisnikID = korisnikId.Value,
                    ProizvodID = id
                };
                _context.WishLists.Add(novaStavka);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Proizvodi");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UkloniIzMape(int id)
        {
            int? korisnikId = HttpContext.Session.GetInt32("KorisnikID");
            if (korisnikId == null)
            {
                return RedirectToAction("Login", "Racun");
            }

            var stavka = _context.WishLists.FirstOrDefault(w => w.KorisnikID == korisnikId && w.ProizvodID == id);
            if (stavka != null)
            {
                _context.WishLists.Remove(stavka);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}