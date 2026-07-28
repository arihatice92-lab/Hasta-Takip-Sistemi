using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    public class HastaController : Controller
    {
        private readonly HastaBusiness _hastaBusiness;

        public HastaController(HastaBusiness hastaBusiness)
        {
            _hastaBusiness = hastaBusiness;
        }
        private bool GirisYapilmisMi()
        {
            return HttpContext.Session.GetInt32("KullaniciID") != null;
        }
        // GET: /Hasta
        public IActionResult Index()
        {
            if (!GirisYapilmisMi())
                return RedirectToAction("Login", "Account");

            var hastalar = _hastaBusiness.HastaListele();
            return View(hastalar);
        }

        // GET: /Hasta/Detay/12345678901
        public IActionResult Detay(string tc)
        {
            if (!GirisYapilmisMi())
                return RedirectToAction("Login", "Account");

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
            if (!GirisYapilmisMi())
                return RedirectToAction("Login", "Account");

            return View();
        }

        // POST: /Hasta/Ekle
        [HttpPost]
        public IActionResult Ekle(Hasta hasta)
        {
            if (!GirisYapilmisMi())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                return View(hasta);
            }

            try
            {
                _hastaBusiness.HastaKaydet(hasta);
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
