using System;
using System.Collections.Generic;
using System.Data;
using HastaTakip.Entities;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class RandevuDal
    {
        private readonly DbHelper _dbHelper;

        public RandevuDal(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public int RandevuOlustur(string hastaTC, short doktorID, byte saatID, DateTime tarih)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_RandevuOlustur", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@TC", hastaTC);
            command.Parameters.AddWithValue("@DoktorID", doktorID);
            command.Parameters.AddWithValue("@SaatID", saatID);
            command.Parameters.AddWithValue("@Tarih", tarih);

            var yeniIDParam = new SqlParameter("@YeniRandevuTarihID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command.Parameters.Add(yeniIDParam);

            connection.Open();
            command.ExecuteNonQuery();

            return (int)yeniIDParam.Value;
        }

        public RandevuTarihi? RandevuGetir(int randevuTarihID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_RandevuGetir", connection);
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
            var randevular = new List<RandevuTarihi>();
            int toplamKayit;

            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_RandevuListele", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Ara",
                string.IsNullOrWhiteSpace(ara) ? DBNull.Value : (object)ara);
            command.Parameters.AddWithValue("@Siralama", siralama);
            command.Parameters.AddWithValue("@baslangicTarihi",
                baslangicTarihi.HasValue ? (object)baslangicTarihi.Value : DBNull.Value);
            command.Parameters.AddWithValue("@bitisTarihi",
                bitisTarihi.HasValue ? (object)bitisTarihi.Value : DBNull.Value);
            command.Parameters.AddWithValue("@doktorID",
                doktorID.HasValue ? (object)doktorID.Value : DBNull.Value);
            command.Parameters.AddWithValue("@hastaTC",
                string.IsNullOrWhiteSpace(hastaTC) ? DBNull.Value : (object)hastaTC);
            command.Parameters.AddWithValue("@durum",
                string.IsNullOrWhiteSpace(durum) ? DBNull.Value : (object)durum);
            command.Parameters.AddWithValue("@Sayfa", sayfa);
            command.Parameters.AddWithValue("@SayfaBoyutu", sayfaBoyutu);

            var toplamKayitParam = new SqlParameter("@ToplamKayit", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
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

        public List<DoktorTakvimGunu> DoktorTakvimAraligiGetir(short doktorID, DateTime baslangicTarih, int gunSayisi)
        {
            var liste = new List<DoktorTakvimGunu>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_DoktorTakvimAraligi", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DoktorID", doktorID);
            command.Parameters.AddWithValue("@BaslangicTarih", baslangicTarih);
            command.Parameters.AddWithValue("@GunSayisi", gunSayisi);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(new DoktorTakvimGunu
                {
                    Tarih = (DateTime)reader["Tarih"],
                    ToplamSaat = (int)reader["ToplamSaat"],
                    DoluSaat = (int)reader["DoluSaat"]
                });
            }
            return liste;
        }
        public List<DoktorTakvimSlotu> DoktorGunlukTakvimGetir(short doktorID, DateTime tarih)
        {
            var slotlar = new List<DoktorTakvimSlotu>();

            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_DoktorGunlukTakvim", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DoktorID", doktorID);
            command.Parameters.AddWithValue("@Tarih", tarih);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                slotlar.Add(new DoktorTakvimSlotu
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
        public void RandevuDurumGuncelle(int randevuTarihID, string yeniDurum)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_RandevuDurumGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@randevuTarihID", randevuTarihID);
            command.Parameters.AddWithValue("@randevuDurum", yeniDurum);

            connection.Open();
            command.ExecuteNonQuery();
        }
        public void GelisZamaniGuncelle(int randevuTarihID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_RandevuGelisZamaniGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@randevuTarihID", randevuTarihID);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void MuayeneBaslangicGuncelle(int randevuTarihID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_RandevuMuayeneBaslangicGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@randevuTarihID", randevuTarihID);
            connection.Open();
            command.ExecuteNonQuery();
        }
        private RandevuTarihi MapToRandevu(SqlDataReader reader)
        {
            return new RandevuTarihi
            {
                RandevuTarihID = (int)reader["randevuTarihID"],
                HastaTC = reader["hastaTC"].ToString()!,
                DoktorID = (short)reader["doktorID"],
                SaatID = (byte)reader["saatID"],
                RandevuTarih = (DateTime)reader["randevuTarih"],
                RandevuOlusturmaTarihi = (DateTime)reader["randevuOlusturmaTarihi"],
                RandevuDurum = reader["randevuDurum"].ToString()!,
                HastaGelisZamani = reader["hastaGelisZamani"] == DBNull.Value ? null : (DateTime?)reader["hastaGelisZamani"],
                MuayeneBaslangicZamani = reader["muayeneBaslangicZamani"] == DBNull.Value ? null : (DateTime?)reader["muayeneBaslangicZamani"]
            };
        }
    }
}