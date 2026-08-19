using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HastaTakip.Web.Controllers
{
    [Authorize]
    public class GelisimselOykuController : Controller
    {
        private readonly GelisimselOykuBusiness _business;
        private readonly HastaBusiness _hastaBusiness;
        public GelisimselOykuController(
            GelisimselOykuBusiness business,
            HastaBusiness hastaBusiness) 
        {   _business = business; 
            _hastaBusiness = hastaBusiness;
        
        }

        public IActionResult Ekle(Guid? hastaGuid, string? hastaTC)
        {
            if (!hastaGuid.HasValue && !string.IsNullOrWhiteSpace(hastaTC))
            {
                var hastaGecici = _hastaBusiness.HastaGetir(hastaTC);
                if (hastaGecici == null) return NotFound();

                return RedirectToAction(nameof(Ekle), new { hastaGuid = hastaGecici.HastaGuid });
            }

            if (!hastaGuid.HasValue) return NotFound();

            var hasta = _hastaBusiness.HastaGetirById(hastaGuid.Value);
            if (hasta == null) return NotFound();

            ViewBag.HastaTC = hastaTC;
            ViewBag.HastaGuid = hastaGuid.Value;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(GelisimselOyku oyku)
        {
            var kullaniciID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            _business.GelisimselOykuEkle(oyku, kullaniciID);
            TempData["BasariMesaji"] = "Gelişimsel öykü kaydedildi.";
            return RedirectToAction("Detay", "Hasta", new { tc = oyku.HastaTC, tab = "aileGelisim" });
        }

        public IActionResult Guncelle(int gelisimOykuID)
        {
            var oyku = _business.GelisimselOykuGetir(gelisimOykuID);
            if (oyku == null) return NotFound();
            return View(oyku);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(GelisimselOyku oyku)
        {
            var kullaniciID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            _business.GelisimselOykuGuncelle(oyku, kullaniciID);
            TempData["BasariMesaji"] = "Gelişimsel öykü güncellendi.";
            return RedirectToAction("Detay", "Hasta", new { tc = oyku.HastaTC, tab = "aileGelisim" });
        }
    }
}