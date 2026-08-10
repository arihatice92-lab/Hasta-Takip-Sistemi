using System.Collections.Generic;
using HastaTakip.DataAccess;
using HastaTakip.Entities;

namespace HastaTakip.Business
{
    public class AileOykusuBusiness
    {
        private readonly AileOykusuDal _dal;
        public AileOykusuBusiness(AileOykusuDal dal) { _dal = dal; }

        public int AileOykusuEkle(AileOykusu oyku, int kullaniciID) => _dal.AileOykusuEkle(oyku, kullaniciID);
        public AileOykusu? AileOykusuGetir(int aileOykuID) => _dal.AileOykusuGetir(aileOykuID);
        public void AileOykusuGuncelle(AileOykusu oyku, int kullaniciID) => _dal.AileOykusuGuncelle(oyku, kullaniciID);
        public List<AileOykusu> HastaAileOykusuListele(string hastaTC) => _dal.HastaAileOykusuListele(hastaTC);
        
    }
}