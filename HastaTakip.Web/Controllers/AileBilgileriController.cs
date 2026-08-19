using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HastaTakip.Web.Controllers
{
    [Authorize]
    public class AileBilgileriController : Controller
    {
        private readonly AileBilgileriBusiness _business;

        private readonly HastaBusiness _hastaBusiness;
        public AileBilgileriController(
            AileBilgileriBusiness business, 
            HastaBusiness hastaBusiness)
        {
            _business = business;
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
        public IActionResult Ekle(AileBilgileri bilgi)
        {
            var kullaniciID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            _business.AileBilgileriEkle(bilgi, kullaniciID);
            TempData["BasariMesaji"] = "Aile bilgileri kaydedildi.";
            return RedirectToAction("Detay", "Hasta", new { tc = bilgi.HastaTC, tab = "aileGelisim" });
        }

        public IActionResult Guncelle(int aileBilgileriID)
        {
            var bilgi = _business.AileBilgileriGetir(aileBilgileriID);
            if (bilgi == null) return NotFound();
            return View(bilgi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(AileBilgileri bilgi)
        {
            var kullaniciID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            _business.AileBilgileriGuncelle(bilgi, kullaniciID);
            TempData["BasariMesaji"] = "Aile bilgileri güncellendi.";
            return RedirectToAction("Detay", "Hasta", new { tc = bilgi.HastaTC, tab = "aileGelisim" });
        }
    }
}