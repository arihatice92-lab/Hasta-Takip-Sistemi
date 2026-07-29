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
        public IActionResult Index()
        {
            var hastalar = _hastaBusiness.HastaListele();
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
                TempData["Basari"] = $"✓ {hasta.HastaAd} {hasta.HastaSoyad} isimli hasta başarıyla sisteme kaydedildi.";//buraya  Dosya No: {hasta.HastaDosyaNo} eklediğimde hata vermedi ama test aşamasında kayıt olmadı, zaman aşımı oldu??
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