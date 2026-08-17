using System;
using System.Collections.Generic;
using System.Data;
using HastaTakip.Entities;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class DoktorIzniDal
    {
        private readonly DbHelper _dbHelper;
        public DoktorIzniDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public void IzinEkle(DoktorIzni izin)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_DoktorIzniEkle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@doktorID", izin.DoktorID);
            command.Parameters.AddWithValue("@izinTuru", izin.IzinTuru);
            command.Parameters.AddWithValue("@baslangicTarihi", izin.BaslangicTarihi);
            command.Parameters.AddWithValue("@bitisTarihi", izin.BitisTarihi);
            command.Parameters.AddWithValue("@aciklama",
                string.IsNullOrWhiteSpace(izin.Aciklama) ? DBNull.Value : (object)izin.Aciklama);
            command.Parameters.AddWithValue("@ekleyenKullaniciID", izin.EkleyenKullaniciID!.Value);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<DoktorIzni> IzinleriListele(short doktorID)
        {
            var liste = new List<DoktorIzni>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_DoktorIzinleriListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@doktorID", doktorID);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(new DoktorIzni
                {
                    IzinID = (int)reader["izinID"],
                    DoktorID = (short)reader["doktorID"],
                    IzinTuru = reader["izinTuru"].ToString()!,
                    BaslangicTarihi = (DateTime)reader["baslangicTarihi"],
                    BitisTarihi = (DateTime)reader["bitisTarihi"],
                    Aciklama = reader["aciklama"] == DBNull.Value ? null : reader["aciklama"].ToString(),
                    EkleyenKullaniciID = reader["ekleyenKullaniciID"] == DBNull.Value ? null : (int?)reader["ekleyenKullaniciID"],
                    EklemeTarihi = (DateTime)reader["eklemeTarihi"]
                });
            }
            return liste;
        }

        public void IzinSil(int izinID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_DoktorIzniSil", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@izinID", izinID);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<RandevuCakismasi> RandevuCakismalariGetir(short doktorID, DateTime baslangicTarihi, DateTime bitisTarihi)
        {
            var liste = new List<RandevuCakismasi>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_DoktorRandevuCakismalari", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@doktorID", doktorID);
            command.Parameters.AddWithValue("@baslangicTarihi", baslangicTarihi);
            command.Parameters.AddWithValue("@bitisTarihi", bitisTarihi);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(new RandevuCakismasi
                {
                    RandevuTarihID = (int)reader["randevuTarihID"],
                    RandevuTarih = (DateTime)reader["randevuTarih"],
                    Saat = (TimeSpan)reader["randevuBaslangicSaat"],
                    HastaAdSoyad = $"{reader["hastaAd"]} {reader["hastaSoyad"]}"
                });
            }
            return liste;
        }
    }
}