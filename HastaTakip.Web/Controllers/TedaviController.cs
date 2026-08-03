using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    [Authorize]
    public class TedaviController : Controller
    {
        private readonly HastaTedaviBusiness _hastaTedaviBusiness;
        private readonly IlacBusiness _ilacBusiness;
        private readonly DoktorBusiness _doktorBusiness;

        public TedaviController(HastaTedaviBusiness hastaTedaviBusiness, IlacBusiness ilacBusiness, DoktorBusiness doktorBusiness)
        {
            _hastaTedaviBusiness = hastaTedaviBusiness;
            _ilacBusiness = ilacBusiness;
            _doktorBusiness = doktorBusiness;
        }

        // GET: /Tedavi/Ekle?hastaTC=...
        public IActionResult Ekle(string hastaTC)
        {
            ViewBag.HastaTC = hastaTC;
            ViewBag.IlacListesi = _ilacBusiness.IlaclariListele();
            ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
            return View();
        }

        // POST: /Tedavi/Ekle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(HastaTedavi tedavi)
        {
            _hastaTedaviBusiness.HastaTedaviEkle(tedavi);
            TempData["BasariMesaji"] = "Tedavi kaydedildi.";
            return RedirectToAction("Detay", "Hasta", new { tc = tedavi.HastaTC });
        }

        // GET: /Tedavi/Guncelle/5
        public IActionResult Guncelle(int tedaviID)
        {
            var tedavi = _hastaTedaviBusiness.HastaTedaviGetir(tedaviID);
            if (tedavi == null)
            {
                return NotFound();
            }
            ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
            ViewBag.IlacListesi = _ilacBusiness.IlaclariListele();
            return View(tedavi);
        }

        // POST: /Tedavi/Guncelle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(HastaTedavi tedavi)
        {
            _hastaTedaviBusiness.HastaTedaviGuncelle(tedavi);
            TempData["BasariMesaji"] = "Tedavi güncellendi.";
            return RedirectToAction("Detay", "Hasta", new { tc = tedavi.HastaTC });
        }
    }
}