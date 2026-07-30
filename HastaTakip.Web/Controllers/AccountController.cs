using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string kullaniciAdi, string sifre)
        {
            var kullanici = _kullaniciBusiness.GirisYap(kullaniciAdi, sifre);

            if (kullanici == null)
            {
                ViewBag.Hata = "Kullanıcı adı veya şifre hatalı.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, kullanici.KullaniciID.ToString()),
                new Claim(ClaimTypes.Name, kullanici.KullaniciAdi),
                new Claim("AdSoyad", kullanici.AdSoyad),
                new Claim(ClaimTypes.Role, RolAdiGetir(kullanici.RolID))
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Dashboard");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


        [Authorize]
        [HttpGet]
        public IActionResult SifreDegistir()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SifreDegistir(string eskiSifre, string yeniSifre, string yeniSifreTekrar)
        {
            if (yeniSifre != yeniSifreTekrar)
            {
                ViewBag.Hata = "Yeni şifreler birbiriyle uyuşmuyor.";
                return View();
            }

            var kullaniciIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(kullaniciIdStr, out int kullaniciID))
            {
                return RedirectToAction("Login");
            }

            try
            {
                _kullaniciBusiness.SifreDegistir(kullaniciID, eskiSifre, yeniSifre);
                ViewBag.Basari = "Şifreniz başarıyla değiştirildi.";
            }
            catch (Exception ex)
            {
                ViewBag.Hata = ex.Message;
            }

            return View();
        }

        

        private string RolAdiGetir(byte rolID)
        {
            return rolID switch
            {
                1 => "Yönetici",
                2 => "Doktor",
                3 => "Sekreter",
                4 => "Psikolog",
                _ => "Bilinmiyor"
            };
        }
    }
}