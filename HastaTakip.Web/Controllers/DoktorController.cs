using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace HastaTakip.Web.Controllers
{
    [Authorize]
    public class DoktorController : Controller
    {
        private readonly DoktorBusiness _doktorBusiness;
        private readonly DoktorIzniBusiness _doktorIzniBusiness;
        private readonly KullaniciBusiness _kullaniciBusiness;
        public DoktorController(DoktorBusiness doktorBusiness, DoktorIzniBusiness doktorIzniBusiness, KullaniciBusiness kullaniciBusiness)
        {
            _doktorBusiness = doktorBusiness;
            _doktorIzniBusiness = doktorIzniBusiness;
            _kullaniciBusiness = kullaniciBusiness;
        }

        // GET: /Doktor

        [Authorize(Roles = "Yönetici, Sekreter")]
        public IActionResult Index(
            string? ara,
            string siralama = "AZ",
            string aktif = "aktif",
            string? brans = null,
            int sayfa = 1)
        {
            bool? aktifFiltre = aktif switch
            {
                "aktif" => true,
                "pasif" => false,
                "hepsi" => (bool?)null,
                _ => true
            };

            const int sayfaBoyutu = 10;
            var tumDoktorlar = _doktorBusiness.DoktorAra(
                ara,
                siralama,
                aktifFiltre,
                brans);

            int toplamKayit = tumDoktorlar.Count;
            int toplamSayfa = (int)Math.Ceiling(toplamKayit / (double)sayfaBoyutu);

            if (sayfa < 1) sayfa = 1;
            if (toplamSayfa > 0 && sayfa > toplamSayfa) sayfa = toplamSayfa;

            var sayfalanmisDoktorlar = tumDoktorlar
                .Skip((sayfa - 1) * sayfaBoyutu)
                .Take(sayfaBoyutu)
                .ToList();
            ViewBag.Ara = ara;
            ViewBag.Siralama = siralama;
            ViewBag.Aktif = aktif;
            ViewBag.Brans = brans;
            ViewBag.SayfaNo= sayfa;
            ViewBag.ToplamSayfa= toplamSayfa;

            return View(sayfalanmisDoktorlar);
        }

        // GET: /Doktor/Detay/5

        [Authorize(Roles = "Yönetici,Sekreter,Doktor")]
        public IActionResult Detay(short doktorID)
        {

            if (User.IsInRole("Doktor") && !User.IsInRole("Yönetici") && !User.IsInRole("Sekreter"))
            {
                var kullaniciAdi = User.Identity?.Name;
                var kullanici = !string.IsNullOrEmpty(kullaniciAdi) ? _kullaniciBusiness.KullaniciGetir(kullaniciAdi) : null;

                if (kullanici?.DoktorID != doktorID)
                {
                    return Forbid();
                }
            }

            var doktor = _doktorBusiness.DoktorGetir(doktorID);
            if (doktor == null) return NotFound();

            ViewBag.Izinler = _doktorIzniBusiness.IzinleriListele(doktorID);
            return View(doktor);
        }

        // GET: /Doktor/Ekle
        [Authorize(Roles = "Yönetici, Sekreter")]
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
        [Authorize(Roles = "Yönetici, Sekreter")]
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
        [Authorize(Roles = "Yönetici, Sekreter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ayrilis(short doktorID)
        {
            _doktorBusiness.DoktorAyrilis(doktorID);
            TempData["BasariMesaji"] = "Doktorun ayrılış tarihi işaretlendi.";
            return RedirectToAction(nameof(Index));
        }
        //Doktor izin  ekle
        [Authorize(Roles = "Yönetici,Sekreter,Doktor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IzinEkle(DoktorIzni izin)
        {
            if (User.IsInRole("Doktor") && !User.IsInRole("Yönetici") && !User.IsInRole("Sekreter"))
            {
                var kullaniciAdi = User.Identity?.Name;
                var kullanici = !string.IsNullOrEmpty(kullaniciAdi) ? _kullaniciBusiness.KullaniciGetir(kullaniciAdi) : null;

                if (kullanici?.DoktorID != izin.DoktorID)
                {
                    TempData["HataMesaji"] = "Sadece kendi izin kaydınızı ekleyebilirsiniz.";
                    return RedirectToAction(nameof(Detay), new { doktorID = izin.DoktorID });
                }
            }
            var cakismalar = _doktorIzniBusiness.RandevuCakismalariGetir(izin.DoktorID, izin.BaslangicTarihi, izin.BitisTarihi);

            if (cakismalar.Count > 0)
            {
                var doktor = _doktorBusiness.DoktorGetir(izin.DoktorID);
                ViewBag.Izinler = _doktorIzniBusiness.IzinleriListele(izin.DoktorID);
                ViewBag.Cakismalar = cakismalar;
                TempData["HataMesaji"] = $"Bu tarih aralığında {cakismalar.Count} randevu çakışması bulunuyor. Önce bunları çözmelisiniz.";
                return View("Detay", doktor);
            }

            var kullaniciID = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            izin.EkleyenKullaniciID = kullaniciID;

            try
            {
                _doktorIzniBusiness.IzinEkle(izin);
                TempData["BasariMesaji"] = "İzin kaydı eklendi.";
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.Message;
            }
            return RedirectToAction(nameof(Detay), new { doktorID = izin.DoktorID });
        }
        //Doktor izin sil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IzinSil(int izinID, short doktorID)
        {
            _doktorIzniBusiness.IzinSil(izinID);
            TempData["BasariMesaji"] = "İzin kaydı silindi.";
            return RedirectToAction(nameof(Detay), new { doktorID });
        }
    }

}
