using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace WebShop.Controllers
{
    public class KosaricaController : Controller
    {
        private readonly BazaDbContext _context;

        public KosaricaController(BazaDbContext context)
        {
            _context = context;
        }

        private List<Proizvod> GetKosarica()
            => HttpContext.Session.GetObjectFromJson<List<Proizvod>>("Kosarica") ?? new List<Proizvod>();

        private void SaveKosarica(List<Proizvod> kosarica)
            => HttpContext.Session.SetObjectAsJson("Kosarica", kosarica);

        [HttpGet]
        public IActionResult BrojStavki()
        {
            return Json(new { count = GetKosarica().Count });
        }

        public IActionResult Index()
        {
            return View(GetKosarica());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(int id)
        {
            if (HttpContext.Session.GetString("KorisnikUloga") == "Admin")
            {
                TempData["Greska"] = "Kao administrator nemate ovlasti za dodavanje artikala u košaricu.";
                return RedirectToAction("Index", "Proizvodi");
            }

            if (HttpContext.Session.GetString("KorisnikIme") == null)
            {
                TempData["Greska"] = "Morate se prijaviti na svoj račun kako biste dodavali artikle u košaricu.";
                return RedirectToAction("Login", "Racun");
            }

            var proizvod = await _context.Proizvodi.FindAsync(id);
            if (proizvod == null)
            {
                TempData["Greska"] = "Proizvod nije pronađen.";
                return RedirectToAction("Index", "Proizvodi");
            }

            var kosarica = GetKosarica();
            kosarica.Add(proizvod);
            SaveKosarica(kosarica);

            TempData["Poruka"] = $"'{proizvod.Naziv}' je dodano u košaricu.";
            return RedirectToAction("Index", "Proizvodi");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Kupi()
        {
            if (HttpContext.Session.GetString("KorisnikUloga") == "Admin")
                return RedirectToAction("Index", "Proizvodi");

            int? korisnikId = HttpContext.Session.GetInt32("KorisnikID");
            if (korisnikId == null)
            {
                TempData["Greska"] = "Morate se prijaviti kako biste završili kupnju.";
                return RedirectToAction("Login", "Racun");
            }

            var kosarica = GetKosarica();
            if (!kosarica.Any())
                return RedirectToAction("Index");

            var proizvodIds = kosarica.Select(p => p.ProizvodID).Distinct().ToList();
            var proizvodi = await _context.Proizvodi
                .Where(p => proizvodIds.Contains(p.ProizvodID))
                .ToListAsync();

            foreach (var item in kosarica)
            {
                var proizvod = proizvodi.FirstOrDefault(p => p.ProizvodID == item.ProizvodID);
                if (proizvod != null && proizvod.Lager > 0)
                    proizvod.Lager--;
            }

            var narudzba = new Narudzba
            {
                KorisnikID = korisnikId.Value,
                DatumNarudžbe = DateTime.Now,
                UkupnaCijena = kosarica.Sum(p => p.Cijena),
                Stavke = kosarica.Select(p => new NarudzbaStavka
                {
                    ProizvodID = p.ProizvodID,
                    Naziv = p.Naziv ?? string.Empty,
                    Cijena = p.Cijena
                }).ToList()
            };

            _context.Narudžbe.Add(narudzba);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Kosarica");

            return RedirectToAction("Potvrda", new { id = narudzba.NarudzbaID });
        }

        public async Task<IActionResult> Potvrda(int id)
        {
            int? korisnikId = HttpContext.Session.GetInt32("KorisnikID");
            if (korisnikId == null) return RedirectToAction("Login", "Racun");

            var narudzba = await _context.Narudžbe
                .Include(n => n.Stavke)
                .FirstOrDefaultAsync(n => n.NarudzbaID == id && n.KorisnikID == korisnikId);

            if (narudzba == null) return RedirectToAction("Index");

            return View(narudzba);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ocisti()
        {
            HttpContext.Session.Remove("Kosarica");
            return RedirectToAction("Index");
        }

        public IActionResult GenerirajPdf()
        {
            var kosarica = GetKosarica();

            if (!kosarica.Any())
            {
                return RedirectToAction("Index");
            }

            using var document = new PdfDocument();
            document.Info.Title = "WebShop Ponuda";

            XFont fontNaslov = new XFont("Helvetica", 20);
            XFont fontTekst = new XFont("Helvetica", 12);
            XFont fontBold = new XFont("Helvetica#bold", 12);

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            double sirinaStranice = page.Width.Point;
            double visinaStranice = page.Height.Point;
            const double donjaMargina = 80;

            gfx.DrawString("WEBSHOP - SLUŽBENA PONUDA", fontNaslov, XBrushes.Black, new XRect(0, 40, sirinaStranice, 40), XStringFormats.TopCenter);

            string datum = $"Datum ponude: {DateTime.Now:dd.MM.yyyy. HH:mm}";
            gfx.DrawString(datum, fontTekst, XBrushes.DarkGray, 50, 100);

            double trenutniY = 150;
            gfx.DrawString("Naziv proizvoda", fontBold, XBrushes.Black, 50, trenutniY);
            gfx.DrawString("Kategorija", fontBold, XBrushes.Black, 300, trenutniY);
            gfx.DrawString("Cijena", fontBold, XBrushes.Black, 480, trenutniY);
            gfx.DrawLine(XPens.Black, 50, trenutniY + 15, 550, trenutniY + 15);
            trenutniY += 35;

            decimal ukupno = 0;

            foreach (var artikl in kosarica)
            {
                if (trenutniY > visinaStranice - donjaMargina)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    visinaStranice = page.Height.Point;
                    trenutniY = 50;

                    gfx.DrawString("Naziv proizvoda", fontBold, XBrushes.Black, 50, trenutniY);
                    gfx.DrawString("Kategorija", fontBold, XBrushes.Black, 300, trenutniY);
                    gfx.DrawString("Cijena", fontBold, XBrushes.Black, 480, trenutniY);
                    gfx.DrawLine(XPens.Black, 50, trenutniY + 15, 550, trenutniY + 15);
                    trenutniY += 35;
                }

                gfx.DrawString(artikl.Naziv ?? "Proizvod", fontTekst, XBrushes.Black, 50, trenutniY);
                gfx.DrawString(artikl.Kategorija ?? "Općenito", fontTekst, XBrushes.Black, 300, trenutniY);
                gfx.DrawString($"{artikl.Cijena} EUR", fontTekst, XBrushes.Black, 480, trenutniY);

                ukupno += artikl.Cijena;
                trenutniY += 25;
            }

            gfx.DrawLine(XPens.Gray, 50, trenutniY, 550, trenutniY);
            trenutniY += 20;
            gfx.DrawString($"Ukupan iznos za platiti: {ukupno:F2} EUR", fontBold, XBrushes.Black, 350, trenutniY);

            using var stream = new MemoryStream();
            document.Save(stream, false);

            return File(stream.ToArray(), "application/pdf", "WebShop_Ponuda.pdf");
        }

        public async Task<IActionResult> Povijest()
        {
            int? korisnikId = HttpContext.Session.GetInt32("KorisnikID");
            if (korisnikId == null)
            {
                TempData["Greska"] = "Morate se prijaviti kako biste vidjeli povijest narudžbi.";
                return RedirectToAction("Login", "Racun");
            }

            var narudžbe = await _context.Narudžbe
                .Where(n => n.KorisnikID == korisnikId)
                .Include(n => n.Stavke)
                .OrderByDescending(n => n.DatumNarudžbe)
                .ToListAsync();

            return View(narudžbe);
        }

        public async Task<IActionResult> GenerirajPdfNarudzbe(int id)
        {
            int? korisnikId = HttpContext.Session.GetInt32("KorisnikID");
            if (korisnikId == null) return RedirectToAction("Login", "Racun");

            var narudzba = await _context.Narudžbe
                .Include(n => n.Stavke)
                .FirstOrDefaultAsync(n => n.NarudzbaID == id && n.KorisnikID == korisnikId);

            if (narudzba == null) return RedirectToAction("Povijest");

            using var document = new PdfDocument();
            document.Info.Title = $"Narudžba #{narudzba.NarudzbaID}";

            XFont fontNaslov = new XFont("Helvetica", 20);
            XFont fontTekst  = new XFont("Helvetica", 12);
            XFont fontBold   = new XFont("Helvetica#bold", 12);

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            double sirinaStranice  = page.Width.Point;
            double visinaStranice  = page.Height.Point;
            const double donjaMargina = 80;

            gfx.DrawString("TECHGEAR - RAČUN", fontNaslov, XBrushes.Black,
                new XRect(0, 40, sirinaStranice, 40), XStringFormats.TopCenter);

            gfx.DrawString($"Narudžba #{narudzba.NarudzbaID}", fontBold, XBrushes.Black, 50, 95);
            gfx.DrawString($"Datum: {narudzba.DatumNarudžbe:dd.MM.yyyy. HH:mm}", fontTekst, XBrushes.DarkGray, 50, 115);

            double trenutniY = 160;
            DrawTableHeader(gfx, fontBold, trenutniY);
            gfx.DrawLine(XPens.Black, 50, trenutniY + 15, 550, trenutniY + 15);
            trenutniY += 35;

            foreach (var stavka in narudzba.Stavke)
            {
                if (trenutniY > visinaStranice - donjaMargina)
                {
                    page = document.AddPage();
                    gfx  = XGraphics.FromPdfPage(page);
                    visinaStranice = page.Height.Point;
                    trenutniY = 50;
                    DrawTableHeader(gfx, fontBold, trenutniY);
                    gfx.DrawLine(XPens.Black, 50, trenutniY + 15, 550, trenutniY + 15);
                    trenutniY += 35;
                }

                gfx.DrawString(stavka.Naziv, fontTekst, XBrushes.Black, 50, trenutniY);
                gfx.DrawString($"{stavka.Cijena:F2} EUR", fontTekst, XBrushes.Black, 480, trenutniY);
                trenutniY += 25;
            }

            gfx.DrawLine(XPens.Gray, 50, trenutniY, 550, trenutniY);
            trenutniY += 20;
            gfx.DrawString($"Ukupno: {narudzba.UkupnaCijena:F2} EUR", fontBold, XBrushes.Black, 380, trenutniY);

            using var stream = new MemoryStream();
            document.Save(stream, false);

            return File(stream.ToArray(), "application/pdf", $"TechGear_Narudzba_{narudzba.NarudzbaID}.pdf");
        }

        private static void DrawTableHeader(XGraphics gfx, XFont fontBold, double y)
        {
            gfx.DrawString("Naziv proizvoda", fontBold, XBrushes.Black, 50, y);
            gfx.DrawString("Cijena", fontBold, XBrushes.Black, 480, y);
        }
    }
}