using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    [Authorize(Roles = "Yönetici, Sekreter")]
    public class PsikologController : Controller
    {
        private readonly PsikologBusiness _psikologBusiness;

        public PsikologController(PsikologBusiness psikologBusiness)
        {
            _psikologBusiness = psikologBusiness;
        }

        public IActionResult Index(string? ara, string siralama = "AZ", string aktif = "aktif")
        {
            var psikologlar = _psikologBusiness.PsikologAra(ara, siralama, aktif);

            ViewBag.Ara = ara;
            ViewBag.Siralama = siralama;
            ViewBag.Aktif = aktif;

            return View(psikologlar);
        }

        public IActionResult Detay(byte psikologID)
        {
            var psikolog = _psikologBusiness.PsikologGetir(psikologID);
            if (psikolog == null) return NotFound();
            return View(psikolog);
        }

        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(Psikolog psikolog)
        {
            if (!ModelState.IsValid)
            {
                return View(psikolog);
            }

            try
            {
                _psikologBusiness.PsikologEkle(psikolog);
                TempData["BasariMesaji"] = $"{psikolog.PsikologAd} {psikolog.PsikologSoyad} başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(psikolog);
            }
        }

        public IActionResult Guncelle(byte psikologID)
        {
            var psikolog = _psikologBusiness.PsikologGetir(psikologID);
            if (psikolog == null) return NotFound();
            return View(psikolog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(Psikolog psikolog)
        {
            if (!ModelState.IsValid)
            {
                return View(psikolog);
            }

            try
            {
                _psikologBusiness.PsikologGuncelle(psikolog);
                TempData["BasariMesaji"] = "Psikolog bilgileri güncellendi.";
                return RedirectToAction(nameof(Detay), new { psikologID = psikolog.PsikologID });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(psikolog);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ayrilis(byte psikologID)
        {
            _psikologBusiness.PsikologAyrilis(psikologID);
            TempData["BasariMesaji"] = "Psikoloğun ayrılış tarihi işaretlendi.";
            return RedirectToAction(nameof(Detay), new { psikologID });
        }
    }
}