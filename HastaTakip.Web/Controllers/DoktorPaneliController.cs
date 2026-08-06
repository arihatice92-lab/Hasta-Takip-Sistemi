using HastaTakip.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace HastaTakip.Web.Controllers
{
    [Authorize(Roles = "Doktor")]
    public class DoktorPaneliController : Controller
    {
        private readonly KullaniciBusiness _kullaniciBusiness;
        private readonly DoktorBusiness _doktorBusiness;
        private readonly RandevuBusiness _randevuBusiness;
        private readonly HastaBusiness _hastaBusiness;
        private readonly RandevuSaatBusiness _randevuSaatBusiness;

        public DoktorPaneliController(
            KullaniciBusiness kullaniciBusiness,
            DoktorBusiness doktorBusiness,
            RandevuBusiness randevuBusiness,
            HastaBusiness hastaBusiness,
            RandevuSaatBusiness randevuSaatBusiness)
        {
            _kullaniciBusiness = kullaniciBusiness;
            _doktorBusiness = doktorBusiness;
            _randevuBusiness = randevuBusiness;
            _hastaBusiness = hastaBusiness;
            _randevuSaatBusiness = randevuSaatBusiness;
        }

        public IActionResult Index(string? hastaTC)
        {
            var kullaniciAdi = User.Identity?.Name;
            var kullanici = !string.IsNullOrEmpty(kullaniciAdi) ? _kullaniciBusiness.KullaniciGetir(kullaniciAdi) : null;

            if (kullanici?.DoktorID == null)
            {
                return Content("Bu kullanıcıya bağlı bir doktor kaydı bulunamadı.");
            }

            ViewBag.Doktor = _doktorBusiness.DoktorGetir(kullanici.DoktorID.Value);

            var (randevular, _) = _randevuBusiness.RandevuListele(
                ara: null,
                siralama: "TarihYeni",
                baslangicTarihi: DateTime.Today,
                bitisTarihi: DateTime.Today,
                doktorID: kullanici.DoktorID,
                hastaTC: null,
                durum: null,
                sayfa: 1,
                sayfaBoyutu: 100);

            var hastalar = randevular
                .Select(r => r.HastaTC)
                .Distinct()
                .Select(tc => _hastaBusiness.HastaGetir(tc))
                .Where(h => h != null)
                .ToDictionary(h => h!.HastaTC);

            ViewBag.Randevular = randevular;
            ViewBag.Hastalar = hastalar;
            ViewBag.Saatler = _randevuSaatBusiness.SaatleriListele().ToDictionary(s => s.SaatID);
            ViewBag.SeciliHastaTC = hastaTC;

            if (!string.IsNullOrWhiteSpace(hastaTC))
            {
                ViewBag.SeciliHasta = _hastaBusiness.HastaGetir(hastaTC);
            }

            return View();
        }
    }
}