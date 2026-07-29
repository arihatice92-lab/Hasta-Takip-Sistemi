using HastaTakip.Entities;
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class KullaniciDal
    {
        private readonly DbHelper _dbHelper;

        public KullaniciDal(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        // Kullanıcı adına göre kullanıcı getir (sadece veri getirir, doğrulama yapmaz)
        public Kullanici? KullaniciGetir(string kullaniciAdi)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_KullaniciGetir", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);

            connection.Open();

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return MapToKullanici(reader);
            }

            return null;
        }

        // Yeni kullanıcı ekle — hash'i dışarıdan (Business'tan) hazır alır
        public void KullaniciEkle(Kullanici kullanici)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_KullaniciEkle", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@KullaniciAdi", kullanici.KullaniciAdi);
            command.Parameters.AddWithValue("@SifreHash", kullanici.SifreHash);
            command.Parameters.AddWithValue("@AdSoyad", kullanici.AdSoyad);
            command.Parameters.AddWithValue("@RolID", kullanici.RolID);
            command.Parameters.AddWithValue("@DoktorID",
                kullanici.DoktorID.HasValue ? (object)kullanici.DoktorID.Value : DBNull.Value);
            command.Parameters.AddWithValue("@PsikologID",
                kullanici.PsikologID.HasValue ? (object)kullanici.PsikologID.Value : DBNull.Value);

            connection.Open();
            command.ExecuteNonQuery();
        }

        // Son giriş tarihini güncelle
        public void SonGirisGuncelle(int kullaniciID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_SonGirisGuncelle", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@KullaniciID", kullaniciID);

            connection.Open();
            command.ExecuteNonQuery();
        }

        private Kullanici MapToKullanici(SqlDataReader reader)
        {
            return new Kullanici
            {
                KullaniciID = (int)reader["KullaniciID"],
                KullaniciAdi = reader["KullaniciAdi"].ToString()!,
                SifreHash = reader["SifreHash"].ToString()!,
                AdSoyad = reader["AdSoyad"].ToString()!,
                RolID = (byte)reader["RolID"],
                DoktorID = reader["DoktorID"] == DBNull.Value ? (short?)null : (short)reader["DoktorID"],
                PsikologID = reader["PsikologID"] == DBNull.Value ? (byte?)null : (byte)reader["PsikologID"],
                KullaniciAktif = (bool)reader["KullaniciAktif"],
                OlusturmaTarihi = (DateTime)reader["OlusturmaTarihi"],
                SonGirisTarihi = reader["SonGirisTarihi"] == DBNull.Value
                    ? null
                    : (DateTime?)reader["SonGirisTarihi"]
            };
        }
    }
}