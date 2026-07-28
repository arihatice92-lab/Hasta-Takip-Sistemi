using HastaTakip.Business;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly KullaniciBusiness _kullaniciBusiness;

        public AccountController(KullaniciBusiness kullaniciBusiness)
        {
            _kullaniciBusiness = kullaniciBusiness;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string kullaniciAdi, string sifre)
        {
            var kullanici = _kullaniciBusiness.GirisYap(kullaniciAdi, sifre);

            if (kullanici == null)
            {
                ViewBag.Hata = "Kullanıcı adı veya şifre hatalı.";
                return View();
            }

            HttpContext.Session.SetInt32("KullaniciID", kullanici.KullaniciID);
            HttpContext.Session.SetString("KullaniciAdi", kullanici.KullaniciAdi);
            HttpContext.Session.SetString("AdSoyad", kullanici.AdSoyad);
            HttpContext.Session.SetInt32("RolID", kullanici.RolID);

            return RedirectToAction("Index", "Hasta");
        }
    }
}
