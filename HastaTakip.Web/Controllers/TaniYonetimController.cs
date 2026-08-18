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

        public IActionResult Index() => View(_taniBusiness.TanilariListele());

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