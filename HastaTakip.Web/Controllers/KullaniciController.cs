using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HastaTakip.Web.Controllers
{
    [Authorize(Roles = "Yönetici")]
    public class KullaniciController : Controller
    {
        private readonly KullaniciBusiness _kullaniciBusiness;
        private readonly DoktorBusiness _doktorBusiness;
        private readonly PsikologBusiness _psikologBusiness;

        public KullaniciController(KullaniciBusiness kullaniciBusiness, DoktorBusiness doktorBusiness, PsikologBusiness psikologBusiness)
        {
            _kullaniciBusiness = kullaniciBusiness;
            _doktorBusiness = doktorBusiness;
            _psikologBusiness = psikologBusiness;
        }

        // GET: /Kullanici
        public IActionResult Index()
        {
            var kullanicilar = _kullaniciBusiness.KullaniciListele();
            var rolSozlugu = new Dictionary<byte, string>
            {
                { 1, "Yönetici" },
                { 2, "Doktor" },
                { 3, "Sekreter" },
                { 4, "Psikolog" }
            };
            ViewBag.RolSozlugu = rolSozlugu;
            return View(kullanicilar);
        }

        // GET: /Kullanici/Ekle
        public IActionResult Ekle()
        {
            ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
            ViewBag.PsikologListesi = _psikologBusiness.PsikologAra(null, "AZ", "aktif");
            return View();
        }

        // POST: /Kullanici/Ekle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(Kullanici kullanici, string sifre)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
                ViewBag.PsikologListesi = _psikologBusiness.PsikologAra(null, "AZ", "aktif");
                return View(kullanici);
            }

            try
            {
                _kullaniciBusiness.KullaniciEkle(kullanici, sifre);
                TempData["BasariMesaji"] = $"{kullanici.KullaniciAdi} kullanıcısı başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
                ViewBag.PsikologListesi = _psikologBusiness.PsikologAra(null, "AZ", "aktif");
                return View(kullanici);
            }
        }

        // POST: /Kullanici/SifreSifirla
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SifreSifirla(int kullaniciID, string yeniSifre)
        {
            try
            {
                _kullaniciBusiness.SifreSifirla(kullaniciID, yeniSifre);
                TempData["BasariMesaji"] = "Şifre başarıyla sıfırlandı.";
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Guncelle(int kullaniciID)
        {
            var kullanicilar = _kullaniciBusiness.KullaniciListele();
            var kullanici = kullanicilar.FirstOrDefault(k => k.KullaniciID == kullaniciID);
            if (kullanici == null)
            {
                return NotFound();
            }

            ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
            ViewBag.PsikologListesi = _psikologBusiness.PsikologAra(null, "AZ", "aktif");
            return View(kullanici);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(Kullanici kullanici)
        {
            _kullaniciBusiness.KullaniciGuncelle(kullanici);
            TempData["BasariMesaji"] = "Kullanıcı bilgileri güncellendi.";
            return RedirectToAction(nameof(Index));
        }
        // POST: /Kullanici/PasifeAl
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PasifeAl(int kullaniciID)
        {
            _kullaniciBusiness.KullaniciPasifeAl(kullaniciID);
            TempData["BasariMesaji"] = "Kullanıcı pasife alındı.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Kullanici/AktifEt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AktifEt(int kullaniciID)
        {
            _kullaniciBusiness.KullaniciAktifEt(kullaniciID);
            TempData["BasariMesaji"] = "Kullanıcı aktife alındı.";
            return RedirectToAction(nameof(Index));
        }
    }
}