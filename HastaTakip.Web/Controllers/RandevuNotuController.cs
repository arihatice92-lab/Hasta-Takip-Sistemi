using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HastaTakip.Web.Controllers
{
    [Authorize]
    public class RandevuNotuController : Controller
    {
        private readonly RandevuNotuBusiness _randevuNotuBusiness;
        private readonly RandevuBusiness _randevuBusiness;
        private readonly HastaBusiness _hastaBusiness;
        private readonly DoktorBusiness _doktorBusiness;
        private readonly RandevuSaatBusiness _randevuSaatBusiness;

        public RandevuNotuController(
            RandevuNotuBusiness randevuNotuBusiness,
            RandevuBusiness randevuBusiness,
            HastaBusiness hastaBusiness,
            DoktorBusiness doktorBusiness,
            RandevuSaatBusiness randevuSaatBusiness)
        {
            _randevuNotuBusiness = randevuNotuBusiness;
            _randevuBusiness = randevuBusiness;
            _hastaBusiness = hastaBusiness;
            _doktorBusiness = doktorBusiness;
            _randevuSaatBusiness = randevuSaatBusiness;
        }

        public IActionResult Ekle(int randevuTarihID)
        {
            var randevu = _randevuBusiness.RandevuGetir(randevuTarihID);
            if (randevu == null)
            {
                return NotFound();
            }

            var hasta = _hastaBusiness.HastaGetir(randevu.HastaTC);
            var doktor = _doktorBusiness.DoktorGetir(randevu.DoktorID);
            var saat = _randevuSaatBusiness.SaatleriListele().FirstOrDefault(s => s.SaatID == randevu.SaatID);

            ViewBag.RandevuTarihID = randevuTarihID;
            ViewBag.HastaTC = randevu.HastaTC;
            ViewBag.DoktorID = randevu.DoktorID;
            ViewBag.HastaAdi = hasta != null ? $"{hasta.HastaAd} {hasta.HastaSoyad}" : "-";
            ViewBag.DoktorAdi = doktor != null ? $"{doktor.DoktorAd} {doktor.DoktorSoyad}" : "-";
            ViewBag.RandevuTarih = randevu.RandevuTarih;
            ViewBag.SaatMetin = saat != null ? saat.RandevuBaslangicSaat.ToString(@"hh\:mm") : "-";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(RandevuNotu notu)
        {
            _randevuNotuBusiness.RandevuNotuEkle(notu);
            TempData["BasariMesaji"] = "Randevu notu kaydedildi.";
            if (notu.SonrakiRandevuTarihi.HasValue)
            {
                return RedirectToAction("Ekle", "Randevu", new
                {
                    hastaTC = notu.HastaTC,
                    doktorID = notu.DoktorID,
                    tarih = notu.SonrakiRandevuTarihi.Value.ToString("yyyy-MM-dd"),
                    kaynak = "randevuNotu"
                });
            }
            return RedirectToAction("Detay", "Randevu", new { randevuTarihID = notu.RandevuTarihID });
        }

        public IActionResult Guncelle(short randevuNotID)
        {
            var notu = _randevuNotuBusiness.RandevuNotuGetir(randevuNotID);
            if (notu == null)
            {
                return NotFound();
            }
            return View(notu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(RandevuNotu notu)
        {
            var mevcutNot = _randevuNotuBusiness.RandevuNotuGetir(notu.RandevuNotID);
            var eskiSonrakiTarih = mevcutNot?.SonrakiRandevuTarihi;

            _randevuNotuBusiness.RandevuNotuGuncelle(notu);
            TempData["BasariMesaji"] = "Randevu notu güncellendi.";

            bool yeniTarihGirildi = notu.SonrakiRandevuTarihi.HasValue
                && notu.SonrakiRandevuTarihi != eskiSonrakiTarih;

            if (yeniTarihGirildi)
            {
                return RedirectToAction("Ekle", "Randevu", new
                {
                    hastaTC = notu.HastaTC,
                    doktorID = notu.DoktorID,
                    tarih = notu.SonrakiRandevuTarihi!.Value.ToString("yyyy-MM-dd"),
                    kaynak = "randevuNotu"
                });
            }

            return RedirectToAction("Detay", "Randevu", new { randevuTarihID = notu.RandevuTarihID });
        }
    }
}