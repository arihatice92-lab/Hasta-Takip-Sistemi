using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    [Authorize]
    public class TaniController : Controller
    {
        private readonly HastaTaniBusiness _hastaTaniBusiness;
        private readonly TaniBusiness _taniBusiness;
        private readonly DoktorBusiness _doktorBusiness;

        public TaniController(HastaTaniBusiness hastaTaniBusiness, TaniBusiness taniBusiness, DoktorBusiness doktorBusiness)
        {
            _hastaTaniBusiness = hastaTaniBusiness;
            _taniBusiness = taniBusiness;
            _doktorBusiness = doktorBusiness;
        }

        // GET: /Tani/Ekle?hastaTC=...
        public IActionResult Ekle(string hastaTC)
        {
            ViewBag.HastaTC = hastaTC;
            ViewBag.TaniListesi = _taniBusiness.TanilariListele();
            ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
            return View();
        }

        // POST: /Tani/Ekle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(HastaTani hastaTani)
        {
            _hastaTaniBusiness.HastaTaniEkle(hastaTani);
            TempData["BasariMesaji"] = "Tanı kaydedildi.";
            return RedirectToAction("Detay", "Hasta", new { tc = hastaTani.HastaTC });
        }

        // GET: /Tani/Guncelle/5
        public IActionResult Guncelle(int hastaTaniID)
        {
            var hastaTani = _hastaTaniBusiness.HastaTaniGetir(hastaTaniID);
            if (hastaTani == null)
            {
                return NotFound();
            }
            ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
            ViewBag.TaniListesi = _taniBusiness.TanilariListele();
            return View(hastaTani);
        }

        // POST: /Tani/Guncelle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(HastaTani hastaTani)
        {
            _hastaTaniBusiness.HastaTaniGuncelle(hastaTani);
            TempData["BasariMesaji"] = "Tanı güncellendi.";
            return RedirectToAction("Detay", "Hasta", new { tc = hastaTani.HastaTC });
        }
    }
}