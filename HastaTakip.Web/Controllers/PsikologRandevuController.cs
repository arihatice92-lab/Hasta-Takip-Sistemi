using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HastaTakip.Web.Controllers
{
    [Authorize]
    public class PsikologRandevuController : Controller
    {
        private readonly PsikologRandevuBusiness _psikologRandevuBusiness;
        private readonly HastaBusiness _hastaBusiness;
        private readonly PsikologBusiness _psikologBusiness;
        private readonly PsikologRandevuSaatBusiness _psikologRandevuSaatBusiness;
        private readonly KullaniciBusiness _kullaniciBusiness;

        public PsikologRandevuController(
            PsikologRandevuBusiness psikologRandevuBusiness,
            HastaBusiness hastaBusiness,
            PsikologBusiness psikologBusiness,
            PsikologRandevuSaatBusiness psikologRandevuSaatBusiness,
            KullaniciBusiness kullaniciBusiness)
        {
            _psikologRandevuBusiness = psikologRandevuBusiness;
            _hastaBusiness = hastaBusiness;
            _psikologBusiness = psikologBusiness;
            _psikologRandevuSaatBusiness = psikologRandevuSaatBusiness;
            _kullaniciBusiness = kullaniciBusiness;
        }

        // GET: /PsikologRandevu
        public IActionResult Index(
            string? ara,
            string siralama = "TarihYeni",
            DateTime? baslangicTarihi = null,
            DateTime? bitisTarihi = null,
            byte? psikologID = null,
            string? hastaTC = null,
            string? durum = null,
            int sayfa = 1)
        {
            const int sayfaBoyutu = 15;

            var (randevular, toplamKayit) = _psikologRandevuBusiness.RandevuListele(
                ara, siralama, baslangicTarihi, bitisTarihi, psikologID, hastaTC, durum, sayfa, sayfaBoyutu);

            int toplamSayfa = (int)Math.Ceiling(toplamKayit / (double)sayfaBoyutu);

            var hastalar = _hastaBusiness.HastaListele(sadeceAktif: false).ToDictionary(h => h.HastaTC);
            var psikologlar = _psikologBusiness.PsikologAra(null, "AZ", "hepsi").ToDictionary(p => p.PsikologID);
            var saatler = _psikologRandevuSaatBusiness.SaatleriListele().ToDictionary(s => s.SaatID);

            ViewBag.Hastalar = hastalar;
            ViewBag.Psikologlar = psikologlar;
            ViewBag.Saatler = saatler;
            ViewBag.PsikologListesi = _psikologBusiness.PsikologAra(null, "AZ", "aktif");
            ViewBag.Ara = ara;
            ViewBag.Siralama = siralama;
            ViewBag.Baslangic = baslangicTarihi;
            ViewBag.Bitis = bitisTarihi;
            ViewBag.SeciliPsikologID = psikologID;
            ViewBag.SeciliHastaTC = hastaTC;
            ViewBag.SeciliDurum = durum;
            ViewBag.SayfaNo = sayfa;
            ViewBag.ToplamSayfa = toplamSayfa;

            return View(randevular);
        }

        // GET: /PsikologRandevu/Takvim
        public IActionResult Takvim(byte? psikologID, DateTime? tarih, string? hastaTC, DateTime? secilenGun, int? randevuTarihID)
        {
            ViewBag.PsikologListesi = _psikologBusiness.PsikologAra(null, "AZ", "aktif");
            ViewBag.SeciliPsikologID = psikologID;
            ViewBag.HastaTC = hastaTC;
            ViewBag.RandevuTarihID = randevuTarihID;

            var baslangicTarih = tarih ?? DateTime.Today;
            ViewBag.BaslangicTarih = baslangicTarih;

            if (psikologID.HasValue)
            {
                const int gunSayisi = 28;

                int farkPazartesi = ((int)baslangicTarih.DayOfWeek + 6) % 7;
                var haftaBasi = baslangicTarih.AddDays(-farkPazartesi);
                var aralikBitis = baslangicTarih.AddDays(gunSayisi - 1);
                int toplamGun = (int)(aralikBitis - haftaBasi).TotalDays + 1;
                toplamGun = ((toplamGun + 6) / 7) * 7;

                ViewBag.HaftaBasi = haftaBasi;
                ViewBag.ToplamGun = toplamGun;
                ViewBag.AralikBaslangic = baslangicTarih;
                ViewBag.AralikBitis = aralikBitis;
                ViewBag.GunlukDurumlar = _psikologRandevuBusiness.TakvimAraligiGetir(psikologID.Value, haftaBasi, toplamGun);

                if (secilenGun.HasValue &&
                    secilenGun.Value.DayOfWeek != DayOfWeek.Saturday &&
                    secilenGun.Value.DayOfWeek != DayOfWeek.Sunday)
                {
                    ViewBag.SecilenGun = secilenGun.Value;
                    ViewBag.SecilenGunSlotlari =
                        _psikologRandevuBusiness.GunlukTakvimGetir(
                            psikologID.Value,
                            secilenGun.Value);
                }
            }

            return View();
        }

        // GET: /PsikologRandevu/Detay/5
        public IActionResult Detay(int randevuTarihID)
        {
            var randevu = _psikologRandevuBusiness.RandevuGetir(randevuTarihID);
            if (randevu == null)
            {
                return NotFound();
            }

            ViewBag.Hasta = _hastaBusiness.HastaGetir(randevu.HastaTC);
            ViewBag.Psikolog = _psikologBusiness.PsikologGetir(randevu.PsikologID);
            ViewBag.Saat = _psikologRandevuSaatBusiness.SaatleriListele().FirstOrDefault(s => s.SaatID == randevu.SaatID);
            var kullaniciAdi = User.Identity?.Name;
            if (!string.IsNullOrEmpty(kullaniciAdi))
            {
                var kullanici = _kullaniciBusiness.KullaniciGetir(kullaniciAdi);
                ViewBag.GirisYapanPsikologID = kullanici?.PsikologID;
            }
            else
            {
                ViewBag.GirisYapanPsikologID = null;
            }
            return View(randevu);
        }

        // GET: /PsikologRandevu/Ekle?hastaTC=...
        public IActionResult Ekle(string? hastaTC, byte? psikologID, DateTime? tarih, byte? saatID)
        {
            Hasta? seciliHasta = null;

            if (!string.IsNullOrWhiteSpace(hastaTC))
            {
                seciliHasta = _hastaBusiness.HastaGetir(hastaTC);
                if (seciliHasta == null)
                {
                    ViewBag.HataMesaji = "Bu TC kimlik numarasına ait hasta bulunamadı.";
                }
            }

            ViewBag.SeciliHasta = seciliHasta;
            ViewBag.PsikologListesi = _psikologBusiness.PsikologAra(null, "AZ", "aktif");
            ViewBag.SaatListesi = _psikologRandevuSaatBusiness.SaatleriListele();
            ViewBag.OnSeciliPsikologID = psikologID;
            ViewBag.OnSeciliTarih = tarih;
            ViewBag.OnSeciliSaatID = saatID;

            return View();
        }

        // POST: /PsikologRandevu/Ekle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(string hastaTC, byte psikologID, byte saatID, DateTime randevuTarih)
        {
            try
            {
                _psikologRandevuBusiness.RandevuOlustur(hastaTC, psikologID, saatID, randevuTarih);
                TempData["BasariMesaji"] = "Randevu başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.HastaTC = hastaTC;
                ViewBag.PsikologListesi = _psikologBusiness.PsikologAra(null, "AZ", "aktif");
                ViewBag.SaatListesi = _psikologRandevuSaatBusiness.SaatleriListele();
                return View();
            }
        }

        private bool BuRandevuyaIslemYapabilirMi(byte psikologID)
        {
            if (User.IsInRole("Yönetici") || User.IsInRole("Sekreter"))
            {
                return true;
            }

            var kullaniciAdi = User.Identity?.Name;
            if (string.IsNullOrEmpty(kullaniciAdi))
            {
                return false;
            }

            var kullanici = _kullaniciBusiness.KullaniciGetir(kullaniciAdi);
            return kullanici?.PsikologID == psikologID;
        }

        public IActionResult YenidenPlanla(int randevuTarihID, byte? yeniPsikologID = null, DateTime? yeniTarih = null, byte? yeniSaatID = null)
        {
            var randevu = _psikologRandevuBusiness.RandevuGetir(randevuTarihID);
            if (randevu == null) return NotFound();

            if (randevu.RandevuDurum != "Planlandı")
            {
                TempData["HataMesaji"] = "Sadece 'Planlandı' durumundaki randevular yeniden planlanabilir.";
                return RedirectToAction(nameof(Detay), new { randevuTarihID });
            }

            if (!BuRandevuyaIslemYapabilirMi(randevu.PsikologID))
            {
                TempData["HataMesaji"] = "Bu randevuyu yeniden planlama yetkiniz yok.";
                return RedirectToAction(nameof(Detay), new { randevuTarihID });
            }

            ViewBag.Randevu = randevu;
            ViewBag.PsikologListesi = _psikologBusiness.PsikologAra(null, "AZ", "aktif");
            ViewBag.SaatListesi = _psikologRandevuSaatBusiness.SaatleriListele();
            ViewBag.Hasta = _hastaBusiness.HastaGetir(randevu.HastaTC);

            ViewBag.OnSeciliPsikologID = yeniPsikologID ?? randevu.PsikologID;
            ViewBag.OnSeciliTarih = yeniTarih ?? randevu.RandevuTarih;
            ViewBag.OnSeciliSaatID = yeniSaatID ?? randevu.SaatID;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult YenidenPlanla(int randevuTarihID, byte yeniPsikologID, byte yeniSaatID, DateTime yeniTarih)
        {
            var randevu = _psikologRandevuBusiness.RandevuGetir(randevuTarihID);
            if (randevu == null) return NotFound();

            if (!BuRandevuyaIslemYapabilirMi(randevu.PsikologID))
            {
                TempData["HataMesaji"] = "Bu randevuyu yeniden planlama yetkiniz yok.";
                return RedirectToAction(nameof(Detay), new { randevuTarihID });
            }

            try
            {
                _psikologRandevuBusiness.RandevuYenidenPlanla(randevuTarihID, yeniPsikologID, yeniSaatID, yeniTarih);
                TempData["BasariMesaji"] = "Randevu başarıyla yeniden planlandı.";
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.Message;
            }

            return RedirectToAction(nameof(Detay), new { randevuTarihID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Iptal(int randevuTarihID)
        {
            try
            {
                _psikologRandevuBusiness.RandevuIptalEt(randevuTarihID);
                TempData["BasariMesaji"] = "Randevu iptal edildi.";
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Gelmedi(int randevuTarihID)
        {
            try
            {
                _psikologRandevuBusiness.RandevuGelmediIsaretle(randevuTarihID);
                TempData["BasariMesaji"] = "Randevu 'Gelmedi' olarak işaretlendi.";
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Tamamlandi(int randevuTarihID)
        {
            try
            {
                _psikologRandevuBusiness.RandevuTamamlandiIsaretle(randevuTarihID);
                TempData["BasariMesaji"] = "Randevu tamamlandı olarak işaretlendi.";
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Geldi(int randevuTarihID)
        {
            _psikologRandevuBusiness.GelisZamaniGuncelle(randevuTarihID);
            TempData["BasariMesaji"] = "Hasta geldi olarak işaretlendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TestBaslat(int randevuTarihID)
        {
            _psikologRandevuBusiness.TestBaslangicGuncelle(randevuTarihID);
            TempData["BasariMesaji"] = "Test başlatıldı.";
            return RedirectToAction(nameof(Index));
        }


    }
}