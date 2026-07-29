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

        public void KullaniciEkle(Kullanici kullanici, string sifre)
        {
            kullanici.SifreHash = BCrypt.Net.BCrypt.HashPassword(sifre);
            _kullaniciDal.KullaniciEkle(kullanici);
        }

        public Kullanici? KullaniciGetir(string kullaniciAdi)
        {
            return _kullaniciDal.KullaniciGetir(kullaniciAdi);
        }
    }
}