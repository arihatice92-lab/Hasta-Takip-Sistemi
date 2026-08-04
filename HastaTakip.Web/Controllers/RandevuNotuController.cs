using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    [Authorize]
    public class RandevuNotuController : Controller
    {
        private readonly RandevuNotuBusiness _randevuNotuBusiness;
        private readonly RandevuBusiness _randevuBusiness;

        public RandevuNotuController(RandevuNotuBusiness randevuNotuBusiness, RandevuBusiness randevuBusiness)
        {
            _randevuNotuBusiness = randevuNotuBusiness;
            _randevuBusiness = randevuBusiness;
        }

        // GET: /RandevuNotu/Ekle?randevuTarihID=5
        public IActionResult Ekle(int randevuTarihID)
        {
            var randevu = _randevuBusiness.RandevuGetir(randevuTarihID);
            if (randevu == null)
            {
                return NotFound();
            }

            ViewBag.RandevuTarihID = randevuTarihID;
            ViewBag.HastaTC = randevu.HastaTC;
            ViewBag.DoktorID = randevu.DoktorID;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(RandevuNotu notu)
        {
            _randevuNotuBusiness.RandevuNotuEkle(notu);
            TempData["BasariMesaji"] = "Randevu notu kaydedildi.";
            return RedirectToAction("Detay", "Randevu", new { randevuTarihID = notu.RandevuTarihID });
        }

        public IActionResult Guncelle(short randevuNotID)
        {
            var notu = _randevuNotuBusiness.RandevuNotuGetir(randevuNotID);
            if (notu == null)
            {
                return NotFound();
            }
            return View(notu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(RandevuNotu notu)
        {
            _randevuNotuBusiness.RandevuNotuGuncelle(notu);
            TempData["BasariMesaji"] = "Randevu notu güncellendi.";
            return RedirectToAction("Detay", "Randevu", new { randevuTarihID = notu.RandevuTarihID });
        }
    }
}