using System;
using HastaTakip.DataAccess;
using HastaTakip.Entities;

namespace HastaTakip.Business

{
    public class RandevuNotuBusiness
    {
        private readonly RandevuNotuDal _dal;
        private readonly RandevuBusiness _randevuBusiness;
        public RandevuNotuBusiness(RandevuNotuDal dal, RandevuBusiness randevuBusiness) { 
            _dal = dal; _randevuBusiness = randevuBusiness;
        }

        public short RandevuNotuEkle(RandevuNotu notu)
        {

            var randevu = _randevuBusiness.RandevuGetir(notu.RandevuTarihID);
            if (randevu == null)
            {
                throw new Exception("Randevu bulunamadı.");
            }

            if (randevu.RandevuTarih.Date > DateTime.Today)
            {
                throw new Exception("Randevu tarihi gelmeden not girilemez.");
            }

            return _dal.RandevuNotuEkle(notu);
        }

        public void SonrakiTarihGuncelle(short randevuNotID, DateTime tarih) => _dal.SonrakiTarihGuncelle(randevuNotID, tarih);
        public RandevuNotu? RandevuNotuGetir(short id) => _dal.RandevuNotuGetir(id);
        public RandevuNotu? RandevuNotuGetirByRandevuTarihID(int randevuTarihID) => _dal.RandevuNotuGetirByRandevuTarihID(randevuTarihID);
        public void RandevuNotuGuncelle(RandevuNotu notu) => _dal.RandevuNotuGuncelle(notu);
        public List<HastaTakip.Entities.RandevuNotu> HastaRandevuNotlariListele(string hastaTC) => _dal.HastaRandevuNotlariListele(hastaTC);
    }
}