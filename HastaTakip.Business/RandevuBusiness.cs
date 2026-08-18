using System;
using System.Collections.Generic;
using HastaTakip.DataAccess;
using HastaTakip.Entities;
using Microsoft.Data.SqlClient;

namespace HastaTakip.Business
{
    public class RandevuBusiness
    {
        private readonly RandevuDal _randevuDal;

        public RandevuBusiness(RandevuDal randevuDal)
        {
            _randevuDal = randevuDal;
        }

        public int RandevuOlustur(string hastaTC, short doktorID, byte saatID, DateTime tarih)
        {
            if (tarih.Date < DateTime.Today)
            {
                throw new Exception("Geçmiş bir tarihe randevu oluşturulamaz.");
            }

            if (_randevuDal.HastaGelecekRandevusuVarMi(hastaTC))
            {
                throw new Exception("Bu hastanın zaten planlanmış, gelecek tarihli bir randevusu bulunuyor. Yeni randevu almadan önce mevcut randevunun tamamlanması, iptal edilmesi ya da gerçekleşmiş olması gerekir.");
            }

            var sonGelmediTarihi = _randevuDal.HastaSonGelmediTarihi(hastaTC);
            if (sonGelmediTarihi.HasValue)
            {
                var yasakBitisTarihi = sonGelmediTarihi.Value.AddDays(14);
                if (tarih.Date < yasakBitisTarihi.Date)
                {
                    throw new Exception($"Bu hasta {sonGelmediTarihi.Value:dd.MM.yyyy} tarihli randevusuna gelmediği için {yasakBitisTarihi:dd.MM.yyyy} tarihine kadar yeni randevu alamaz.");
                }
            }

            try
            {
                return _randevuDal.RandevuOlustur(hastaTC, doktorID, saatID, tarih);
            }
            catch (SqlException ex) when (ex.Message.Contains("Bu saatte randevu bulunmaktadır"))
            {
                throw new Exception("Seçilen doktor ve saat için zaten bir randevu bulunuyor. Lütfen farklı bir saat seçin.");
            }
        }

        public RandevuTarihi? RandevuGetir(int randevuTarihID)
        {
            return _randevuDal.RandevuGetir(randevuTarihID);
        }

        public (List<RandevuTarihi> Randevular, int ToplamKayit) RandevuListele(
            string? ara,
            string siralama,
            DateTime? baslangicTarihi,
            DateTime? bitisTarihi,
            short? doktorID,
            string? hastaTC,
            string? durum,
            int sayfa,
            int sayfaBoyutu)
        {
            return _randevuDal.RandevuListele(ara, siralama, baslangicTarihi, bitisTarihi, doktorID, hastaTC, durum, sayfa, sayfaBoyutu);
        }

        public List<DoktorTakvimSlotu> DoktorGunlukTakvimGetir(short doktorID, DateTime tarih)
        {
            return _randevuDal.DoktorGunlukTakvimGetir(doktorID, tarih);
        }

        public List<DoktorTakvimGunu> DoktorTakvimAraligiGetir(short doktorID, DateTime baslangicTarih, int gunSayisi)
    => _randevuDal.DoktorTakvimAraligiGetir(doktorID, baslangicTarih, gunSayisi);
        public void RandevuIptalEt(int randevuTarihID)
        {
            _randevuDal.RandevuDurumGuncelle(randevuTarihID, "İptal");
        }

        public void RandevuTamamlandiIsaretle(int randevuTarihID)
        {
            try
            {
                _randevuDal.RandevuDurumGuncelle(randevuTarihID, "Tamamlandı");
            }
            catch (SqlException ex) when (ex.Message.Contains("Önce randevu notu giriniz"))
            {
                throw new Exception("Randevuyu tamamlandı olarak işaretlemeden önce bir randevu notu girmelisiniz.");
            }
        }

        public void RandevuGelmediIsaretle(int randevuTarihID)
        {
            _randevuDal.RandevuDurumGuncelle(randevuTarihID, "Gelmedi");
        }
        public void GelisZamaniGuncelle(int randevuTarihID) => _randevuDal.GelisZamaniGuncelle(randevuTarihID);
        public void MuayeneBaslangicGuncelle(int randevuTarihID) => _randevuDal.MuayeneBaslangicGuncelle(randevuTarihID);
    }
}