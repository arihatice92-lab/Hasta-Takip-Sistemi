using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    [Authorize(Roles = "Yönetici")]
    public class IlacYonetimController : Controller
    {
        private readonly IlacBusiness _ilacBusiness;
        public IlacYonetimController(IlacBusiness ilacBusiness) { _ilacBusiness = ilacBusiness; }

        public IActionResult Index() => View(_ilacBusiness.IlaclariListele());

        public IActionResult Ekle() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(Ilac ilac)
        {
            try
            {
                _ilacBusiness.IlacEkle(ilac);
                TempData["BasariMesaji"] = "İlaç eklendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(ilac);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Sil(short ilacID)
        {
            try
            {
                _ilacBusiness.IlacSil(ilacID);
                TempData["BasariMesaji"] = "İlaç silindi.";
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}