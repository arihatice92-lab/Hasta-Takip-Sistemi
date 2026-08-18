using HastaTakip.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    [Authorize(Roles = "Psikolog")]
    public class PsikologPaneliController: Controller
    {
        private readonly KullaniciBusiness _kullaniciBusiness;
        private readonly PsikologBusiness _psikologBusiness;
        private readonly PsikologRandevuBusiness _psikologRandevuBusiness;
        private readonly HastaBusiness _hastaBusiness;
        private readonly PsikologRandevuSaatBusiness _psikologRandevuSaatBusiness;

        public PsikologPaneliController(
            KullaniciBusiness kullaniciBusiness,
            PsikologBusiness psikologBusiness,
            PsikologRandevuBusiness psikologRandevuBusiness,
            HastaBusiness hastaBusiness,
            PsikologRandevuSaatBusiness psikologRandevuSaatBusiness)
        {
            _kullaniciBusiness = kullaniciBusiness;
            _psikologBusiness = psikologBusiness;
            _psikologRandevuBusiness = psikologRandevuBusiness;
            _hastaBusiness = hastaBusiness;
            _psikologRandevuSaatBusiness = psikologRandevuSaatBusiness;
        }
        public IActionResult Index(string? hastaTC)
        {
            var kullaniciAdi = User.Identity?.Name;
            var kullanici = !string.IsNullOrEmpty(kullaniciAdi) ? _kullaniciBusiness.KullaniciGetir(kullaniciAdi) : null;

            if (kullanici?.PsikologID == null)
            {
                return Content("Bu kullanıcıya bağlı bir psikolog kaydı bulunamadı.");
            }

            ViewBag.Psikolog = _psikologBusiness.PsikologGetir(kullanici.PsikologID.Value);

            var (psikologRandevular, _) = _psikologRandevuBusiness.RandevuListele(
                ara: null,
                siralama: "TarihYeni",
                baslangicTarihi: DateTime.Today,
                bitisTarihi: DateTime.Today,
                psikologID: kullanici.PsikologID,
                hastaTC: null,
                durum: null,
                sayfa: 1,
                sayfaBoyutu: 100);

            var hastalar = psikologRandevular
                .Select(r => r.HastaTC)
                .Distinct()
                .Select(tc => _hastaBusiness.HastaGetir(tc))
                .Where(h => h != null)
                .ToDictionary(h => h!.HastaTC);

            ViewBag.PsikologRandevular = psikologRandevular;
            ViewBag.Hastalar = hastalar;
            ViewBag.Saatler = _psikologRandevuSaatBusiness.SaatleriListele().ToDictionary(s => s.SaatID);
            ViewBag.SeciliHastaTC = hastaTC;

            if (!string.IsNullOrWhiteSpace(hastaTC))
            {
                ViewBag.SeciliHasta = _hastaBusiness.HastaGetir(hastaTC);
            }

            return View();
        }
    }

}
