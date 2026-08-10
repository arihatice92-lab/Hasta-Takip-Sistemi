using HastaTakip.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HastaTakip.Web.Controllers
{
    [Authorize]
    public class KayitNotuController : Controller
    {
        private readonly KayitNotuBusiness _business;
        public KayitNotuController(KayitNotuBusiness business) { _business = business; }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(string kayitTuru, int kayitID, string notMetni, string hastaTC)
        {
            var kullaniciID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            _business.NotEkle(kayitTuru, kayitID, kullaniciID, notMetni);
            TempData["BasariMesaji"] = "Not eklendi.";
            return RedirectToAction("Detay", "Hasta", new { tc = hastaTC, tab = "aileGelisim" });
        }
    }
}