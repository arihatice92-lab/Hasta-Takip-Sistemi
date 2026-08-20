
using System;
using System.Collections.Generic;
using System.Data;
using HastaTakip.Entities;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class PsikologRandevuDal
    {
        private readonly DbHelper _dbHelper;
        public PsikologRandevuDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public int RandevuOlustur(string hastaTC, byte psikologID, byte saatID, DateTime tarih)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_PsikologRandevuOlustur", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@TC", hastaTC);
            command.Parameters.AddWithValue("@PsikologID", psikologID);
            command.Parameters.AddWithValue("@SaatID", saatID);
            command.Parameters.AddWithValue("@Tarih", tarih);

            var yeniIDParam = new SqlParameter("@YeniRandevuTarihID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command.Parameters.Add(yeniIDParam);

            connection.Open();
            command.ExecuteNonQuery();
            return (int)yeniIDParam.Value;
        }

        public PsikologRandevuTarihi? RandevuGetir(int randevuTarihID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_PsikologRandevuGetir", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@randevuTarihID", randevuTarihID);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapToRandevu(reader);
            }
            return null;
        }

        public (List<PsikologRandevuTarihi> Randevular, int ToplamKayit) RandevuListele(
            string? ara, string siralama, DateTime? baslangicTarihi, DateTime? bitisTarihi,
            byte? psikologID, string? hastaTC, string? durum, int sayfa, int sayfaBoyutu)
        {
            var randevular = new List<PsikologRandevuTarihi>();
            int toplamKayit;

            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_PsikologRandevuListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Ara", string.IsNullOrWhiteSpace(ara) ? DBNull.Value : (object)ara);
            command.Parameters.AddWithValue("@Siralama", siralama);
            command.Parameters.AddWithValue("@baslangicTarihi", baslangicTarihi.HasValue ? (object)baslangicTarihi.Value : DBNull.Value);
            command.Parameters.AddWithValue("@bitisTarihi", bitisTarihi.HasValue ? (object)bitisTarihi.Value : DBNull.Value);
            command.Parameters.AddWithValue("@psikologID", psikologID.HasValue ? (object)psikologID.Value : DBNull.Value);
            command.Parameters.AddWithValue("@hastaTC", string.IsNullOrWhiteSpace(hastaTC) ? DBNull.Value : (object)hastaTC);
            command.Parameters.AddWithValue("@durum", string.IsNullOrWhiteSpace(durum) ? DBNull.Value : (object)durum);
            command.Parameters.AddWithValue("@Sayfa", sayfa);
            command.Parameters.AddWithValue("@SayfaBoyutu", sayfaBoyutu);

            var toplamKayitParam = new SqlParameter("@ToplamKayit", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command.Parameters.Add(toplamKayitParam);

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    randevular.Add(MapToRandevu(reader));
                }
            }
            toplamKayit = (int)toplamKayitParam.Value;

            return (randevular, toplamKayit);
        }

        public void RandevuYenidenPlanla(int randevuTarihID, byte yeniPsikologID, byte yeniSaatID, DateTime yeniTarih)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_PsikologRandevuYenidenPlanla", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@randevuTarihID", randevuTarihID);
            command.Parameters.AddWithValue("@yeniPsikologID", yeniPsikologID);
            command.Parameters.AddWithValue("@yeniSaatID", yeniSaatID);
            command.Parameters.AddWithValue("@yeniTarih", yeniTarih);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public void DurumGuncelle(int randevuTarihID, string yeniDurum)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_PsikologRandevuDurumGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@randevuTarihID", randevuTarihID);
            command.Parameters.AddWithValue("@randevuDurum", yeniDurum);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void GelisZamaniGuncelle(int randevuTarihID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_PsikologRandevuGelisZamaniGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@randevuTarihID", randevuTarihID);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void TestBaslangicGuncelle(int randevuTarihID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_PsikologRandevuTestBaslangicGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@randevuTarihID", randevuTarihID);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<PsikologTakvimSlotu> GunlukTakvimGetir(byte psikologID, DateTime tarih)
        {
            var slotlar = new List<PsikologTakvimSlotu>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_PsikologGunlukTakvim", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PsikologID", psikologID);
            command.Parameters.AddWithValue("@Tarih", tarih);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                slotlar.Add(new PsikologTakvimSlotu
                {
                    SaatID = (byte)reader["saatID"],
                    BaslangicSaat = (TimeSpan)reader["randevuBaslangicSaat"],
                    BitisSaat = (TimeSpan)reader["randevuBitisSaat"],
                    RandevuTarihID = reader["randevuTarihID"] == DBNull.Value ? null : (int?)reader["randevuTarihID"],
                    HastaTC = reader["hastaTC"] == DBNull.Value ? null : reader["hastaTC"].ToString(),
                    RandevuDurum = reader["randevuDurum"] == DBNull.Value ? null : reader["randevuDurum"].ToString()
                });
            }
            return slotlar;
        }

        public List<PsikologTakvimGunu> TakvimAraligiGetir(byte psikologID, DateTime baslangicTarih, int gunSayisi)
        {
            var liste = new List<PsikologTakvimGunu>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_PsikologTakvimAraligi", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PsikologID", psikologID);
            command.Parameters.AddWithValue("@BaslangicTarih", baslangicTarih);
            command.Parameters.AddWithValue("@GunSayisi", gunSayisi);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(new PsikologTakvimGunu
                {
                    Tarih = (DateTime)reader["Tarih"],
                    ToplamSaat = (int)reader["ToplamSaat"],
                    DoluSaat = (int)reader["DoluSaat"],
                    IzinliMi = (bool)reader["IzinliMi"]
                });
            }
            return liste;
        }

        public bool HastaGelecekRandevusuVarMi(string hastaTC)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_HastaPsikologGelecekRandevuKontrol", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTC", hastaTC);
            connection.Open();
            var sonuc = command.ExecuteScalar();
            return sonuc != null && (int)sonuc > 0;
        }

        public DateTime? HastaSonGelmediTarihi(string hastaTC)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_HastaPsikologSonGelmediTarihi", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTC", hastaTC);
            connection.Open();
            var sonuc = command.ExecuteScalar();
            return sonuc == null || sonuc == DBNull.Value ? null : (DateTime)sonuc;
        }
        private PsikologRandevuTarihi MapToRandevu(SqlDataReader reader)
        {
            return new PsikologRandevuTarihi
            {
                RandevuTarihID = (int)reader["randevuTarihID"],
                HastaTC = reader["hastaTC"].ToString()!,
                PsikologID = (byte)reader["psikologID"],
                SaatID = (byte)reader["saatID"],
                RandevuTarih = (DateTime)reader["randevuTarih"],
                RandevuOlusturmaTarihi = (DateTime)reader["randevuOlusturmaTarihi"],
                RandevuDurum = reader["randevuDurum"].ToString()!,
                HastaGelisZamani = reader["hastaGelisZamani"] == DBNull.Value ? null : (DateTime?)reader["hastaGelisZamani"],
                TestBaslangicZamani = reader["testBaslangicZamani"] == DBNull.Value ? null : (DateTime?)reader["testBaslangicZamani"]
            };
        }
    }
}