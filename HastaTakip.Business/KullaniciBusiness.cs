using HastaTakip.DataAccess;
using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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
            return _kullaniciDal.GirisYap(kullaniciAdi, sifre);
        }

        public void KullaniciEkle(Kullanici kullanici, string sifre)
        {
            _kullaniciDal.KullaniciEkle(kullanici, sifre);
        }

        public Kullanici? KullaniciGetir(string kullaniciAdi)
        {
            return _kullaniciDal.KullaniciGetir(kullaniciAdi);
        }
    }
}
