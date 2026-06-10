using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class ProizvodiController : Controller
    {
        private readonly BazaDbContext _context;

        public ProizvodiController(BazaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder, string kategorija)
        {
            ViewData["TrenutnaPretraga"] = searchString;
            ViewData["TrenutnoSortiranje"] = sortOrder;
            ViewData["TrenutnaKategorija"] = kategorija;

            var proizvodi = from p in _context.Proizvodi select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                proizvodi = proizvodi.Where(s => s.Naziv!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(kategorija))
            {
                proizvodi = proizvodi.Where(s => s.Kategorija == kategorija);
            }

            switch (sortOrder)
            {
                case "cijena_asc":
                    proizvodi = proizvodi.OrderBy(p => p.Cijena);
                    break;
                case "cijena_desc":
                    proizvodi = proizvodi.OrderByDescending(p => p.Cijena);
                    break;
                default:
                    proizvodi = proizvodi.OrderBy(p => p.Naziv);
                    break;
            }

            ViewBag.SveKategorije = await _context.Proizvodi
                .Select(p => p.Kategorija)
                .Distinct()
                .ToListAsync();

            return View(await proizvodi.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var proizvod = await _context.Proizvodi.FirstOrDefaultAsync(m => m.ProizvodID == id);
            if (proizvod == null) return NotFound();

            return View(proizvod);
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("KorisnikUloga") != "Admin")
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Proizvod proizvod)
        {
            if (HttpContext.Session.GetString("KorisnikUloga") != "Admin")
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                _context.Add(proizvod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proizvod);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("KorisnikUloga") != "Admin")
            {
                return RedirectToAction("Index");
            }

            if (id == null) return NotFound();

            var proizvod = await _context.Proizvodi.FindAsync(id);
            if (proizvod == null) return NotFound();

            return View(proizvod);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Proizvod proizvod)
        {
            if (HttpContext.Session.GetString("KorisnikUloga") != "Admin")
            {
                return RedirectToAction("Index");
            }

            if (id != proizvod.ProizvodID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proizvod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProizvodExists(proizvod.ProizvodID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(proizvod);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("KorisnikUloga") != "Admin")
            {
                return RedirectToAction("Index");
            }

            var proizvod = await _context.Proizvodi.FindAsync(id);

            if (proizvod != null)
            {
                _context.Proizvodi.Remove(proizvod);
                await _context.SaveChangesAsync();
                TempData["Poruka"] = "Artikl je uspješno obrisan iz kataloga.";
            }
            else
            {
                TempData["Greska"] = "Proizvod nije pronađen.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProizvodExists(int id)
        {
            return _context.Proizvodi.Any(e => e.ProizvodID == id);
        }
    }
}