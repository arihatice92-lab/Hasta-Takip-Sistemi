using System;
using System.Collections.Generic;
using HastaTakip.DataAccess;
using HastaTakip.Entities;

namespace HastaTakip.Business
{
    public class DoktorIzniBusiness
    {
        private readonly DoktorIzniDal _dal;
        public DoktorIzniBusiness(DoktorIzniDal dal) { _dal = dal; }

        public void IzinEkle(DoktorIzni izin)
        {
            if (izin.BitisTarihi < izin.BaslangicTarihi)
                throw new Exception("Bitiş tarihi başlangıç tarihinden önce olamaz.");
            _dal.IzinEkle(izin);
        }

        public List<DoktorIzni> IzinleriListele(short doktorID) => _dal.IzinleriListele(doktorID);
        public void IzinSil(int izinID) => _dal.IzinSil(izinID);

        public List<RandevuCakismasi> RandevuCakismalariGetir(short doktorID, DateTime baslangicTarihi, DateTime bitisTarihi)
    => _dal.RandevuCakismalariGetir(doktorID, baslangicTarihi, bitisTarihi);
    }
}