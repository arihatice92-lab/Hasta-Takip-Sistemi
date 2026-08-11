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
        private readonly KullaniciBusiness _kullaniciBusiness;

        public TestSonucController(
            HastaTestSonucBusiness testSonucBusiness,
            TestBusiness testBusiness,
            PsikologBusiness psikologBusiness,
            TestAltKumeBusiness testAltKumeBusiness,
            AltKumeSonucBusiness altKumeSonucBusiness,
            KullaniciBusiness kullaniciBusiness)
        {
            _testSonucBusiness = testSonucBusiness;
            _testBusiness = testBusiness;
            _psikologBusiness = psikologBusiness;
            _testAltKumeBusiness = testAltKumeBusiness;
            _altKumeSonucBusiness = altKumeSonucBusiness;
            _kullaniciBusiness = kullaniciBusiness;
        }
        private bool BuKayitIcinIslemYapabilirMi(byte psikologID)
        {
            if (User.IsInRole("Yönetici"))
            {
                return true;
            }

            var kullaniciAdi = User.Identity?.Name;
            if (string.IsNullOrEmpty(kullaniciAdi))
            {
                return false;
            }

            var kullanici = _kullaniciBusiness.KullaniciGetir(kullaniciAdi);
            return kullanici?.PsikologID == psikologID;
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
            if (!BuKayitIcinIslemYapabilirMi(testSonuc.PsikologID))
            {
                TempData["HataMesaji"] = "Bu işlem için yetkiniz yok.";
                return RedirectToAction("Detay", "Hasta", new { tc = testSonuc.HastaTC, tab = "testOlcek" });
            }

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
            if (!BuKayitIcinIslemYapabilirMi(testSonuc.PsikologID))
            {
                TempData["HataMesaji"] = "Bu işlem için yetkiniz yok.";
                return RedirectToAction("Detay", "Hasta", new { tc = testSonuc.HastaTC, tab = "testOlcek" });
            }
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
            var mevcutKayit = _testSonucBusiness.TestSonucGetir(testSonuc.TestSonucID);
            if (mevcutKayit == null) return NotFound();
            if (!BuKayitIcinIslemYapabilirMi(mevcutKayit.PsikologID))
            {
                TempData["HataMesaji"] = "Bu işlem için yetkiniz yok.";
                return RedirectToAction("Detay", "Hasta", new { tc = testSonuc.HastaTC, tab = "testOlcek" });
            }
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
