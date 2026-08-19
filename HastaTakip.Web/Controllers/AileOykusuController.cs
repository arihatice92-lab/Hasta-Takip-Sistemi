using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HastaTakip.Web.Controllers
{
    [Authorize]
    public class AileOykusuController : Controller
    {
        private readonly AileOykusuBusiness _business;

        private readonly HastaBusiness _hastaBusiness;
        public AileOykusuController(
            AileOykusuBusiness business, 
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
        public IActionResult Ekle(AileOykusu oyku)
        {
            var kullaniciID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            _business.AileOykusuEkle(oyku, kullaniciID);
            TempData["BasariMesaji"] = "Aile öyküsü kaydedildi.";
            return RedirectToAction("Detay", "Hasta", new { tc = oyku.HastaTC, tab = "aileGelisim" });
        }

        public IActionResult Guncelle(int aileOykuID)
        {
            var oyku = _business.AileOykusuGetir(aileOykuID);
            if (oyku == null) return NotFound();
            return View(oyku);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(AileOykusu oyku)
        {
            var kullaniciID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            _business.AileOykusuGuncelle(oyku, kullaniciID);
            TempData["BasariMesaji"] = "Aile öyküsü güncellendi.";
            return RedirectToAction("Detay", "Hasta", new { tc = oyku.HastaTC, tab = "aileGelisim" });
        }
    }
}