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
        private readonly KullaniciBusiness _kullaniciBusiness;

        private readonly HastaBusiness _hastaBusiness; 
        


        public TaniController(HastaTaniBusiness hastaTaniBusiness, TaniBusiness taniBusiness, DoktorBusiness doktorBusiness, KullaniciBusiness kullaniciBusiness, HastaBusiness hastaBusiness)
        {
            _hastaTaniBusiness = hastaTaniBusiness;
            _taniBusiness = taniBusiness;
            _doktorBusiness = doktorBusiness;
            _kullaniciBusiness = kullaniciBusiness;
            _hastaBusiness = hastaBusiness;
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

        // GET: /Tani/Ekle?hastaTC=...
        public IActionResult Ekle(Guid? hastaGuid, string? hastaTC)
        {

            if (!hastaGuid.HasValue && !string.IsNullOrWhiteSpace(hastaTC))
            {
                var hastaGecici = _hastaBusiness.HastaGetir(hastaTC);
                if (hastaGecici == null) return NotFound();

                return RedirectToAction(nameof(Ekle), new { hastaGuid = hastaGecici.HastaGuid });
            }

            if (!hastaGuid.HasValue) return NotFound();

            var hasta = _hastaBusiness.HastaGetirById(hastaGuid.Value);
            if (hasta == null) return NotFound();

            ViewBag.HastaTC = hastaTC;
            ViewBag.HastaGuid = hastaGuid.Value;
            ViewBag.TaniListesi = _taniBusiness.TanilariListele();
            ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
            if (User.IsInRole("Doktor"))
            {
                var kullaniciAdi = User.Identity?.Name;
                if (!string.IsNullOrEmpty(kullaniciAdi))
                {
                    var kullanici = _kullaniciBusiness.KullaniciGetir(kullaniciAdi);
                    ViewBag.OnSeciliDoktorID = kullanici?.DoktorID;
                }
            }
            return View();
        }

        // POST: /Tani/Ekle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(HastaTani hastaTani)
        {
            if (!BuKayitIcinIslemYapabilirMi(hastaTani.DoktorID))
            {
                TempData["HataMesaji"] = "Bu işlem için yetkiniz yok.";
                return RedirectToAction("Detay", "Hasta", new { tc = hastaTani.HastaTC, tab = "taniTedavi" });
            }

            _hastaTaniBusiness.HastaTaniEkle(hastaTani);
            TempData["BasariMesaji"] = "Tanı kaydedildi.";
            return RedirectToAction("Detay", "Hasta", new { tc = hastaTani.HastaTC, tab = "taniTedavi"});
        }
       
        
        // GET: /Tani/Guncelle/5
        public IActionResult Guncelle(int hastaTaniID)
        {
            var hastaTani = _hastaTaniBusiness.HastaTaniGetir(hastaTaniID);
            if (hastaTani == null)
            {
                return NotFound();
            }
            if(!BuKayitIcinIslemYapabilirMi(hastaTani.DoktorID))
            {
                TempData["HataMesaji"] = "Bu kaydı düzenleme yetkiniz yok.";
                return RedirectToAction("Detay", "Hasta", new { tc = hastaTani.HastaTC, tab = "taniTedavi" });
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
            var mevcutKayit = _hastaTaniBusiness.HastaTaniGetir(hastaTani.HastaTaniID);
            if (mevcutKayit == null) return NotFound();

            if (!BuKayitIcinIslemYapabilirMi(mevcutKayit.DoktorID))
            {
                TempData["HataMesaji"] = "Bu kaydı düzenleme yetkiniz yok.";
                return RedirectToAction("Detay", "Hasta", new { tc = hastaTani.HastaTC, tab = "taniTedavi" });
            }
            _hastaTaniBusiness.HastaTaniGuncelle(hastaTani);
            TempData["BasariMesaji"] = "Tanı güncellendi.";
            return RedirectToAction("Detay", "Hasta", new { tc = hastaTani.HastaTC, tab = "taniTedavi" });
        }
    }
}