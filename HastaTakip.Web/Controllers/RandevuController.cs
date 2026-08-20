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
    public class RandevuController : Controller
    {
        private readonly RandevuBusiness _randevuBusiness;
        private readonly HastaBusiness _hastaBusiness;
        private readonly DoktorBusiness _doktorBusiness;
        private readonly RandevuSaatBusiness _randevuSaatBusiness;
        private readonly RandevuNotuBusiness _randevuNotuBusiness;
        private readonly KullaniciBusiness _kullaniciBusiness;

        public RandevuController(
            RandevuBusiness randevuBusiness,
            HastaBusiness hastaBusiness,
            DoktorBusiness doktorBusiness,
            RandevuSaatBusiness randevuSaatBusiness,
            RandevuNotuBusiness randevuNotuBusiness,
            KullaniciBusiness kullaniciBusiness)
        {
            _randevuBusiness = randevuBusiness;
            _hastaBusiness = hastaBusiness;
            _doktorBusiness = doktorBusiness;
            _randevuSaatBusiness = randevuSaatBusiness;
            _randevuNotuBusiness = randevuNotuBusiness;
            _kullaniciBusiness = kullaniciBusiness;
           
        }

        // GET: /Randevu
        public IActionResult Index(
            string? ara,
            string siralama = "TarihYeni",
            DateTime? baslangicTarihi = null,
            DateTime? bitisTarihi = null,
            short? doktorID = null,
            Guid? hastaGuid = null,
            string? hastaTC = null,
            string? durum = null,
            int sayfa = 1)
        {

            if (!hastaGuid.HasValue && !string.IsNullOrWhiteSpace(hastaTC))
            {
                var hastaGecici = _hastaBusiness.HastaGetir(hastaTC);
                if (hastaGecici != null)
                {
                    hastaTC = null; // eski parametreyi temizliyoruz
                    return RedirectToAction(nameof(Index), new { ara, siralama, baslangicTarihi, bitisTarihi, doktorID, hastaGuid = hastaGecici.HastaGuid, durum, sayfa });
                }
            }

            if (hastaGuid.HasValue)
            {
                var hasta = _hastaBusiness.HastaGetirById(hastaGuid.Value);
                hastaTC = hasta?.HastaTC;
            }
            const int sayfaBoyutu = 10;

            var (randevular, toplamKayit) = _randevuBusiness.RandevuListele(
                ara, siralama, baslangicTarihi, bitisTarihi, doktorID, hastaTC, durum, sayfa, sayfaBoyutu);

            int toplamSayfa = (int)Math.Ceiling(toplamKayit / (double)sayfaBoyutu);

            var hastalar = _hastaBusiness.HastaListele(sadeceAktif: false).ToDictionary(h => h.HastaTC);
            var doktorlar = _doktorBusiness.DoktorListele(sadeceAktif: false).ToDictionary(d => d.DoktorID);
            var saatler = _randevuSaatBusiness.SaatleriListele().ToDictionary(s => s.SaatID);

            ViewBag.Hastalar = hastalar;
            ViewBag.Doktorlar = doktorlar;
            ViewBag.Saatler = saatler;
            ViewBag.HastaGuid = hastaGuid;
            ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
            ViewBag.Ara = ara;
            ViewBag.Siralama = siralama;
            ViewBag.Baslangic = baslangicTarihi;
            ViewBag.Bitis = bitisTarihi;
            ViewBag.SeciliDoktorID = doktorID;
            ViewBag.SeciliHastaTC = hastaTC;
            ViewBag.SeciliDurum = durum;
            ViewBag.SayfaNo = sayfa;
            ViewBag.ToplamSayfa = toplamSayfa;

            return View(randevular);
        }
        public IActionResult Takvim(short? doktorID, DateTime? tarih, string? hastaTC, DateTime? secilenGun)
        {
            ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
            ViewBag.SeciliDoktorID = doktorID;
            ViewBag.HastaTC = hastaTC;

            var baslangicTarih = tarih ?? DateTime.Today;
            ViewBag.BaslangicTarih = baslangicTarih;

            if (doktorID.HasValue)
            {
                const int gunSayisi = 14;

                // Takvimi tam haftalar halinde (Pazartesi başlangıçlı) hizala
                int farkPazartesi = ((int)baslangicTarih.DayOfWeek + 6) % 7;
                var haftaBasi = baslangicTarih.AddDays(-farkPazartesi);
                var aralikBitis = baslangicTarih.AddDays(gunSayisi - 1);
                int toplamGun = (int)(aralikBitis - haftaBasi).TotalDays + 1;
                toplamGun = ((toplamGun + 6) / 7) * 7;

                ViewBag.HaftaBasi = haftaBasi;
                ViewBag.ToplamGun = toplamGun;
                ViewBag.AralikBaslangic = baslangicTarih;
                ViewBag.AralikBitis = aralikBitis;
                ViewBag.GunlukDurumlar = _randevuBusiness.DoktorTakvimAraligiGetir(doktorID.Value, haftaBasi, toplamGun);

                if (secilenGun.HasValue &&
                    secilenGun.Value.DayOfWeek != DayOfWeek.Saturday &&
                    secilenGun.Value.DayOfWeek != DayOfWeek.Sunday)
                {
                    ViewBag.SecilenGun = secilenGun.Value;
                    ViewBag.SecilenGunSlotlari =
                        _randevuBusiness.DoktorGunlukTakvimGetir(
                            doktorID.Value,
                            secilenGun.Value);
                }

                //if (secilenGun.HasValue)
                //{
                //    ViewBag.SecilenGun = secilenGun.Value;
                //    ViewBag.SecilenGunSlotlari = _randevuBusiness.DoktorGunlukTakvimGetir(doktorID.Value, secilenGun.Value);
                //}
            }

            return View();
        }
        // GET: /Randevu/Detay/5
        public IActionResult Detay(int randevuTarihID)
        {
            var randevu = _randevuBusiness.RandevuGetir(randevuTarihID);
            if (randevu == null)
            {
                return NotFound();
            }

            ViewBag.Hasta = _hastaBusiness.HastaGetir(randevu.HastaTC);
            ViewBag.Doktor = _doktorBusiness.DoktorGetir(randevu.DoktorID);
            ViewBag.Saat = _randevuSaatBusiness.SaatleriListele()
                .FirstOrDefault(s => s.SaatID == randevu.SaatID);

            var kullaniciAdi = User.Identity?.Name;
            if (!string.IsNullOrEmpty(kullaniciAdi))
            {
                var kullanici = _kullaniciBusiness.KullaniciGetir(kullaniciAdi);
                ViewBag.GirisYapanDoktorID = kullanici?.DoktorID;
            }
            else
            {
                ViewBag.GirisYapanDoktorID = null;
            }
            
            return View(randevu);
        }

        /// GET: /Randevu/Ekle?hastaTC=12345678901
        public IActionResult Ekle(string? hastaTC, short? doktorID, DateTime? tarih, byte? saatID, string? kaynak, short? randevuNotID)
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
            ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
            ViewBag.SaatListesi = _randevuSaatBusiness.SaatleriListele();
            ViewBag.OnSeciliDoktorID = doktorID;
            ViewBag.OnSeciliTarih = tarih;
            ViewBag.OnSeciliSaatID = saatID;
            ViewBag.Kaynak = kaynak;
            ViewBag.RandevuNotID = randevuNotID;

            return View();
        }

        // POST: /Randevu/Ekle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(string hastaTC, short doktorID, byte saatID, DateTime randevuTarih, string? kaynak, short? randevuNotID)
        {
            //System.Diagnostics.Debug.WriteLine($"KAYNAK: {kaynak} | RANDEVU_NOT_ID: {randevuNotID}");
            try
            {
                var yeniRandevuTarihID = _randevuBusiness.RandevuOlustur(hastaTC, doktorID, saatID, randevuTarih);

                if (kaynak == "randevuNotu")
                {
                    if (randevuNotID.HasValue)
                    {
                        _randevuNotuBusiness.SonrakiTarihGuncelle(randevuNotID.Value, randevuTarih);
                    }

                    TempData["BasariMesaji"] = "Sonraki randevu başarıyla oluşturuldu.";
                    return RedirectToAction("Detay", "Hasta", new { tc = hastaTC, tab = "randevuNotlari" });
                }

                TempData["BasariMesaji"] = "Randevu başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.HastaTC = hastaTC;
                ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
                ViewBag.SaatListesi = _randevuSaatBusiness.SaatleriListele();
                ViewBag.Kaynak = kaynak;
                ViewBag.RandevuNotID = randevuNotID;
                return View();
            }
        }

        // POST: /Randevu/Tamamlandi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Tamamlandi(int randevuTarihID)
        {
            try
            {
                _randevuBusiness.RandevuTamamlandiIsaretle(randevuTarihID);
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
        public IActionResult Geldi(int randevuTarihID, string? donus, string? hastaTC)
        {
            _randevuBusiness.GelisZamaniGuncelle(randevuTarihID);
            TempData["BasariMesaji"] = "Hasta geldi olarak işaretlendi.";
            return YonlendirGeriDon(donus, hastaTC);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MuayeneBaslat(int randevuTarihID, string? donus, string? hastaTC)
        {
            _randevuBusiness.MuayeneBaslangicGuncelle(randevuTarihID);
            TempData["BasariMesaji"] = "Muayene başlatıldı.";

            if (donus == "doktorPaneli")
            {
                return RedirectToAction("Index", "DoktorPaneli", new { hastaTC });
            }

            var randevu = _randevuBusiness.RandevuGetir(randevuTarihID);
            if (randevu != null)
            {
                return RedirectToAction("Detay", "Hasta", new { tc = randevu.HastaTC, tab = "randevuNotlari", aktifRandevuTarihID = randevuTarihID });
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Gelmedi(int randevuTarihID, string? donus, string? hastaTC)
        {
            try
            {
                _randevuBusiness.RandevuGelmediIsaretle(randevuTarihID);
                TempData["BasariMesaji"] = "Randevu 'Gelmedi' olarak işaretlendi.";
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.Message;
            }
            return YonlendirGeriDon(donus, hastaTC);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Iptal(int randevuTarihID, string? donus, string? hastaTC)
        {
            try
            {
                _randevuBusiness.RandevuIptalEt(randevuTarihID);
                TempData["BasariMesaji"] = "Randevu iptal edildi.";
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.Message;
            }
            return YonlendirGeriDon(donus, hastaTC);
        }

        private bool BuRandevuyaIslemYapabilirMi(short doktorID)
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
            return kullanici?.DoktorID == doktorID;
        }

        public IActionResult YenidenPlanla(int randevuTarihID)
        {
            var randevu = _randevuBusiness.RandevuGetir(randevuTarihID);
            if (randevu == null) return NotFound();

            if (randevu.RandevuDurum != "Planlandı")
            {
                TempData["HataMesaji"] = "Sadece 'Planlandı' durumundaki randevular yeniden planlanabilir.";
                return RedirectToAction(nameof(Detay), new { randevuTarihID });
            }

            if (!BuRandevuyaIslemYapabilirMi(randevu.DoktorID))
            {
                TempData["HataMesaji"] = "Bu randevuyu yeniden planlama yetkiniz yok.";
                return RedirectToAction(nameof(Detay), new { randevuTarihID });
            }

            ViewBag.Randevu = randevu;
            ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
            ViewBag.SaatListesi = _randevuSaatBusiness.SaatleriListele();
            ViewBag.Hasta = _hastaBusiness.HastaGetir(randevu.HastaTC);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult YenidenPlanla(int randevuTarihID, short yeniDoktorID, byte yeniSaatID, DateTime yeniTarih)
        {
            var randevu = _randevuBusiness.RandevuGetir(randevuTarihID);
            if (randevu == null) return NotFound();

            if (!BuRandevuyaIslemYapabilirMi(randevu.DoktorID))
            {
                TempData["HataMesaji"] = "Bu randevuyu yeniden planlama yetkiniz yok.";
                return RedirectToAction(nameof(Detay), new { randevuTarihID });
            }

            try
            {
                _randevuBusiness.RandevuYenidenPlanla(randevuTarihID, yeniDoktorID, yeniSaatID, yeniTarih);
                TempData["BasariMesaji"] = "Randevu başarıyla yeniden planlandı.";
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.Message;
            }

            return RedirectToAction(nameof(Detay), new { randevuTarihID });
        }

        private IActionResult YonlendirGeriDon(string? donus, string? hastaTC)
        {
            if (donus == "doktorPaneli")
            {
                return RedirectToAction("Index", "DoktorPaneli", new { hastaTC });
            }
            return RedirectToAction(nameof(Index));
        }
        private void YukleDropdownlar(string? seciliHastaTC)
        {
            ViewBag.HastaListesi = _hastaBusiness.HastaListele();
            ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
            ViewBag.SaatListesi = _randevuSaatBusiness.SaatleriListele();
            ViewBag.SeciliHastaTC = seciliHastaTC;
        }
    }
}