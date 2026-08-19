using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    [Authorize]
    public class OlcekSonucController : Controller
    {
        private readonly HastaOlcekSonucBusiness _olcekSonucBusiness;
        private readonly OlcekBusiness _olcekBusiness;
        private readonly DoktorBusiness _doktorBusiness;
        private readonly KullaniciBusiness _kullaniciBusiness;

        private readonly HastaBusiness _hastaBusiness;

        public OlcekSonucController(HastaOlcekSonucBusiness olcekSonucBusiness, OlcekBusiness olcekBusiness, DoktorBusiness doktorBusiness, KullaniciBusiness kullaniciBusiness, HastaBusiness hastaBusiness)
        {
            _olcekSonucBusiness = olcekSonucBusiness;
            _olcekBusiness = olcekBusiness;
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
            ViewBag.HastaGuid = hastaGuid;
            ViewBag.OlcekListesi = _olcekBusiness.OlcekleriListele();
            ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(OlcekSonuc olcekSonuc)
        {
            if (!BuKayitIcinIslemYapabilirMi(olcekSonuc.DoktorID))
            {
                TempData["HataMesaji"] = "Bu işlem için yetkiniz yok.";
                return RedirectToAction("Detay", "Hasta", new { tc = olcekSonuc.HastaTC, tab = "testOlcek" });
            }
            
            _olcekSonucBusiness.OlcekSonucEkle(olcekSonuc);
            TempData["BasariMesaji"] = "Ölçek sonucu kaydedildi.";
            return RedirectToAction("Detay", "Hasta", new { tc = olcekSonuc.HastaTC, tab = "testOlcek" });
        }

        public IActionResult Guncelle(int olcekSonucID)
        {
            var olcekSonuc = _olcekSonucBusiness.OlcekSonucGetir(olcekSonucID);
            if (olcekSonuc == null) return NotFound();
            if (!BuKayitIcinIslemYapabilirMi(olcekSonuc.DoktorID))
            {
                TempData["HataMesaji"] = "Bu işlem için yetkiniz yok.";
                return RedirectToAction("Detay", "Hasta", new { tc = olcekSonuc.HastaTC, tab = "testOlcek" });
            }
            ViewBag.OlcekListesi = _olcekBusiness.OlcekleriListele();
            ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
            return View(olcekSonuc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(OlcekSonuc olcekSonuc)
        {
            var mevcutKayit = _olcekSonucBusiness.OlcekSonucGetir(olcekSonuc.OlcekID);
            if (mevcutKayit == null) return NotFound();
            if (!BuKayitIcinIslemYapabilirMi(mevcutKayit.DoktorID))
            {
                TempData["HataMesaji"] = "Bu işlem için yetkiniz yok.";
                return RedirectToAction("Detay", "Hasta", new { tc = olcekSonuc.HastaTC, tab = "testOlcek" });
            }
            _olcekSonucBusiness.OlcekSonucGuncelle(olcekSonuc);
            TempData["BasariMesaji"] = "Ölçek sonucu güncellendi.";
            return RedirectToAction("Detay", "Hasta", new { tc = olcekSonuc.HastaTC, tab = "testOlcek" });
        }
    }
}
