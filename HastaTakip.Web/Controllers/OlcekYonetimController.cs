using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    [Authorize(Roles = "Yönetici")]
    public class OlcekYonetimController : Controller
    {
        private readonly OlcekBusiness _olcekBusiness;
        public OlcekYonetimController(OlcekBusiness olcekBusiness) { _olcekBusiness = olcekBusiness; }

        public IActionResult Index() => View(_olcekBusiness.OlcekleriListele());

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
    }
}