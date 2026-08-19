using System;
using System.Collections.Generic;
using System.Data;
using HastaTakip.Entities;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class KayitDosyasiDal
    {
        private readonly DbHelper _dbHelper;
        public KayitDosyasiDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public void DosyaEkle(KayitDosyasi dosya)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_KayitDosyasiEkle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@kayitTuru", dosya.KayitTuru);
            command.Parameters.AddWithValue("@kayitID", dosya.KayitID);
            command.Parameters.AddWithValue("@dosyaAdi", dosya.DosyaAdi);
            command.Parameters.AddWithValue("@dosyaYolu", dosya.DosyaYolu);
            command.Parameters.AddWithValue("@dosyaTipi", dosya.DosyaTipi);
            command.Parameters.AddWithValue("@yukleyenKullaniciID", dosya.YukleyenKullaniciID);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<KayitDosyasi> DosyalariListele(string kayitTuru, int kayitID)
        {
            var liste = new List<KayitDosyasi>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_KayitDosyalariListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@kayitTuru", kayitTuru);
            command.Parameters.AddWithValue("@kayitID", kayitID);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(MapToDosya(reader));
            }
            return liste;
        }

        public KayitDosyasi? DosyaGetir(int dosyaID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_KayitDosyasiGetir", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@dosyaID", dosyaID);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapToDosya(reader);
            }
            return null;
        }

        public void DosyaSil(int dosyaID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_KayitDosyasiSil", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@dosyaID", dosyaID);
            connection.Open();
            command.ExecuteNonQuery();
        }

        private KayitDosyasi MapToDosya(SqlDataReader reader)
        {
            return new KayitDosyasi
            {
                DosyaID = (int)reader["dosyaID"],
                KayitTuru = reader["kayitTuru"].ToString()!,
                KayitID = (int)reader["kayitID"],
                DosyaAdi = reader["dosyaAdi"].ToString()!,
                DosyaYolu = reader["dosyaYolu"].ToString()!,
                DosyaTipi = reader["dosyaTipi"].ToString()!,
                YukleyenKullaniciID = (int)reader["yukleyenKullaniciID"],
                YuklemeTarihi = (DateTime)reader["yuklemeTarihi"]
            };
        }
    }
}