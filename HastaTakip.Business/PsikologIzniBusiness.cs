using System;
using System.Collections.Generic;
using HastaTakip.DataAccess;
using HastaTakip.Entities;

namespace HastaTakip.Business
{
    public class PsikologIzniBusiness
    {
        private readonly PsikologIzniDal _dal;
        public PsikologIzniBusiness(PsikologIzniDal dal) { _dal = dal; }

        public void IzinEkle(PsikologIzni izin)
        {
            if (izin.BitisTarihi < izin.BaslangicTarihi)
                throw new Exception("Bitiş tarihi başlangıç tarihinden önce olamaz.");
            _dal.IzinEkle(izin);
        }

        public List<PsikologIzni> IzinleriListele(byte psikologID) => _dal.IzinleriListele(psikologID);
        public void IzinSil(int izinID) => _dal.IzinSil(izinID);

        public List<RandevuCakismasi> RandevuCakismalariGetir(byte psikologID, DateTime baslangicTarihi, DateTime bitisTarihi)
    => _dal.RandevuCakismalariGetir(psikologID, baslangicTarihi, bitisTarihi);
    }
}