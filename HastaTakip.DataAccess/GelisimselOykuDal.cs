using System;
using System.Collections.Generic;
using System.Data;
using HastaTakip.Entities;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class GelisimselOykuDal
    {
        private readonly DbHelper _dbHelper;
        public GelisimselOykuDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public void GelisimselOykuEkle(GelisimselOyku oyku, int kullaniciID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_GelisimselOykuEkle", connection);
            command.CommandType = CommandType.StoredProcedure;
            EkleParametreler(command, oyku);
            command.Parameters.AddWithValue("@kullaniciID", kullaniciID);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public GelisimselOyku? GelisimselOykuGetir(int gelisimOykuID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_GelisimselOykuGetir", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@gelisimOykuID", gelisimOykuID);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapToGelisimselOyku(reader);
            }
            return null;
        }

        public void GelisimselOykuGuncelle(GelisimselOyku oyku, int kullaniciID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_GelisimselOykuGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@gelisimOykuID", oyku.GelisimOykuID);
            OrtakParametreler(command, oyku);
            command.Parameters.AddWithValue("@kullaniciID", kullaniciID);
            connection.Open();
            command.ExecuteNonQuery();
        }
        private void OrtakParametreler(SqlCommand command, GelisimselOyku oyku)
        {
            command.Parameters.AddWithValue("@dogumAnneYasi", (object?)oyku.DogumAnneYasi ?? DBNull.Value);
            command.Parameters.AddWithValue("@dogumBabaYasi", (object?)oyku.DogumBabaYasi ?? DBNull.Value);
            command.Parameters.AddWithValue("@dogumHaftasi", (object?)oyku.DogumHaftasi ?? DBNull.Value);
            command.Parameters.AddWithValue("@dogumSekli",
                string.IsNullOrWhiteSpace(oyku.DogumSekli) ? DBNull.Value : (object)oyku.DogumSekli);
            command.Parameters.AddWithValue("@dogumKomplikasyonu",
                string.IsNullOrWhiteSpace(oyku.DogumKomplikasyonu) ? DBNull.Value : (object)oyku.DogumKomplikasyonu);
            command.Parameters.AddWithValue("@dogumAgirligi", (object?)oyku.DogumAgirligi ?? DBNull.Value);
            command.Parameters.AddWithValue("@planliGebelikMi", (object?)oyku.PlanliGebelikMi ?? DBNull.Value);
            command.Parameters.AddWithValue("@gebeKalmadaGucluk",
                string.IsNullOrWhiteSpace(oyku.GebeKalmadaGucluk) ? DBNull.Value : (object)oyku.GebeKalmadaGucluk);
            command.Parameters.AddWithValue("@aileCinsiyetBeklentisi",
                string.IsNullOrWhiteSpace(oyku.AileCinsiyetBeklentisi) ? DBNull.Value : (object)oyku.AileCinsiyetBeklentisi);
            command.Parameters.AddWithValue("@aileDogumaTepki",
                string.IsNullOrWhiteSpace(oyku.AileDogumaTepki) ? DBNull.Value : (object)oyku.AileDogumaTepki);
            command.Parameters.AddWithValue("@oturmaYasi", (object?)oyku.OturmaYasi ?? DBNull.Value);
            command.Parameters.AddWithValue("@emeklemeYasi", (object?)oyku.EmeklemeYasi ?? DBNull.Value);
            command.Parameters.AddWithValue("@yurumeYasi", (object?)oyku.YurumeYasi ?? DBNull.Value);
            command.Parameters.AddWithValue("@ilkSozcukYasi", (object?)oyku.IlkSozcukYasi ?? DBNull.Value);
            command.Parameters.AddWithValue("@ilkCumleYasi", (object?)oyku.IlkCumleYasi ?? DBNull.Value);
            command.Parameters.AddWithValue("@tuvaletEgitimi",
                string.IsNullOrWhiteSpace(oyku.TuvaletEgitimi) ? DBNull.Value : (object)oyku.TuvaletEgitimi);
            command.Parameters.AddWithValue("@gecirilenKaza",
                string.IsNullOrWhiteSpace(oyku.GecirilenKaza) ? DBNull.Value : (object)oyku.GecirilenKaza);
            command.Parameters.AddWithValue("@bebeklikDonemi",
                string.IsNullOrWhiteSpace(oyku.BebeklikDonemi) ? DBNull.Value : (object)oyku.BebeklikDonemi);
            command.Parameters.AddWithValue("@cocuklukDonemi",
                string.IsNullOrWhiteSpace(oyku.CocuklukDonemi) ? DBNull.Value : (object)oyku.CocuklukDonemi);
            command.Parameters.AddWithValue("@okulOykusu",
                string.IsNullOrWhiteSpace(oyku.OkulOykusu) ? DBNull.Value : (object)oyku.OkulOykusu);
            command.Parameters.AddWithValue("@sosyalIliskileri",
                string.IsNullOrWhiteSpace(oyku.SosyalIliskileri) ? DBNull.Value : (object)oyku.SosyalIliskileri);
            command.Parameters.AddWithValue("@kisilikOzellikleri",
                string.IsNullOrWhiteSpace(oyku.KisilikOzellikleri) ? DBNull.Value : (object)oyku.KisilikOzellikleri);
            command.Parameters.AddWithValue("@gelisimselOykuEkNot",
                string.IsNullOrWhiteSpace(oyku.GelisimselOykuEkNot) ? DBNull.Value : (object)oyku.GelisimselOykuEkNot);
        }
        public List<GelisimselOyku> HastaGelisimselOykuListele(string hastaTC)
        {
            var liste = new List<GelisimselOyku>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_HastaGelisimselOykuListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTC", hastaTC);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(MapToGelisimselOyku(reader));
            }
            return liste;
        }

        private void EkleParametreler(SqlCommand command, GelisimselOyku oyku)
        {
            command.Parameters.AddWithValue("@hastaTC", oyku.HastaTC);
            command.Parameters.AddWithValue("@dogumAnneYasi", (object?)oyku.DogumAnneYasi ?? DBNull.Value);
            command.Parameters.AddWithValue("@dogumBabaYasi", (object?)oyku.DogumBabaYasi ?? DBNull.Value);
            command.Parameters.AddWithValue("@dogumHaftasi", (object?)oyku.DogumHaftasi ?? DBNull.Value);
            command.Parameters.AddWithValue("@dogumSekli",
                string.IsNullOrWhiteSpace(oyku.DogumSekli) ? DBNull.Value : (object)oyku.DogumSekli);
            command.Parameters.AddWithValue("@dogumKomplikasyonu",
                string.IsNullOrWhiteSpace(oyku.DogumKomplikasyonu) ? DBNull.Value : (object)oyku.DogumKomplikasyonu);
            command.Parameters.AddWithValue("@dogumAgirligi", (object?)oyku.DogumAgirligi ?? DBNull.Value);
            command.Parameters.AddWithValue("@planliGebelikMi", (object?)oyku.PlanliGebelikMi ?? DBNull.Value);
            command.Parameters.AddWithValue("@gebeKalmadaGucluk",
                string.IsNullOrWhiteSpace(oyku.GebeKalmadaGucluk) ? DBNull.Value : (object)oyku.GebeKalmadaGucluk);
            command.Parameters.AddWithValue("@aileCinsiyetBeklentisi",
                string.IsNullOrWhiteSpace(oyku.AileCinsiyetBeklentisi) ? DBNull.Value : (object)oyku.AileCinsiyetBeklentisi);
            command.Parameters.AddWithValue("@aileDogumaTepki",
                string.IsNullOrWhiteSpace(oyku.AileDogumaTepki) ? DBNull.Value : (object)oyku.AileDogumaTepki);
            command.Parameters.AddWithValue("@oturmaYasi", (object?)oyku.OturmaYasi ?? DBNull.Value);
            command.Parameters.AddWithValue("@emeklemeYasi", (object?)oyku.EmeklemeYasi ?? DBNull.Value);
            command.Parameters.AddWithValue("@yurumeYasi", (object?)oyku.YurumeYasi ?? DBNull.Value);
            command.Parameters.AddWithValue("@ilkSozcukYasi", (object?)oyku.IlkSozcukYasi ?? DBNull.Value);
            command.Parameters.AddWithValue("@ilkCumleYasi", (object?)oyku.IlkCumleYasi ?? DBNull.Value);
            command.Parameters.AddWithValue("@tuvaletEgitimi",
                string.IsNullOrWhiteSpace(oyku.TuvaletEgitimi) ? DBNull.Value : (object)oyku.TuvaletEgitimi);
            command.Parameters.AddWithValue("@gecirilenKaza",
                string.IsNullOrWhiteSpace(oyku.GecirilenKaza) ? DBNull.Value : (object)oyku.GecirilenKaza);
            command.Parameters.AddWithValue("@bebeklikDonemi",
                string.IsNullOrWhiteSpace(oyku.BebeklikDonemi) ? DBNull.Value : (object)oyku.BebeklikDonemi);
            command.Parameters.AddWithValue("@cocuklukDonemi",
                string.IsNullOrWhiteSpace(oyku.CocuklukDonemi) ? DBNull.Value : (object)oyku.CocuklukDonemi);
            command.Parameters.AddWithValue("@okulOykusu",
                string.IsNullOrWhiteSpace(oyku.OkulOykusu) ? DBNull.Value : (object)oyku.OkulOykusu);
            command.Parameters.AddWithValue("@sosyalIliskileri",
                string.IsNullOrWhiteSpace(oyku.SosyalIliskileri) ? DBNull.Value : (object)oyku.SosyalIliskileri);
            command.Parameters.AddWithValue("@kisilikOzellikleri",
                string.IsNullOrWhiteSpace(oyku.KisilikOzellikleri) ? DBNull.Value : (object)oyku.KisilikOzellikleri);
            command.Parameters.AddWithValue("@gelisimselOykuEkNot",
                string.IsNullOrWhiteSpace(oyku.GelisimselOykuEkNot) ? DBNull.Value : (object)oyku.GelisimselOykuEkNot);
        }

        private GelisimselOyku MapToGelisimselOyku(SqlDataReader reader)
        {
            return new GelisimselOyku
            {
                GelisimOykuID = (int)reader["gelisimOykuID"],
                HastaTC = reader["hastaTC"].ToString()!,
                DogumAnneYasi = reader["dogumAnneYasi"] == DBNull.Value ? null : (byte?)reader["dogumAnneYasi"],
                DogumBabaYasi = reader["dogumBabaYasi"] == DBNull.Value ? null : (byte?)reader["dogumBabaYasi"],
                DogumHaftasi = reader["dogumHaftasi"] == DBNull.Value ? null : (byte?)reader["dogumHaftasi"],
                DogumSekli = reader["dogumSekli"] == DBNull.Value ? null : reader["dogumSekli"].ToString(),
                DogumKomplikasyonu = reader["dogumKomplikasyonu"] == DBNull.Value ? null : reader["dogumKomplikasyonu"].ToString(),
                DogumAgirligi = reader["dogumAgirligi"] == DBNull.Value ? null : (short?)reader["dogumAgirligi"],
                PlanliGebelikMi = reader["planliGebelikMi"] == DBNull.Value ? null : (bool?)reader["planliGebelikMi"],
                GebeKalmadaGucluk = reader["gebeKalmadaGucluk"] == DBNull.Value ? null : reader["gebeKalmadaGucluk"].ToString(),
                AileCinsiyetBeklentisi = reader["aileCinsiyetBeklentisi"] == DBNull.Value ? null : reader["aileCinsiyetBeklentisi"].ToString(),
                AileDogumaTepki = reader["aileDogumaTepki"] == DBNull.Value ? null : reader["aileDogumaTepki"].ToString(),
                OturmaYasi = reader["oturmaYasi"] == DBNull.Value ? null : (byte?)reader["oturmaYasi"],
                EmeklemeYasi = reader["emeklemeYasi"] == DBNull.Value ? null : (byte?)reader["emeklemeYasi"],
                YurumeYasi = reader["yurumeYasi"] == DBNull.Value ? null : (byte?)reader["yurumeYasi"],
                IlkSozcukYasi = reader["ilkSozcukYasi"] == DBNull.Value ? null : (byte?)reader["ilkSozcukYasi"],
                IlkCumleYasi = reader["ilkCumleYasi"] == DBNull.Value ? null : (byte?)reader["ilkCumleYasi"],
                TuvaletEgitimi = reader["tuvaletEgitimi"] == DBNull.Value ? null : reader["tuvaletEgitimi"].ToString(),
                GecirilenKaza = reader["gecirilenKaza"] == DBNull.Value ? null : reader["gecirilenKaza"].ToString(),
                BebeklikDonemi = reader["bebeklikDonemi"] == DBNull.Value ? null : reader["bebeklikDonemi"].ToString(),
                CocuklukDonemi = reader["cocuklukDonemi"] == DBNull.Value ? null : reader["cocuklukDonemi"].ToString(),
                OkulOykusu = reader["okulOykusu"] == DBNull.Value ? null : reader["okulOykusu"].ToString(),
                SosyalIliskileri = reader["sosyalIliskileri"] == DBNull.Value ? null : reader["sosyalIliskileri"].ToString(),
                KisilikOzellikleri = reader["kisilikOzellikleri"] == DBNull.Value ? null : reader["kisilikOzellikleri"].ToString(),
                GelisimselOykuEkNot = reader["gelisimselOykuEkNot"] == DBNull.Value ? null : reader["gelisimselOykuEkNot"].ToString(),
                SonGuncelleyenKullaniciID = reader["sonGuncelleyenKullaniciID"] == DBNull.Value ? null : (int?)reader["sonGuncelleyenKullaniciID"],
                SonGuncellemeTarihi = reader["sonGuncellemeTarihi"] == DBNull.Value ? null : (DateTime?)reader["sonGuncellemeTarihi"],
                OlusturanKullaniciID = reader["olusturanKullaniciID"] == DBNull.Value ? null : (int?)reader["olusturanKullaniciID"],
                OlusturmaTarihi = reader["olusturmaTarihi"] == DBNull.Value ? null : (DateTime?)reader["olusturmaTarihi"]
            };
        }
    }
}