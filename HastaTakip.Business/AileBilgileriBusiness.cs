using System.Collections.Generic;
using HastaTakip.DataAccess;
using HastaTakip.Entities;

namespace HastaTakip.Business
{
    public class AileBilgileriBusiness
    {
        private readonly AileBilgileriDal _dal;
        public AileBilgileriBusiness(AileBilgileriDal dal) { _dal = dal; }

        public void AileBilgileriEkle(AileBilgileri bilgi, int kullaniciID) => _dal.AileBilgileriEkle(bilgi, kullaniciID);
        public AileBilgileri? AileBilgileriGetir(int aileBilgileriID) => _dal.AileBilgileriGetir(aileBilgileriID);
        public void AileBilgileriGuncelle(AileBilgileri bilgi, int kullaniciID) => _dal.AileBilgileriGuncelle(bilgi, kullaniciID);
        public List<AileBilgileri> HastaAileBilgileriListele(string hastaTC) => _dal.HastaAileBilgileriListele(hastaTC);
    }
}