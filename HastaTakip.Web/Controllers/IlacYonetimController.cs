using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    [Authorize(Roles = "Yönetici, Sekreter")]
    public class IlacYonetimController : Controller
    {
        private readonly IlacBusiness _ilacBusiness;
        public IlacYonetimController(IlacBusiness ilacBusiness) { _ilacBusiness = ilacBusiness; }

        public IActionResult Index(string? ara, string aktif)
        {
            ViewBag.Ara = ara;
            ViewBag.Aktif = aktif;
            var ilaclar = _ilacBusiness.IlacAra(ara, aktif);
            return View(ilaclar);
        }
        

        public IActionResult Ekle() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(Ilac ilac)
        {
            try
            {
                _ilacBusiness.IlacEkle(ilac);
                TempData["BasariMesaji"] = "İlaç eklendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(ilac);
            }
        }

        public IActionResult Guncelle(short ilacID)
        {
            var ilac = _ilacBusiness.IlacGetir(ilacID);
            if (ilac == null) return NotFound();
            return View(ilac);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(Ilac ilac)
        {
            _ilacBusiness.IlacGuncelle(ilac);
            TempData["BasariMesaji"] = "İlaç bilgileri güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PasifeAl(short ilacID)
        {
            _ilacBusiness.IlacPasifeAl(ilacID);
            TempData["BasariMesaji"] = "İlaç pasife alındı.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AktifEt(short ilacID)
        {
            _ilacBusiness.IlacAktifEt(ilacID);
            TempData["BasariMesaji"] = "İlaç aktife alındı.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Sil(short ilacID)
        {
            try
            {
                _ilacBusiness.IlacSil(ilacID);
                TempData["BasariMesaji"] = "İlaç silindi.";
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}