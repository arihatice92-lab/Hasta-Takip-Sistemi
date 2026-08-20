using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    [Authorize]
    public class PsikologController : Controller
    {
        private readonly PsikologBusiness _psikologBusiness;
        private readonly PsikologIzniBusiness _psikologIzniBusiness;

        private readonly KullaniciBusiness _kullaniciBusiness;

        public PsikologController(PsikologBusiness psikologBusiness, PsikologIzniBusiness psikologIzniBusiness, KullaniciBusiness kullaniciBusiness)
        {
            _psikologBusiness = psikologBusiness;
            _psikologIzniBusiness = psikologIzniBusiness;
            _kullaniciBusiness = kullaniciBusiness;
        }

        public IActionResult Index(string? ara, string siralama = "AZ", string aktif = "aktif")
        {
            var psikologlar = _psikologBusiness.PsikologAra(ara, siralama, aktif);

            ViewBag.Ara = ara;
            ViewBag.Siralama = siralama;
            ViewBag.Aktif = aktif;

            return View(psikologlar);
        }

        public IActionResult Detay(byte psikologID)
        {
            var psikolog = _psikologBusiness.PsikologGetir(psikologID);
            if (psikolog == null) return NotFound();

            ViewBag.Izinler = _psikologIzniBusiness.IzinleriListele(psikologID);
            return View(psikolog);
        }

        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(Psikolog psikolog)
        {
            if (!ModelState.IsValid)
            {
                return View(psikolog);
            }

            try
            {
                _psikologBusiness.PsikologEkle(psikolog);
                TempData["BasariMesaji"] = $"{psikolog.PsikologAd} {psikolog.PsikologSoyad} başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(psikolog);
            }
        }

        public IActionResult Guncelle(byte psikologID)
        {
            var psikolog = _psikologBusiness.PsikologGetir(psikologID);
            if (psikolog == null) return NotFound();
            return View(psikolog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(Psikolog psikolog)
        {
            if (!ModelState.IsValid)
            {
                return View(psikolog);
            }

            try
            {
                _psikologBusiness.PsikologGuncelle(psikolog);
                TempData["BasariMesaji"] = "Psikolog bilgileri güncellendi.";
                return RedirectToAction(nameof(Detay), new { psikologID = psikolog.PsikologID });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(psikolog);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ayrilis(byte psikologID)
        {
            _psikologBusiness.PsikologAyrilis(psikologID);
            TempData["BasariMesaji"] = "Psikoloğun ayrılış tarihi işaretlendi.";
            return RedirectToAction(nameof(Detay), new { psikologID });
        }
        [Authorize(Roles = "Yönetici,Sekreter,Psikolog")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IzinEkle(PsikologIzni izin)
        {
            var cakismalar = _psikologIzniBusiness.RandevuCakismalariGetir(izin.PsikologID, izin.BaslangicTarihi, izin.BitisTarihi);

            if (cakismalar.Count > 0)
            {
                var psikolog = _psikologBusiness.PsikologGetir(izin.PsikologID);
                ViewBag.Izinler = _psikologIzniBusiness.IzinleriListele(izin.PsikologID);
                ViewBag.Cakismalar = cakismalar;
                ViewBag.HataMesaji = $"Bu tarih aralığında {cakismalar.Count} randevu çakışması bulunuyor. Önce bunları çözmelisiniz.";
                return View("Detay", psikolog);
            }

            if (User.IsInRole("Psikolog") && !User.IsInRole("Yönetici") && !User.IsInRole("Sekreter"))
            {
                var kullaniciAdi = User.Identity?.Name;
                var kullanici = !string.IsNullOrEmpty(kullaniciAdi) ? _kullaniciBusiness.KullaniciGetir(kullaniciAdi) : null;

                if (kullanici?.PsikologID != izin.PsikologID)
                {
                    TempData["HataMesaji"] = "Sadece kendi izin kaydınızı ekleyebilirsiniz.";
                    return RedirectToAction(nameof(Detay), new { psikologID = izin.PsikologID });
                }
            }

            var girisYapanKullaniciID = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            izin.EkleyenKullaniciID = girisYapanKullaniciID;

            try
            {
                _psikologIzniBusiness.IzinEkle(izin);
                TempData["BasariMesaji"] = "İzin kaydı eklendi.";
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.Message;
            }
            return RedirectToAction(nameof(Detay), new { psikologID = izin.PsikologID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IzinSil(int izinID, byte psikologID)
        {
            _psikologIzniBusiness.IzinSil(izinID);
            TempData["BasariMesaji"] = "İzin kaydı silindi.";
            return RedirectToAction(nameof(Detay), new { psikologID });
        }
    }
}