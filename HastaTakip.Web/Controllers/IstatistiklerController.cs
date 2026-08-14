using HastaTakip.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    [Authorize(Roles = "Yönetici,Sekreter")]
    public class IstatistiklerController : Controller
    {
        private readonly IstatistikBusiness _istatistikBusiness;
        public IstatistiklerController(IstatistikBusiness istatistikBusiness)
        {
            _istatistikBusiness = istatistikBusiness;
        }

        public IActionResult Index()
        {
            ViewBag.TaniCinsiyetVerisi = _istatistikBusiness.TaniCinsiyetIstatistigiGetir();
            ViewBag.YasDagilimiVerisi = _istatistikBusiness.YasDagilimiIstatistigiGetir();
            ViewBag.TaniOkulBasariVerisi = _istatistikBusiness.TaniOkulBasarisiCinsiyetIstatistigi();
            ViewBag.BMIVerisi = _istatistikBusiness.TaniCinsiyetBMIIstatistigiGetir();
            return View();
        }
    }
}