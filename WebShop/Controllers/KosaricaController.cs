using Microsoft.AspNetCore.Mvc;
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

        // GET: /Kosarica/BrojStavki
        [HttpGet]
        public IActionResult BrojStavki()
        {
            return Json(new { count = GetKosarica().Count });
        }

        // GET: /Kosarica/Index
        public IActionResult Index()
        {
            return View(GetKosarica());
        }

        // POST: /Kosarica/Dodaj/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(int id)
        {
            // 1. ZAŠTITA OD ADMINA: Administrator ne smije kupovati
            if (HttpContext.Session.GetString("KorisnikUloga") == "Admin")
            {
                TempData["Greska"] = "Kao administrator nemate ovlasti za dodavanje artikala u košaricu.";
                return RedirectToAction("Index", "Proizvodi");
            }

            // 2. ZAŠTITA OD GOSTA (UBACI_TRECI_KORAK): Ako korisnik nije ulogiran, pošalji ga na login
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

        // POST: /Kosarica/Ocisti
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ocisti()
        {
            HttpContext.Session.Remove("Kosarica");
            return RedirectToAction("Index");
        }

        // GET: /Kosarica/GenerirajPdf
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
            DrawTableHeader(gfx, fontBold, trenutniY);
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

                    DrawTableHeader(gfx, fontBold, trenutniY);
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

        private static void DrawTableHeader(XGraphics gfx, XFont fontBold, double y)
        {
            gfx.DrawString("Naziv proizvoda", fontBold, XBrushes.Black, 50, y);
            gfx.DrawString("Kategorija", fontBold, XBrushes.Black, 300, y);
            gfx.DrawString("Cijena", fontBold, XBrushes.Black, 480, y);
        }
    }
}