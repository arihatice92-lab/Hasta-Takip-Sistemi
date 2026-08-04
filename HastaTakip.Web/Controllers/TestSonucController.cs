using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HastaTakip.Web.Controllers
{
    
    [Authorize]
    public class TestSonucController : Controller
    {
        private readonly HastaTestSonucBusiness _testSonucBusiness;
        private readonly TestBusiness _testBusiness;
        private readonly PsikologBusiness _psikologBusiness;
        private readonly TestAltKumeBusiness _testAltKumeBusiness;
        private readonly AltKumeSonucBusiness _altKumeSonucBusiness;

        public TestSonucController(
            HastaTestSonucBusiness testSonucBusiness,
            TestBusiness testBusiness,
            PsikologBusiness psikologBusiness,
            TestAltKumeBusiness testAltKumeBusiness,
            AltKumeSonucBusiness altKumeSonucBusiness)
        {
            _testSonucBusiness = testSonucBusiness;
            _testBusiness = testBusiness;
            _psikologBusiness = psikologBusiness;
            _testAltKumeBusiness = testAltKumeBusiness;
            _altKumeSonucBusiness = altKumeSonucBusiness;
        }

        public IActionResult Ekle(string hastaTC)
        {
            ViewBag.HastaTC = hastaTC;
            ViewBag.TestListesi = _testBusiness.TestleriListele();
            ViewBag.PsikologListesi = _psikologBusiness.PsikologlariListele();
            ViewBag.TumAltKumeler = _testAltKumeBusiness.TumAltKumeleriListele();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ekle(TestSonuc testSonuc, int[]? altKumeID, string[]? altKumeSonuc, string[]? altKumeYorum)
        {
            var yeniTestSonucID = _testSonucBusiness.TestSonucEkle(testSonuc);

            if (altKumeID != null)
            {
                for (int i = 0; i < altKumeID.Length; i++)
                {
                    _altKumeSonucBusiness.AltKumeSonucEkle(new AltKumeSonuc
                    {
                        HastaTC = testSonuc.HastaTC,
                        AltKumeID = (byte)altKumeID[i],
                        TestSonucID = yeniTestSonucID,
                        AltKumeSonucDeger = altKumeSonuc != null && i < altKumeSonuc.Length ? altKumeSonuc[i] : null,
                        AltKumeYorum = altKumeYorum != null && i < altKumeYorum.Length ? altKumeYorum[i] : null
                    });
                }
            }

            TempData["BasariMesaji"] = "Test sonucu kaydedildi.";
            return RedirectToAction("Detay", "Hasta", new { tc = testSonuc.HastaTC, tab = "testOlcek" });
        }

        public IActionResult Guncelle(int testSonucID)
        {
            var testSonuc = _testSonucBusiness.TestSonucGetir(testSonucID);
            if (testSonuc == null) return NotFound();

            ViewBag.TestListesi = _testBusiness.TestleriListele();
            ViewBag.PsikologListesi = _psikologBusiness.PsikologlariListele();
            ViewBag.TumAltKumeler = _testAltKumeBusiness.TumAltKumeleriListele();
            ViewBag.MevcutAltKumeSonuclari = _altKumeSonucBusiness.AltKumeSonuclariListele(testSonucID);

            return View(testSonuc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guncelle(TestSonuc testSonuc, int[]? altKumeID, string[]? altKumeSonuc, string[]? altKumeYorum)
        {
            _testSonucBusiness.TestSonucGuncelle(testSonuc);

            _altKumeSonucBusiness.AltKumeSonuclariSil(testSonuc.TestSonucID);

            if (altKumeID != null)
            {
                for (int i = 0; i < altKumeID.Length; i++)
                {
                    _altKumeSonucBusiness.AltKumeSonucEkle(new AltKumeSonuc
                    {
                        HastaTC = testSonuc.HastaTC,
                        AltKumeID = (byte)altKumeID[i],
                        TestSonucID = testSonuc.TestSonucID,
                        AltKumeSonucDeger = altKumeSonuc != null && i < altKumeSonuc.Length ? altKumeSonuc[i] : null,
                        AltKumeYorum = altKumeYorum != null && i < altKumeYorum.Length ? altKumeYorum[i] : null
                    });
                }
            }

            TempData["BasariMesaji"] = "Test sonucu güncellendi.";
            return RedirectToAction("Detay", "Hasta", new { tc = testSonuc.HastaTC, tab = "testOlcek" });
        }
    }
}
