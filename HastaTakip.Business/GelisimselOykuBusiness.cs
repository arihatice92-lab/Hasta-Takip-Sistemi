using System.Collections.Generic;
using HastaTakip.DataAccess;
using HastaTakip.Entities;

namespace HastaTakip.Business
{
    public class GelisimselOykuBusiness
    {
        private readonly GelisimselOykuDal _dal;
        public GelisimselOykuBusiness(GelisimselOykuDal dal) { _dal = dal; }

        public void GelisimselOykuEkle(GelisimselOyku oyku, int kullaniciID) => _dal.GelisimselOykuEkle(oyku, kullaniciID);
        public GelisimselOyku? GelisimselOykuGetir(int gelisimOykuID) => _dal.GelisimselOykuGetir(gelisimOykuID);
        public void GelisimselOykuGuncelle(GelisimselOyku oyku, int kullaniciID) => _dal.GelisimselOykuGuncelle(oyku, kullaniciID);
        public List<GelisimselOyku> HastaGelisimselOykuListele(string hastaTC) => _dal.HastaGelisimselOykuListele(hastaTC);
    }
}