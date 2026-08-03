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

        public HastaController(HastaBusiness hastaBusiness)
        {
            _hastaBusiness = hastaBusiness;
        }

        // GET: /Hasta
        //public IActionResult Index()
        //{
        //    var hastalar = _hastaBusiness.HastaListele();
        //    return View(hastalar);
        //}

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
        public IActionResult Detay(string tc)
        {
            var hasta = _hastaBusiness.HastaGetir(tc);

            if (hasta == null)
            {
                return NotFound();
            }

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
    }
}