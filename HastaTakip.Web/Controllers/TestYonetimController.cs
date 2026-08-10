using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    [Authorize(Roles = "Yönetici")]
    public class TestYonetimController : Controller
    {
        private readonly TestBusiness _testBusiness;
        private readonly TestAltKumeBusiness _altKumeBusiness;

        public TestYonetimController(TestBusiness testBusiness, TestAltKumeBusiness altKumeBusiness)
        {
            _testBusiness = testBusiness;
            _altKumeBusiness = altKumeBusiness;
        }

        public IActionResult Index()
        {
            return View(_testBusiness.TestleriListele());
        }

        public IActionResult Ekle() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(Test test)
        {
            try
            {
                _testBusiness.TestEkle(test);
                TempData["BasariMesaji"] = "Test eklendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(test);
            }
        }

        public IActionResult Guncelle(byte testID)
        {
            var test = _testBusiness.TestGetir(testID);
            if (test == null) return NotFound();
            return View(test);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(Test test)
        {
            _testBusiness.TestGuncelle(test);
            TempData["BasariMesaji"] = "Test güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Sil(byte testID)
        {
            try
            {
                _testBusiness.TestSil(testID);
                TempData["BasariMesaji"] = "Test silindi.";
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        // Alt küme yönetimi — test bazında
        public IActionResult AltKumeler(byte testID)
        {
            var test = _testBusiness.TestGetir(testID);
            if (test == null) return NotFound();

            ViewBag.Test = test;
            ViewBag.AltKumeler = _altKumeBusiness.AltKumeleriListeleByTestID(testID);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AltKumeEkle(TestAltKume altKume)
        {
            _altKumeBusiness.AltKumeEkle(altKume);
            TempData["BasariMesaji"] = "Alt küme eklendi.";
            return RedirectToAction(nameof(AltKumeler), new { testID = altKume.TestID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AltKumeSil(byte testAltKumeID, byte testID)
        {
            try
            {
                _altKumeBusiness.AltKumeSil(testAltKumeID);
                TempData["BasariMesaji"] = "Alt küme silindi.";
            }
            catch (Exception ex)
            {
                TempData["HataMesaji"] = ex.Message;
            }
            return RedirectToAction(nameof(AltKumeler), new { testID });
        }
    }
}