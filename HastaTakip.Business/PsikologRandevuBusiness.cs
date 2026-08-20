
using System;
using System.Collections.Generic;
using HastaTakip.DataAccess;
using HastaTakip.Entities;
using Microsoft.Data.SqlClient;

namespace HastaTakip.Business
{
    public class PsikologRandevuBusiness
    {
        private readonly PsikologRandevuDal _dal;
        public PsikologRandevuBusiness(PsikologRandevuDal dal) { _dal = dal; }

        public int RandevuOlustur(string hastaTC, byte psikologID, byte saatID, DateTime tarih)
        {
            if (tarih.Date < DateTime.Today)
            {
                throw new Exception("Geçmiş bir tarihe randevu oluşturulamaz.");
            }

            if (_dal.HastaGelecekRandevusuVarMi(hastaTC))
            {
                throw new Exception("Bu hastanın zaten planlanmış, gelecek tarihli bir psikolog randevusu bulunuyor. Yeni randevu almadan önce mevcut randevunun tamamlanması, iptal edilmesi ya da gerçekleşmiş olması gerekir.");
            }

            var sonGelmediTarihi = _dal.HastaSonGelmediTarihi(hastaTC);
            if (sonGelmediTarihi.HasValue)
            {
                var yasakBitisTarihi = sonGelmediTarihi.Value.AddDays(14);
                if (tarih.Date < yasakBitisTarihi.Date)
                {
                    throw new Exception($"Bu hasta {sonGelmediTarihi.Value:dd.MM.yyyy} tarihli psikolog randevusuna gelmediği için {yasakBitisTarihi:dd.MM.yyyy} tarihine kadar yeni randevu alamaz.");
                }
            }

            try
            {
                return _dal.RandevuOlustur(hastaTC, psikologID, saatID, tarih);
            }
            catch (SqlException ex) when (ex.Message.Contains("Bu saatte randevu bulunmaktadır"))
            {
                throw new Exception("Seçilen psikolog ve saat için zaten bir randevu bulunuyor. Lütfen farklı bir saat seçin.");
            }
        }

        public PsikologRandevuTarihi? RandevuGetir(int randevuTarihID) => _dal.RandevuGetir(randevuTarihID);

        public (List<PsikologRandevuTarihi> Randevular, int ToplamKayit) RandevuListele(
            string? ara, string siralama, DateTime? baslangicTarihi, DateTime? bitisTarihi,
            byte? psikologID, string? hastaTC, string? durum, int sayfa, int sayfaBoyutu)
            => _dal.RandevuListele(ara, siralama, baslangicTarihi, bitisTarihi, psikologID, hastaTC, durum, sayfa, sayfaBoyutu);

        public void RandevuYenidenPlanla(int randevuTarihID, byte yeniPsikologID, byte yeniSaatID, DateTime yeniTarih)
        {
            if (yeniTarih.Date < DateTime.Today)
            {
                throw new Exception("Geçmiş bir tarihe randevu planlanamaz.");
            }

            try
            {
                _dal.RandevuYenidenPlanla(randevuTarihID, yeniPsikologID, yeniSaatID, yeniTarih);
            }
            catch (SqlException ex) when (ex.Message.Contains("başka bir randevu bulunmaktadır"))
            {
                throw new Exception("Seçilen psikolog ve saat için zaten bir randevu bulunuyor. Lütfen farklı bir saat seçin.");
            }
            catch (SqlException ex) when (ex.Message.Contains("artık düzenlenemez"))
            {
                throw new Exception("Bu randevu artık düzenlenemez.");
            }
        }

        public void RandevuIptalEt(int randevuTarihID) => _dal.DurumGuncelle(randevuTarihID, "İptal");

        public void RandevuTamamlandiIsaretle(int randevuTarihID) => _dal.DurumGuncelle(randevuTarihID, "Tamamlandı");

        public void RandevuGelmediIsaretle(int randevuTarihID) => _dal.DurumGuncelle(randevuTarihID, "Gelmedi");

        public void GelisZamaniGuncelle(int randevuTarihID) => _dal.GelisZamaniGuncelle(randevuTarihID);

        public void TestBaslangicGuncelle(int randevuTarihID) => _dal.TestBaslangicGuncelle(randevuTarihID);

        public List<PsikologTakvimSlotu> GunlukTakvimGetir(byte psikologID, DateTime tarih) => _dal.GunlukTakvimGetir(psikologID, tarih);

        public List<PsikologTakvimGunu> TakvimAraligiGetir(byte psikologID, DateTime baslangicTarih, int gunSayisi)
            => _dal.TakvimAraligiGetir(psikologID, baslangicTarih, gunSayisi);
    }
}