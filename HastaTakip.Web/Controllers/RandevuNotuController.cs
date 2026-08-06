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
        public IActionResult Ekle(RandevuNotu notu, string? randevuDurumu)
        {
            var kullaniciGirdiTarih = notu.SonrakiRandevuTarihi;
            notu.SonrakiRandevuTarihi = null;

            try
            {
                var yeniRandevuNotID = _randevuNotuBusiness.RandevuNotuEkle(notu);

                UygulaRandevuDurumu(notu.RandevuTarihID, randevuDurumu);

                TempData["BasariMesaji"] = "Randevu notu kaydedildi.";

                if (kullaniciGirdiTarih.HasValue)
                {
                    return RedirectToAction("Ekle", "Randevu", new
                    {
                        hastaTC = notu.HastaTC,
                        doktorID = notu.DoktorID,
                        tarih = kullaniciGirdiTarih.Value.ToString("yyyy-MM-dd"),
                        kaynak = "randevuNotu",
                        randevuNotID = yeniRandevuNotID
                    });
                }

                return RedirectToAction("Detay", "Randevu", new { randevuTarihID = notu.RandevuTarihID });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                notu.SonrakiRandevuTarihi = kullaniciGirdiTarih; // kullanıcının girdiği veriyi geri koy
                DoldurBaglamBilgisi(notu.RandevuTarihID);
                return View(notu);
            }
        }

        private void DoldurBaglamBilgisi(int randevuTarihID)
        {
            var randevu = _randevuBusiness.RandevuGetir(randevuTarihID);
            if (randevu == null) return;

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
        public IActionResult Guncelle(RandevuNotu notu, string? randevuDurumu)
        {
            var mevcutNot = _randevuNotuBusiness.RandevuNotuGetir(notu.RandevuNotID);
            var eskiSonrakiTarih = mevcutNot?.SonrakiRandevuTarihi;
            var kullaniciGirdiTarih = notu.SonrakiRandevuTarihi;

            bool yeniTarihGirildi = kullaniciGirdiTarih.HasValue && kullaniciGirdiTarih != eskiSonrakiTarih;
            notu.SonrakiRandevuTarihi = eskiSonrakiTarih; // randevu oluşana kadar değiştirmiyoruz

            _randevuNotuBusiness.RandevuNotuGuncelle(notu);
            UygulaRandevuDurumu(notu.RandevuTarihID, randevuDurumu);

            TempData["BasariMesaji"] = "Randevu notu güncellendi.";

            if (yeniTarihGirildi)
            {
                return RedirectToAction("Ekle", "Randevu", new
                {
                    hastaTC = notu.HastaTC,
                    doktorID = notu.DoktorID,
                    tarih = kullaniciGirdiTarih!.Value.ToString("yyyy-MM-dd"),
                    kaynak = "randevuNotu",
                    randevuNotID = notu.RandevuNotID
                });
            }

            return RedirectToAction("Detay", "Randevu", new { randevuTarihID = notu.RandevuTarihID });
        }

        private void UygulaRandevuDurumu(int randevuTarihID, string? randevuDurumu)
        {
            if (string.IsNullOrWhiteSpace(randevuDurumu))
            {
                return; // "Değiştirme" seçilmiş, dokunmuyoruz
            }

            try
            {
                switch (randevuDurumu)
                {
                    case "Tamamlandı":
                        _randevuBusiness.RandevuTamamlandiIsaretle(randevuTarihID);
                        break;
                    case "Gelmedi":
                        _randevuBusiness.RandevuGelmediIsaretle(randevuTarihID);
                        break;
                    case "İptal":
                        _randevuBusiness.RandevuIptalEt(randevuTarihID);
                        break;
                }
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.Message;
            }
        }
    }
}