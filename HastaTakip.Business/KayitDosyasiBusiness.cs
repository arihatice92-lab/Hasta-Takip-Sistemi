using System.Collections.Generic;
using HastaTakip.DataAccess;
using HastaTakip.Entities;

namespace HastaTakip.Business
{
    public class KayitDosyasiBusiness
    {
        private readonly KayitDosyasiDal _dal;
        public KayitDosyasiBusiness(KayitDosyasiDal dal) { _dal = dal; }

        public void DosyaEkle(KayitDosyasi dosya) => _dal.DosyaEkle(dosya);
        public List<KayitDosyasi> DosyalariListele(string kayitTuru, int kayitID) => _dal.DosyalariListele(kayitTuru, kayitID);
        public KayitDosyasi? DosyaGetir(int dosyaID) => _dal.DosyaGetir(dosyaID);
        public void DosyaSil(int dosyaID) => _dal.DosyaSil(dosyaID);
    }
}