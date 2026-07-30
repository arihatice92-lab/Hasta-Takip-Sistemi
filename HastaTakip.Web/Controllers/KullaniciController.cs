using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    [Authorize(Roles = "Yönetici")]
    public class KullaniciController : Controller
    {
        private readonly KullaniciBusiness _kullaniciBusiness;

        public KullaniciController(KullaniciBusiness kullaniciBusiness)
        {
            _kullaniciBusiness = kullaniciBusiness;
        }

        // GET: /Kullanici
        public IActionResult Index()
        {
            var kullanicilar = _kullaniciBusiness.KullaniciListele();
            return View(kullanicilar);
        }

        // GET: /Kullanici/Ekle
        public IActionResult Ekle()
        {
            return View();
        }

        // POST: /Kullanici/Ekle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(Kullanici kullanici, string sifre)
        {
            if (!ModelState.IsValid)
            {
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