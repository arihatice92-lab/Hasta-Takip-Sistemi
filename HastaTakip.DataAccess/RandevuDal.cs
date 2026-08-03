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

        public void RandevuOlustur(string hastaTC, short doktorID, byte saatID, DateTime tarih)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_RandevuOlustur", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@TC", hastaTC);
            command.Parameters.AddWithValue("@DoktorID", doktorID);
            command.Parameters.AddWithValue("@SaatID", saatID);
            command.Parameters.AddWithValue("@Tarih", tarih);

            connection.Open();
            command.ExecuteNonQuery();
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
                RandevuDurum = reader["randevuDurum"].ToString()!
            };
        }
    }
}