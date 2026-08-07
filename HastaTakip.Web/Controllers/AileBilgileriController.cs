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
        public AileBilgileriController(AileBilgileriBusiness business) { _business = business; }

        public IActionResult Ekle(string hastaTC)
        {
            ViewBag.HastaTC = hastaTC;
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