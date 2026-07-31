using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    [Authorize]
    public class DoktorController : Controller
    {
        private readonly DoktorBusiness _doktorBusiness;

        public DoktorController(DoktorBusiness doktorBusiness)
        {
            _doktorBusiness = doktorBusiness;
        }

        // GET: /Doktor
        

        public IActionResult Index(
            string? ara,
            string siralama = "AZ",
            bool? aktif = null,
            string? brans = null)
        {
            // Sayfa ilk kez açıldıysa sadece aktifleri göstersin diye
            if (!Request.Query.ContainsKey("aktif"))
            {
                aktif = true;
            }
            var doktorlar = _doktorBusiness.DoktorAra(
                ara,
                siralama,
                aktif,
                brans);

            ViewBag.Ara = ara;
            ViewBag.Siralama = siralama;
            ViewBag.Aktif = aktif;
            ViewBag.Brans = brans;

            return View(doktorlar);
        }

        // GET: /Doktor/Detay/5
        public IActionResult Detay(short doktorID)
        {
            var doktor = _doktorBusiness.DoktorGetir(doktorID);
            if (doktor == null)
            {
                return NotFound();
            }
            return View(doktor);
        }

        // GET: /Doktor/Ekle
        public IActionResult Ekle()
        {
            return View();
        }

        // POST: /Doktor/Ekle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(Doktor doktor)
        {
            if (!ModelState.IsValid)
            {
                return View(doktor);
            }

            try
            {
                _doktorBusiness.DoktorKaydet(doktor);
                TempData["BasariMesaji"] = $"{doktor.DoktorAd} {doktor.DoktorSoyad} başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(doktor);
            }
        }



        // GET: /Doktor/Guncelle/5
        public IActionResult Guncelle(short doktorID)
        {
            var doktor = _doktorBusiness.DoktorGetir(doktorID);
            if (doktor == null)
            {
                return NotFound();
            }
            return View(doktor);
        }

        // POST: /Doktor/Guncelle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(Doktor doktor)
        {
            if (!ModelState.IsValid)
            {
                return View(doktor);
            }

            _doktorBusiness.DoktorGuncelle(doktor);
            TempData["BasariMesaji"] = "Doktor bilgileri güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Doktor/Ayrilis
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ayrilis(short doktorID)
        {
            _doktorBusiness.DoktorAyrilis(doktorID);
            TempData["BasariMesaji"] = "Doktorun ayrılış tarihi işaretlendi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
