using HastaTakip.DataAccess;
using HastaTakip.Entities;

namespace HastaTakip.Business
{
    public class KullaniciBusiness
    {
        private readonly KullaniciDal _kullaniciDal;

        public KullaniciBusiness(KullaniciDal kullaniciDal)
        {
            _kullaniciDal = kullaniciDal;
        }

        public Kullanici? GirisYap(string kullaniciAdi, string sifre)
        {
            var kullanici = _kullaniciDal.KullaniciGetir(kullaniciAdi);

            if (kullanici == null || !kullanici.KullaniciAktif)
                return null;

            bool sifreDogruMu = BCrypt.Net.BCrypt.Verify(sifre, kullanici.SifreHash);
            if (!sifreDogruMu)
                return null;

            _kullaniciDal.SonGirisGuncelle(kullanici.KullaniciID);

            return kullanici;
        }
        public void SifreDegistir(int kullaniciID, string eskiSifre, string yeniSifre)
        {
            var kullanici = _kullaniciDal.KullaniciGetirById(kullaniciID);

            if (kullanici == null)
                throw new Exception("Kullanıcı bulunamadı.");

            bool eskiSifreDogruMu = BCrypt.Net.BCrypt.Verify(eskiSifre, kullanici.SifreHash);
            if (!eskiSifreDogruMu)
                throw new Exception("Mevcut şifreniz hatalı.");

            if (yeniSifre.Length < 6)
                throw new Exception("Yeni şifre en az 6 karakter olmalıdır.");

            string yeniHash = BCrypt.Net.BCrypt.HashPassword(yeniSifre);
            _kullaniciDal.SifreGuncelle(kullaniciID, yeniHash);
        }
        public void KullaniciEkle(Kullanici kullanici, string sifre)
        {
            kullanici.SifreHash = BCrypt.Net.BCrypt.HashPassword(sifre);
            _kullaniciDal.KullaniciEkle(kullanici);
        }

        public Kullanici? KullaniciGetir(string kullaniciAdi)
        {
            return _kullaniciDal.KullaniciGetir(kullaniciAdi);
        }

        public List<Kullanici> KullaniciListele()
        {
            return _kullaniciDal.KullaniciListele();
        }

        public void KullaniciPasifeAl(int kullaniciID)
        {
            _kullaniciDal.KullaniciPasifeAl(kullaniciID);
        }

        public void KullaniciAktifEt(int kullaniciID)
        {
            _kullaniciDal.KullaniciAktifEt(kullaniciID);
        }

        // Yönetici tarafından şifre sıfırlama — mevcut şifre kontrolü YOK,
        // çünkü yönetici zaten yetkili
        public void SifreSifirla(int kullaniciID, string yeniSifre)
        {
            if (yeniSifre.Length < 6)
                throw new Exception("Yeni şifre en az 6 karakter olmalıdır.");

            string yeniHash = BCrypt.Net.BCrypt.HashPassword(yeniSifre);
            _kullaniciDal.SifreGuncelle(kullaniciID, yeniHash);
        }
    }
}