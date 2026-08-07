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
        public GelisimselOykuController(GelisimselOykuBusiness business) { _business = business; }

        public IActionResult Ekle(string hastaTC)
        {
            ViewBag.HastaTC = hastaTC;
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