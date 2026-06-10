using Microsoft.AspNetCore.Mvc;

namespace WebShop.Controllers
{
    public class KontaktController : Controller
    {
        public IActionResult Index() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Posalji(string ime, string email, string poruka)
        {
            if (string.IsNullOrWhiteSpace(ime) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(poruka))
            {
                TempData["Greska"] = "Sva polja su obavezna.";
                return RedirectToAction("Index");
            }

            TempData["Poruka"] = $"Hvala, {ime.Trim()}! Vaša poruka je primljena. Javit ćemo se na {email.Trim()}.";
            return RedirectToAction("Index");
        }
    }
}
