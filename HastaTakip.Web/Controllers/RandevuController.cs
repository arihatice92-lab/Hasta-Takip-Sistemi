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

        public RandevuController(
            RandevuBusiness randevuBusiness,
            HastaBusiness hastaBusiness,
            DoktorBusiness doktorBusiness,
            RandevuSaatBusiness randevuSaatBusiness)
        {
            _randevuBusiness = randevuBusiness;
            _hastaBusiness = hastaBusiness;
            _doktorBusiness = doktorBusiness;
            _randevuSaatBusiness = randevuSaatBusiness;
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
        public IActionResult Ekle(string? hastaTC)
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

            return View();
        }

        // POST: /Randevu/Ekle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(string hastaTC, short doktorID, byte saatID, DateTime randevuTarih)
        {
            try
            {
                _randevuBusiness.RandevuOlustur(hastaTC, doktorID, saatID, randevuTarih);
                TempData["BasariMesaji"] = "Randevu başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.SeciliHasta = _hastaBusiness.HastaGetir(hastaTC);
                ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
                ViewBag.SaatListesi = _randevuSaatBusiness.SaatleriListele();
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

        private void YukleDropdownlar(string? seciliHastaTC)
        {
            ViewBag.HastaListesi = _hastaBusiness.HastaListele();
            ViewBag.DoktorListesi = _doktorBusiness.DoktorListele();
            ViewBag.SaatListesi = _randevuSaatBusiness.SaatleriListele();
            ViewBag.SeciliHastaTC = seciliHastaTC;
        }
    }
}