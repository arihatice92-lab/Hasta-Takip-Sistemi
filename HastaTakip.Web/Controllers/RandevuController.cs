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

        public RandevuController(
            RandevuBusiness randevuBusiness,
            HastaBusiness hastaBusiness,
            DoktorBusiness doktorBusiness,
            RandevuSaatBusiness randevuSaatBusiness,
            RandevuNotuBusiness randevuNotuBusiness)
        {
            _randevuBusiness = randevuBusiness;
            _hastaBusiness = hastaBusiness;
            _doktorBusiness = doktorBusiness;
            _randevuSaatBusiness = randevuSaatBusiness;
            _randevuNotuBusiness = randevuNotuBusiness;
        }

        // GET: /Randevu
        public IActionResult Index(
            string? ara,
            string siralama = "TarihYeni",
            DateTime? baslangicTarihi = null,
            DateTime? bitisTarihi = null,
            short? doktorID = null,
            string? hastaTC = null,
            string? durum = null,
            int sayfa = 1)
        {
            const int sayfaBoyutu = 15;

            var (randevular, toplamKayit) = _randevuBusiness.RandevuListele(
                ara, siralama, baslangicTarihi, bitisTarihi, doktorID, hastaTC, durum, sayfa, sayfaBoyutu);

            int toplamSayfa = (int)Math.Ceiling(toplamKayit / (double)sayfaBoyutu);

            var hastalar = _hastaBusiness.HastaListele(sadeceAktif: false).ToDictionary(h => h.HastaTC);
            var doktorlar = _doktorBusiness.DoktorListele(sadeceAktif: false).ToDictionary(d => d.DoktorID);
            var saatler = _randevuSaatBusiness.SaatleriListele().ToDictionary(s => s.SaatID);

            ViewBag.Hastalar = hastalar;
            ViewBag.Doktorlar = doktorlar;
            ViewBag.Saatler = saatler;

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
        public IActionResult Takvim(short? doktorID, DateTime? tarih, string? hastaTC)
        {
            ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
            ViewBag.SeciliDoktorID = doktorID;
            ViewBag.HastaTC = hastaTC;

            var seciliTarih = tarih ?? DateTime.Today;
            ViewBag.SeciliTarih = seciliTarih;

            if (doktorID.HasValue)
            {
                var slotlar = _randevuBusiness.DoktorGunlukTakvimGetir(doktorID.Value, seciliTarih);
                return View(slotlar);
            }

            return View(new List<HastaTakip.Entities.DoktorTakvimSlotu>());
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

        // POST: /Randevu/Iptal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Iptal(int randevuTarihID)
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
            return RedirectToAction(nameof(Index));
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

        // POST: /Randevu/Gelmedi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Gelmedi(int randevuTarihID)
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
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Geldi(int randevuTarihID)
        {
            _randevuBusiness.GelisZamaniGuncelle(randevuTarihID);
            TempData["BasariMesaji"] = "Hasta geldi olarak işaretlendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MuayeneBaslat(int randevuTarihID)
        {
            _randevuBusiness.MuayeneBaslangicGuncelle(randevuTarihID);
            TempData["BasariMesaji"] = "Muayene başlatıldı.";
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