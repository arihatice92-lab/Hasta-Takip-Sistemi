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
        private readonly KullaniciBusiness _kullaniciBusiness;
        



        public TedaviController(HastaTedaviBusiness hastaTedaviBusiness, IlacBusiness ilacBusiness, DoktorBusiness doktorBusiness, KullaniciBusiness kullaniciBusiness)
        {
            _hastaTedaviBusiness = hastaTedaviBusiness;
            _ilacBusiness = ilacBusiness;
            _doktorBusiness = doktorBusiness;
            _kullaniciBusiness = kullaniciBusiness;
        }
        private bool BuKayitIcinIslemYapabilirMi(short doktorID)
        {
            if (User.IsInRole("Yönetici"))
            {
                return true;
            }

            var kullaniciAdi = User.Identity?.Name;
            if (string.IsNullOrEmpty(kullaniciAdi))
            {
                return false;
            }

            var kullanici = _kullaniciBusiness.KullaniciGetir(kullaniciAdi);
            return kullanici?.DoktorID == doktorID;
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
            if (!BuKayitIcinIslemYapabilirMi(tedavi.DoktorID))
            {
                TempData["HataMesaji"] = "Bu işlem için yetkiniz yok.";
                return RedirectToAction("Detay", "Hasta", new { tc = tedavi.HastaTC, tab = "taniTedavi" });
            }
            _hastaTedaviBusiness.HastaTedaviEkle(tedavi);
            TempData["BasariMesaji"] = "Tedavi kaydedildi.";
            return RedirectToAction("Detay", "Hasta", new { tc = tedavi.HastaTC, tab = "taniTedavi" });
        }

        
        // GET: /Tedavi/Guncelle/5
        public IActionResult Guncelle(int tedaviID)
        {
            var tedavi = _hastaTedaviBusiness.HastaTedaviGetir(tedaviID);
            if (tedavi == null)
            {
                return NotFound();
            }
            if (!BuKayitIcinIslemYapabilirMi(tedavi.DoktorID))
            {
                TempData["HataMesaji"] = "Bu işlem için yetkiniz yok.";
                return RedirectToAction("Detay", "Hasta", new { tc = tedavi.HastaTC, tab = "taniTedavi" });
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
            var mevcutKayit = _hastaTedaviBusiness.HastaTedaviGetir(tedavi.TedaviID);
            if (mevcutKayit == null) return NotFound();

            if (!BuKayitIcinIslemYapabilirMi(mevcutKayit.DoktorID))
            {
                TempData["HataMesaji"] = "Bu kaydı düzenleme yetkiniz yok.";
                return RedirectToAction("Detay", "Hasta", new { tc = tedavi.HastaTC, tab = "taniTedavi" });
            }
            _hastaTedaviBusiness.HastaTedaviGuncelle(tedavi);
            TempData["BasariMesaji"] = "Tedavi güncellendi.";
            return RedirectToAction("Detay", "Hasta", new { tc = tedavi.HastaTC, tab = "taniTedavi" });
        }
    }
}