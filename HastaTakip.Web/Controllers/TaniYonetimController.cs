using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    [Authorize(Roles = "Yönetici, Sekreter")]
    public class TaniYonetimController : Controller
    {
        private readonly TaniBusiness _taniBusiness;
        public TaniYonetimController(TaniBusiness taniBusiness) { _taniBusiness = taniBusiness; }

        public IActionResult Index(string? ara, string aktif = "aktif")
        {
            ViewBag.Ara = ara;
            ViewBag.Aktif = aktif;
            var tanilar = _taniBusiness.TaniAra(ara, aktif);
            return View(tanilar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PasifeAl(short taniID)
        {
            _taniBusiness.TaniPasifeAl(taniID);
            TempData["BasariMesaji"] = "Tanı pasife alındı.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AktifEt(short taniID)
        {
            _taniBusiness.TaniAktifEt(taniID);
            TempData["BasariMesaji"] = "Tanı aktife alındı.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Ekle() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(Tani tani)
        {
            try
            {
                _taniBusiness.TaniEkle(tani);
                TempData["BasariMesaji"] = "Tanı eklendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(tani);
            }
        }

        public IActionResult Guncelle(short taniID)
        {
            var tani = _taniBusiness.TaniGetir(taniID);
            if (tani == null) return NotFound();
            return View(tani);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(Tani tani)
        {
            _taniBusiness.TaniGuncelle(tani);
            TempData["BasariMesaji"] = "Tanı güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Sil(short taniID)
        {
            try
            {
                _taniBusiness.TaniSil(taniID);
                TempData["BasariMesaji"] = "Tanı silindi.";
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}