using HastaTakip.Business;
using HastaTakip.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HastaTakip.Web.Controllers
{
    [Authorize]
    public class KayitDosyasiController : Controller
    {
        private readonly KayitDosyasiBusiness _dosyaBusiness;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        private static readonly string[] IzinliUzantilar = { ".jpg", ".jpeg", ".png", ".pdf" };
        private const long MaksimumBoyut = 10 * 1024 * 1024; // 10 MB

        public KayitDosyasiController(
            KayitDosyasiBusiness dosyaBusiness,
            IWebHostEnvironment environment,
            IConfiguration configuration)
        {
            _dosyaBusiness = dosyaBusiness;
            _environment = environment;
            _configuration = configuration;
        }

        private string DosyaKlasoruYolu()
        {
            var klasorAdi = _configuration["DosyaAyarlari:KayitDosyalariKlasoru"] ?? "KayitDosyalari";
            return Path.Combine(_environment.ContentRootPath, klasorAdi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Yukle(string kayitTuru, int kayitID, IFormFile dosya, string donusUrl, string? gorunenAd)
        {
            if (dosya == null || dosya.Length == 0)
            {
                TempData["HataMesaji"] = "Lütfen bir dosya seçin.";
                return Redirect(donusUrl);
            }

            var uzanti = Path.GetExtension(dosya.FileName).ToLowerInvariant();
            if (!IzinliUzantilar.Contains(uzanti))
            {
                TempData["HataMesaji"] = "Sadece JPG, PNG veya PDF dosyaları yüklenebilir.";
                return Redirect(donusUrl);
            }

            if (dosya.Length > MaksimumBoyut)
            {
                TempData["HataMesaji"] = "Dosya boyutu 10 MB'ı geçemez.";
                return Redirect(donusUrl);
            }

            var kullaniciID = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var guidAdi = Guid.NewGuid().ToString() + uzanti;
            var tamYol = Path.Combine(DosyaKlasoruYolu(), guidAdi);

            using (var stream = new FileStream(tamYol, FileMode.Create))
            {
                await dosya.CopyToAsync(stream);
            }

            var kaydedilecekAd = string.IsNullOrWhiteSpace(gorunenAd) ? dosya.FileName : gorunenAd.Trim() + uzanti;

            _dosyaBusiness.DosyaEkle(new KayitDosyasi
            {
                KayitTuru = kayitTuru,
                KayitID = kayitID,
                DosyaAdi = kaydedilecekAd,
                DosyaYolu = guidAdi,
                DosyaTipi = dosya.ContentType,
                YukleyenKullaniciID = kullaniciID
            });

            TempData["BasariMesaji"] = "Dosya yüklendi.";
            return Redirect(donusUrl);
        }

        public IActionResult Goster(int dosyaID)
        {
            var dosya = _dosyaBusiness.DosyaGetir(dosyaID);
            if (dosya == null) return NotFound();

            var tamYol = Path.Combine(DosyaKlasoruYolu(), dosya.DosyaYolu);
            if (!System.IO.File.Exists(tamYol)) return NotFound();

            Response.Headers["Content-Disposition"] = $"inline; filename=\"{dosya.DosyaAdi}\"";
            

            return PhysicalFile(tamYol, dosya.DosyaTipi);
        }

        public IActionResult Indir(int dosyaID)
        {
            var dosya = _dosyaBusiness.DosyaGetir(dosyaID);
            if (dosya == null) return NotFound();

            var tamYol = Path.Combine(DosyaKlasoruYolu(), dosya.DosyaYolu);
            if (!System.IO.File.Exists(tamYol)) return NotFound();

            return PhysicalFile(tamYol, dosya.DosyaTipi, dosya.DosyaAdi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Sil(int dosyaID, string donusUrl)
        {
            var dosya = _dosyaBusiness.DosyaGetir(dosyaID);
            if (dosya != null)
            {
                var tamYol = Path.Combine(DosyaKlasoruYolu(), dosya.DosyaYolu);
                if (System.IO.File.Exists(tamYol))
                {
                    System.IO.File.Delete(tamYol);
                }
                _dosyaBusiness.DosyaSil(dosyaID);
                TempData["BasariMesaji"] = "Dosya silindi.";
            }
            return Redirect(donusUrl);
        }
    }
}