using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    [Authorize]
    public class HastaController : Controller
    {
        private readonly HastaBusiness _hastaBusiness;
        private readonly RandevuBusiness _randevuBusiness;
        private readonly DoktorBusiness _doktorBusiness;
        private readonly RandevuSaatBusiness _randevuSaatBusiness;
        private readonly HastaTaniBusiness _hastaTaniBusiness;
        private readonly HastaTedaviBusiness _hastaTedaviBusiness;
        private readonly TaniBusiness _taniBusiness;
        private readonly IlacBusiness _ilacBusiness;
        private readonly HastaTestSonucBusiness _hastaTestSonucBusiness;
        private readonly HastaOlcekSonucBusiness _hastaOlcekSonucBusiness;
        private readonly TestBusiness _testBusiness;
        private readonly OlcekBusiness _olcekBusiness;
        private readonly PsikologBusiness _psikologBusiness;

        private readonly TestAltKumeBusiness _testAltKumeBusiness;

        private readonly AltKumeSonucBusiness _altKumeSonucBusiness;

        private readonly RandevuNotuBusiness _randevuNotuBusiness;

        private readonly KullaniciBusiness _kullaniciBusiness;
        private readonly AileBilgileriBusiness _aileBilgileriBusiness;
        private readonly AileOykusuBusiness _aileOykusuBusiness;
        private readonly GelisimselOykuBusiness _gelisimselOykuBusiness;
        private readonly KayitNotuBusiness _kayitNotuBusiness;
        private readonly PsikologRandevuBusiness _psikologRandevuBusiness;
        private readonly PsikologRandevuSaatBusiness _psikologRandevuSaatBusiness;

        public HastaController(
            HastaBusiness hastaBusiness, 
            RandevuBusiness randevuBusiness,
            DoktorBusiness doktorBusiness,
            RandevuSaatBusiness randevuSaatBusiness,
            HastaTaniBusiness hastaTaniBusiness,
            HastaTedaviBusiness hastaTedaviBusiness,
            TaniBusiness taniBusiness,
            IlacBusiness ilacBusiness,
            HastaTestSonucBusiness hastaTestSonucBusiness,
            HastaOlcekSonucBusiness hastaOlcekSonucBusiness,
            TestBusiness testBusiness,
            OlcekBusiness olcekBusiness,
            PsikologBusiness psikologBusiness,
            TestAltKumeBusiness testAltKumeBusiness,
            AltKumeSonucBusiness altKumeSonucBusiness,
            RandevuNotuBusiness randevuNotuBusiness,
            KullaniciBusiness kullaniciBusiness,
            AileBilgileriBusiness aileBilgileriBusiness,
            AileOykusuBusiness aileOykusuBusiness,
            GelisimselOykuBusiness gelisimselOykuBusiness,
            KayitNotuBusiness kayitNotuBusiness,
            PsikologRandevuBusiness psikologRandevuBusiness,
            PsikologRandevuSaatBusiness psikologRandevuSaatBusiness
            )
        {
            _hastaBusiness = hastaBusiness;
            _randevuBusiness = randevuBusiness;
            _doktorBusiness = doktorBusiness;
            _randevuSaatBusiness = randevuSaatBusiness;
            _hastaTaniBusiness = hastaTaniBusiness;
            _hastaTedaviBusiness = hastaTedaviBusiness;
            _taniBusiness = taniBusiness;
            _ilacBusiness = ilacBusiness;
            _hastaTestSonucBusiness = hastaTestSonucBusiness;
            _hastaOlcekSonucBusiness = hastaOlcekSonucBusiness;
            _testBusiness = testBusiness;
            _olcekBusiness = olcekBusiness;
            _psikologBusiness = psikologBusiness;
            _testAltKumeBusiness = testAltKumeBusiness;
            _altKumeSonucBusiness = altKumeSonucBusiness;
            _randevuNotuBusiness = randevuNotuBusiness;
            _kullaniciBusiness = kullaniciBusiness;
            _aileBilgileriBusiness = aileBilgileriBusiness;
            _aileOykusuBusiness = aileOykusuBusiness;
            _gelisimselOykuBusiness = gelisimselOykuBusiness;
            _kayitNotuBusiness = kayitNotuBusiness;
            _psikologRandevuBusiness = psikologRandevuBusiness;
            _psikologRandevuSaatBusiness = psikologRandevuSaatBusiness;
        }
        public IActionResult Index(
            string? ara,
            string siralama = "AdAZ",
            bool? aktif = null,
            string? cinsiyet = null,
            DateTime? baslangicTarihi = null,
            DateTime? bitisTarihi = null,
            int sayfa = 1)
        {
            const int sayfaBoyutu = 10;

            var (hastalar, toplamKayit) = _hastaBusiness.HastaAra(
                ara, siralama, aktif, cinsiyet, baslangicTarihi, bitisTarihi, sayfa, sayfaBoyutu);

            int toplamSayfa = (int)Math.Ceiling(toplamKayit / (double)sayfaBoyutu);

            ViewBag.Ara = ara;
            ViewBag.Siralama = siralama;
            ViewBag.Aktif = aktif;
            ViewBag.Cinsiyet = cinsiyet;
            ViewBag.Baslangic = baslangicTarihi;
            ViewBag.Bitis = bitisTarihi;
            ViewBag.SayfaNo = sayfa;
            ViewBag.ToplamSayfa = toplamSayfa;

            return View(hastalar);
        }

        // GET: /Hasta/Detay/12345678901
        public IActionResult Detay(string tc, int? aktifRandevuTarihID = null, string? tab = null)
        {
            var hasta = _hastaBusiness.HastaGetir(tc);

            if (hasta == null)
            {
                return NotFound();
            }
            var (randevular, _) = _randevuBusiness.RandevuListele(
                ara: null,
                siralama: "TarihYeni",
                baslangicTarihi: null,
                bitisTarihi: null,
                doktorID: null,
                hastaTC: tc,
                durum: null,
                sayfa: 1,
                sayfaBoyutu: 50);

            ViewBag.GecmisRandevular = randevular;
            ViewBag.Doktorlar = _doktorBusiness.DoktorListele(sadeceAktif: false).ToDictionary(d => d.DoktorID);
            ViewBag.Saatler = _randevuSaatBusiness.SaatleriListele().ToDictionary(s => s.SaatID);
            ViewBag.Tanilar = _hastaTaniBusiness.HastaTanilariListele(tc);
            ViewBag.Tedaviler = _hastaTedaviBusiness.HastaTedavileriListele(tc);
            ViewBag.TaniSozlugu = _taniBusiness.TanilariListele().ToDictionary(t => t.TaniID);
            ViewBag.IlacSozlugu = _ilacBusiness.IlaclariListele().ToDictionary(i => i.IlacID);
            ViewBag.TestSonuclari = _hastaTestSonucBusiness.HastaTestSonuclariListele(tc);
            ViewBag.OlcekSonuclari = _hastaOlcekSonucBusiness.HastaOlcekSonuclariListele(tc);
            ViewBag.TestSozlugu = _testBusiness.TestleriListele().ToDictionary(t => t.TestID);
            ViewBag.OlcekSozlugu = _olcekBusiness.OlcekleriListele().ToDictionary(o => o.OlcekID);
            ViewBag.PsikologSozlugu = _psikologBusiness.PsikologlariListele().ToDictionary(p => p.PsikologID);
            var testSonuclari = _hastaTestSonucBusiness.HastaTestSonuclariListele(tc);
            ViewBag.TestSonuclari = testSonuclari;
            ViewBag.AltKumeSonuclariSozluk = testSonuclari.ToDictionary(
                ts => ts.TestSonucID,
                ts => _altKumeSonucBusiness.AltKumeSonuclariListele(ts.TestSonucID));
            ViewBag.AltKumeAdSozlugu = _testAltKumeBusiness.TumAltKumeleriListele().ToDictionary(a => a.TestAltKumeID);
            ViewBag.RandevuNotlari = _randevuNotuBusiness.HastaRandevuNotlariListele(tc);
            ViewBag.RandevuSozlugu = randevular.ToDictionary(r => r.RandevuTarihID);
            ViewBag.AileBilgileriListesi = _aileBilgileriBusiness.HastaAileBilgileriListele(tc);
            ViewBag.AileOykusuListesi = _aileOykusuBusiness.HastaAileOykusuListele(tc);
            ViewBag.GelisimselOykuListesi = _gelisimselOykuBusiness.HastaGelisimselOykuListele(tc);

            var (psikologRandevular, _) = _psikologRandevuBusiness.RandevuListele(
                null, "TarihYeni", null, null, null, tc, null, 1, 100);
            ViewBag.PsikologRandevular = psikologRandevular;
            ViewBag.PsikologSozlugu = _psikologBusiness.PsikologAra(null, "AZ", "hepsi").ToDictionary(p => p.PsikologID);
            ViewBag.PsikologSaatSozlugu = _psikologRandevuSaatBusiness.SaatleriListele().ToDictionary(s => s.SaatID);

            var kullaniciIDler = ((List<HastaTakip.Entities.AileBilgileri>)ViewBag.AileBilgileriListesi).Select(a => a.SonGuncelleyenKullaniciID)
                .Concat(((List<HastaTakip.Entities.AileOykusu>)ViewBag.AileOykusuListesi).Select(a => a.SonGuncelleyenKullaniciID))
                .Concat(((List<HastaTakip.Entities.GelisimselOyku>)ViewBag.GelisimselOykuListesi).Select(a => a.SonGuncelleyenKullaniciID))
                .Where(id => id.HasValue).Select(id => id!.Value).Distinct();

            var kullanicilarSozluk = new Dictionary<int, HastaTakip.Entities.Kullanici>();
            foreach (var id in kullaniciIDler)
            {
                var k = _kullaniciBusiness.KullaniciGetirById(id);
                if (k != null) kullanicilarSozluk[id] = k;
            }
            ViewBag.Kullanicilar = kullanicilarSozluk;


            var notlarSozlugu = new Dictionary<(string, int), List<HastaTakip.Entities.KayitNotu>>();
            foreach (var ab in (List<HastaTakip.Entities.AileBilgileri>)ViewBag.AileBilgileriListesi)
            {
                notlarSozlugu[("AileBilgileri", ab.AileBilgileriID)] = _kayitNotuBusiness.NotlariListele("AileBilgileri", ab.AileBilgileriID);
            }
            foreach (var go in (List<HastaTakip.Entities.GelisimselOyku>)ViewBag.GelisimselOykuListesi)
            {
                notlarSozlugu[("GelisimselOyku", go.GelisimOykuID)] = _kayitNotuBusiness.NotlariListele("GelisimselOyku", go.GelisimOykuID);
            }
            foreach (var ao in (List<HastaTakip.Entities.AileOykusu>)ViewBag.AileOykusuListesi)
            {
                notlarSozlugu[("AileOykusu", ao.AileOykuID)] = _kayitNotuBusiness.NotlariListele("AileOykusu", ao.AileOykuID);
            }
            ViewBag.NotlarSozlugu = notlarSozlugu;

            ViewBag.Kullanicilar = kullanicilarSozluk;
            return View(hasta);
            
        }

        // GET: /Hasta/Ekle
        public IActionResult Ekle()
        {
            return View();
        }

        // POST: /Hasta/Ekle
        [HttpPost]
        public IActionResult Ekle(Hasta hasta)
        {
            if (!ModelState.IsValid)
            {
                return View(hasta);
            }

            try
            {
                _hastaBusiness.HastaKaydet(hasta);
                // Trigger dosya numarasını oluşturduğu için hastayı tekrar getiriyoruz.
                var yeniHasta = _hastaBusiness.HastaGetir(hasta.HastaTC);
                TempData["Basari"] = $"✓ {yeniHasta!.HastaAd} {yeniHasta.HastaSoyad} isimli hasta başarıyla sisteme kaydedildi. Dosya No: {yeniHasta.HastaDosyaNo}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(hasta);
            }
        }

        // GET: /Hasta/Guncelle/12345678901
        public IActionResult Guncelle(string tc)
        {
            var hasta = _hastaBusiness.HastaGetir(tc);
            if (hasta == null)
            {
                return NotFound();
            }
            return View(hasta);
        }

        // POST: /Hasta/Guncelle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(Hasta hasta)
        {
            if (!ModelState.IsValid)
            {
                return View(hasta);
            }

            try
            {
                _hastaBusiness.HastaGuncelle(hasta);
                TempData["BasariMesaji"] = "Hasta bilgileri güncellendi.";
                return RedirectToAction(nameof(Detay), new { tc = hasta.HastaTC });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(hasta);
            }
        }
    }

    
}