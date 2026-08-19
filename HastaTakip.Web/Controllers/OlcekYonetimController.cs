using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    [Authorize(Roles = "Yönetici, Sekreter")]
    public class OlcekYonetimController : Controller
    {
        private readonly OlcekBusiness _olcekBusiness;
        public OlcekYonetimController(OlcekBusiness olcekBusiness) { _olcekBusiness = olcekBusiness; }

        public IActionResult Index(string? ara, string aktif = "aktif")
        {
            ViewBag.Ara = ara;
            ViewBag.Aktif = aktif;
            var olcekler = _olcekBusiness.OlcekAra(ara, aktif);
            return View(olcekler);
        }

        public IActionResult Ekle() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(Olcek olcek)
        {
            try
            {
                _olcekBusiness.OlcekEkle(olcek);
                TempData["BasariMesaji"] = "Ölçek eklendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(olcek);
            }
        }

        public IActionResult Guncelle(byte olcekID)
        {
            var olcek = _olcekBusiness.OlcekGetir(olcekID);
            if (olcek == null) return NotFound();
            return View(olcek);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(Olcek olcek)
        {
            _olcekBusiness.OlcekGuncelle(olcek);
            TempData["BasariMesaji"] = "Ölçek güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Sil(byte olcekID)
        {
            try
            {
                _olcekBusiness.OlcekSil(olcekID);
                TempData["BasariMesaji"] = "Ölçek silindi.";
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PasifeAl(byte olcekID)
        {
            _olcekBusiness.OlcekPasifeAl(olcekID);
            TempData["BasariMesaji"] = "Ölçek pasife alındı.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AktifEt(byte olcekID)
        {
            _olcekBusiness.OlcekAktifEt(olcekID);
            TempData["BasariMesaji"] = "Ölçek aktife alındı.";
            return RedirectToAction(nameof(Index));
        }
    }
}