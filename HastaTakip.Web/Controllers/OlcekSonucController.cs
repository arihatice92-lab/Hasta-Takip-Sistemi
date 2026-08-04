using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    [Authorize]
    public class OlcekSonucController : Controller
    {
        private readonly HastaOlcekSonucBusiness _olcekSonucBusiness;
        private readonly OlcekBusiness _olcekBusiness;
        private readonly DoktorBusiness _doktorBusiness;

        public OlcekSonucController(HastaOlcekSonucBusiness olcekSonucBusiness, OlcekBusiness olcekBusiness, DoktorBusiness doktorBusiness)
        {
            _olcekSonucBusiness = olcekSonucBusiness;
            _olcekBusiness = olcekBusiness;
            _doktorBusiness = doktorBusiness;
        }

        public IActionResult Ekle(string hastaTC)
        {
            ViewBag.HastaTC = hastaTC;
            ViewBag.OlcekListesi = _olcekBusiness.OlcekleriListele();
            ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(OlcekSonuc olcekSonuc)
        {
            _olcekSonucBusiness.OlcekSonucEkle(olcekSonuc);
            TempData["BasariMesaji"] = "Ölçek sonucu kaydedildi.";
            return RedirectToAction("Detay", "Hasta", new { tc = olcekSonuc.HastaTC, tab = "testOlcek" });
        }

        public IActionResult Guncelle(int olcekSonucID)
        {
            var olcekSonuc = _olcekSonucBusiness.OlcekSonucGetir(olcekSonucID);
            if (olcekSonuc == null) return NotFound();
            ViewBag.OlcekListesi = _olcekBusiness.OlcekleriListele();
            ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
            return View(olcekSonuc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(OlcekSonuc olcekSonuc)
        {
            _olcekSonucBusiness.OlcekSonucGuncelle(olcekSonuc);
            TempData["BasariMesaji"] = "Ölçek sonucu güncellendi.";
            return RedirectToAction("Detay", "Hasta", new { tc = olcekSonuc.HastaTC, tab = "testOlcek" });
        }
    }
}
