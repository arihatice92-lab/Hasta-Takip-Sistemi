using System;
using System.Collections.Generic;
using System.Data;
using HastaTakip.Entities;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class AileBilgileriDal
    {
        private readonly DbHelper _dbHelper;
        public AileBilgileriDal(DbHelper dbHelper) { _dbHelper = dbHelper; }


        public AileBilgileri? AileBilgileriGetir(int aileBilgileriID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_AileBilgileriGetir", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@aileBilgileriID", aileBilgileriID);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapToAileBilgileri(reader);
            }
            return null;
        }

        public void AileBilgileriEkle(AileBilgileri bilgi, int kullaniciID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_AileBilgileriEkle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTC", bilgi.HastaTC);   // ← sadece Ekle'de
            OrtakParametreler(command, bilgi);
            command.Parameters.AddWithValue("@kullaniciID", kullaniciID);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void AileBilgileriGuncelle(AileBilgileri bilgi, int kullaniciID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_AileBilgileriGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@aileBilgileriID", bilgi.AileBilgileriID);
            OrtakParametreler(command, bilgi);   // ← @hastaTC yok
            command.Parameters.AddWithValue("@kullaniciID", kullaniciID);
            connection.Open();
            command.ExecuteNonQuery();
        }

        private void OrtakParametreler(SqlCommand command, AileBilgileri b)
        {
            command.Parameters.AddWithValue("@anneYasiyorMu", b.AnneYasiyorMu);
            command.Parameters.AddWithValue("@anneAd", (object?)b.AnneAd ?? DBNull.Value);
            command.Parameters.AddWithValue("@anneSoyad", (object?)b.AnneSoyad ?? DBNull.Value);
            command.Parameters.AddWithValue("@anneYas", (object?)b.AnneYas ?? DBNull.Value);
            command.Parameters.AddWithValue("@anneEgitim", (object?)b.AnneEgitim ?? DBNull.Value);
            command.Parameters.AddWithValue("@anneIs", (object?)b.AnneIs ?? DBNull.Value);
            command.Parameters.AddWithValue("@anneTel", (object?)b.AnneTel ?? DBNull.Value);
            command.Parameters.AddWithValue("@anneAdres", (object?)b.AnneAdres ?? DBNull.Value);
            command.Parameters.AddWithValue("@babaYasiyorMu", b.BabaYasiyorMu);
            command.Parameters.AddWithValue("@babaAd", (object?)b.BabaAd ?? DBNull.Value);
            command.Parameters.AddWithValue("@babaSoyad", (object?)b.BabaSoyad ?? DBNull.Value);
            command.Parameters.AddWithValue("@babaYas", (object?)b.BabaYas ?? DBNull.Value);
            command.Parameters.AddWithValue("@babaEgitim", (object?)b.BabaEgitim ?? DBNull.Value);
            command.Parameters.AddWithValue("@babaIs", (object?)b.BabaIs ?? DBNull.Value);
            command.Parameters.AddWithValue("@babaTel", (object?)b.BabaTel ?? DBNull.Value);
            command.Parameters.AddWithValue("@babaAdres", (object?)b.BabaAdres ?? DBNull.Value);
            command.Parameters.AddWithValue("@uveyVeyaKoruyucuVarMi", b.UveyVeyaKoruyucuVarMi);
            command.Parameters.AddWithValue("@uveyEbeveynTuru", (object?)b.UveyEbeveynTuru ?? DBNull.Value);
            command.Parameters.AddWithValue("@uveyAd", (object?)b.UveyAd ?? DBNull.Value);
            command.Parameters.AddWithValue("@uveySoyad", (object?)b.UveySoyad ?? DBNull.Value);
            command.Parameters.AddWithValue("@uveyYas", (object?)b.UveyYas ?? DBNull.Value);
            command.Parameters.AddWithValue("@uveyEgitim", (object?)b.UveyEgitim ?? DBNull.Value);
            command.Parameters.AddWithValue("@uveyIs", (object?)b.UveyIs ?? DBNull.Value);
            command.Parameters.AddWithValue("@uveyTel", (object?)b.UveyTel ?? DBNull.Value);
            command.Parameters.AddWithValue("@uveyAdres", (object?)b.UveyAdres ?? DBNull.Value);
            command.Parameters.AddWithValue("@uveyYasiyorMu", (object?)b.UveyYasiyorMu ?? DBNull.Value);
            command.Parameters.AddWithValue("@akrabaEvliligi", b.AkrabaEvliligi);
            command.Parameters.AddWithValue("@aileTipi", (object?)b.AileTipi ?? DBNull.Value);
            command.Parameters.AddWithValue("@ebeveynDurumu", (object?)b.EbeveynDurumu ?? DBNull.Value);
            command.Parameters.AddWithValue("@kardesler", (object?)b.Kardesler ?? DBNull.Value);
            command.Parameters.AddWithValue("@ailePsikiyatrikOyku", (object?)b.AilePsikiyatrikOyku ?? DBNull.Value);
            command.Parameters.AddWithValue("@aileTibbiOyku", (object?)b.AileTibbiOyku ?? DBNull.Value);
            command.Parameters.AddWithValue("@aileEkNotlar", (object?)b.AileEkNotlar ?? DBNull.Value);
        }

        public List<AileBilgileri> HastaAileBilgileriListele(string hastaTC)
        {
            var liste = new List<AileBilgileri>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_HastaAileBilgileriListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTC", hastaTC);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(MapToAileBilgileri(reader));
            }
            return liste;
        }

        private void EkleParametreler(SqlCommand command, AileBilgileri b)
        {
            command.Parameters.AddWithValue("@hastaTC", b.HastaTC);
            command.Parameters.AddWithValue("@anneYasiyorMu", b.AnneYasiyorMu);
            command.Parameters.AddWithValue("@anneAd", (object?)b.AnneAd ?? DBNull.Value);
            command.Parameters.AddWithValue("@anneSoyad", (object?)b.AnneSoyad ?? DBNull.Value);
            command.Parameters.AddWithValue("@anneYas", (object?)b.AnneYas ?? DBNull.Value);
            command.Parameters.AddWithValue("@anneEgitim", (object?)b.AnneEgitim ?? DBNull.Value);
            command.Parameters.AddWithValue("@anneIs", (object?)b.AnneIs ?? DBNull.Value);
            command.Parameters.AddWithValue("@anneTel", (object?)b.AnneTel ?? DBNull.Value);
            command.Parameters.AddWithValue("@anneAdres", (object?)b.AnneAdres ?? DBNull.Value);
            command.Parameters.AddWithValue("@babaYasiyorMu", b.BabaYasiyorMu);
            command.Parameters.AddWithValue("@babaAd", (object?)b.BabaAd ?? DBNull.Value);
            command.Parameters.AddWithValue("@babaSoyad", (object?)b.BabaSoyad ?? DBNull.Value);
            command.Parameters.AddWithValue("@babaYas", (object?)b.BabaYas ?? DBNull.Value);
            command.Parameters.AddWithValue("@babaEgitim", (object?)b.BabaEgitim ?? DBNull.Value);
            command.Parameters.AddWithValue("@babaIs", (object?)b.BabaIs ?? DBNull.Value);
            command.Parameters.AddWithValue("@babaTel", (object?)b.BabaTel ?? DBNull.Value);
            command.Parameters.AddWithValue("@babaAdres", (object?)b.BabaAdres ?? DBNull.Value);
            command.Parameters.AddWithValue("@uveyVeyaKoruyucuVarMi", b.UveyVeyaKoruyucuVarMi);
            command.Parameters.AddWithValue("@uveyEbeveynTuru", (object?)b.UveyEbeveynTuru ?? DBNull.Value);
            command.Parameters.AddWithValue("@uveyAd", (object?)b.UveyAd ?? DBNull.Value);
            command.Parameters.AddWithValue("@uveySoyad", (object?)b.UveySoyad ?? DBNull.Value);
            command.Parameters.AddWithValue("@uveyYas", (object?)b.UveyYas ?? DBNull.Value);
            command.Parameters.AddWithValue("@uveyEgitim", (object?)b.UveyEgitim ?? DBNull.Value);
            command.Parameters.AddWithValue("@uveyIs", (object?)b.UveyIs ?? DBNull.Value);
            command.Parameters.AddWithValue("@uveyTel", (object?)b.UveyTel ?? DBNull.Value);
            command.Parameters.AddWithValue("@uveyAdres", (object?)b.UveyAdres ?? DBNull.Value);
            command.Parameters.AddWithValue("@uveyYasiyorMu", (object?)b.UveyYasiyorMu ?? DBNull.Value);
            command.Parameters.AddWithValue("@akrabaEvliligi", b.AkrabaEvliligi);
            command.Parameters.AddWithValue("@aileTipi", (object?)b.AileTipi ?? DBNull.Value);
            command.Parameters.AddWithValue("@ebeveynDurumu", (object?)b.EbeveynDurumu ?? DBNull.Value);
            command.Parameters.AddWithValue("@kardesler", (object?)b.Kardesler ?? DBNull.Value);
            command.Parameters.AddWithValue("@ailePsikiyatrikOyku", (object?)b.AilePsikiyatrikOyku ?? DBNull.Value);
            command.Parameters.AddWithValue("@aileTibbiOyku", (object?)b.AileTibbiOyku ?? DBNull.Value);
            command.Parameters.AddWithValue("@aileEkNotlar", (object?)b.AileEkNotlar ?? DBNull.Value);
        }

        private AileBilgileri MapToAileBilgileri(SqlDataReader reader)
        {
            return new AileBilgileri
            {
                AileBilgileriID = (int)reader["aileBilgileriID"],
                HastaTC = reader["hastaTC"].ToString()!,
                AnneYasiyorMu = (bool)reader["anneYasiyorMu"],
                AnneAd = reader["anneAd"] == DBNull.Value ? null : reader["anneAd"].ToString(),
                AnneSoyad = reader["anneSoyad"] == DBNull.Value ? null : reader["anneSoyad"].ToString(),
                AnneYas = reader["anneYas"] == DBNull.Value ? null : (byte?)reader["anneYas"],
                AnneEgitim = reader["anneEgitim"] == DBNull.Value ? null : reader["anneEgitim"].ToString(),
                AnneIs = reader["anneIs"] == DBNull.Value ? null : reader["anneIs"].ToString(),
                AnneTel = reader["anneTel"] == DBNull.Value ? null : reader["anneTel"].ToString(),
                AnneAdres = reader["anneAdres"] == DBNull.Value ? null : reader["anneAdres"].ToString(),
                BabaYasiyorMu = (bool)reader["babaYasiyorMu"],
                BabaAd = reader["babaAd"] == DBNull.Value ? null : reader["babaAd"].ToString(),
                BabaSoyad = reader["babaSoyad"] == DBNull.Value ? null : reader["babaSoyad"].ToString(),
                BabaYas = reader["babaYas"] == DBNull.Value ? null : (byte?)reader["babaYas"],
                BabaEgitim = reader["babaEgitim"] == DBNull.Value ? null : reader["babaEgitim"].ToString(),
                BabaIs = reader["babaIs"] == DBNull.Value ? null : reader["babaIs"].ToString(),
                BabaTel = reader["babaTel"] == DBNull.Value ? null : reader["babaTel"].ToString(),
                BabaAdres = reader["babaAdres"] == DBNull.Value ? null : reader["babaAdres"].ToString(),
                UveyVeyaKoruyucuVarMi = (bool)reader["uveyVeyaKoruyucuVarMi"],
                UveyEbeveynTuru = reader["uveyEbeveynTuru"] == DBNull.Value ? null : reader["uveyEbeveynTuru"].ToString(),
                UveyAd = reader["uveyAd"] == DBNull.Value ? null : reader["uveyAd"].ToString(),
                UveySoyad = reader["uveySoyad"] == DBNull.Value ? null : reader["uveySoyad"].ToString(),
                UveyYas = reader["uveyYas"] == DBNull.Value ? null : (byte?)reader["uveyYas"],
                UveyEgitim = reader["uveyEgitim"] == DBNull.Value ? null : reader["uveyEgitim"].ToString(),
                UveyIs = reader["uveyIs"] == DBNull.Value ? null : reader["uveyIs"].ToString(),
                UveyTel = reader["uveyTel"] == DBNull.Value ? null : reader["uveyTel"].ToString(),
                UveyAdres = reader["uveyAdres"] == DBNull.Value ? null : reader["uveyAdres"].ToString(),
                UveyYasiyorMu = reader["uveyYasiyorMu"] == DBNull.Value ? null : (bool?)reader["uveyYasiyorMu"],
                AkrabaEvliligi = (bool)reader["akrabaEvliligi"],
                AileTipi = reader["aileTipi"] == DBNull.Value ? null : reader["aileTipi"].ToString(),
                EbeveynDurumu = reader["ebeveynDurumu"] == DBNull.Value ? null : reader["ebeveynDurumu"].ToString(),
                Kardesler = reader["kardesler"] == DBNull.Value ? null : reader["kardesler"].ToString(),
                AilePsikiyatrikOyku = reader["ailePsikiyatrikOyku"] == DBNull.Value ? null : reader["ailePsikiyatrikOyku"].ToString(),
                AileTibbiOyku = reader["aileTibbiOyku"] == DBNull.Value ? null : reader["aileTibbiOyku"].ToString(),
                AileEkNotlar = reader["aileEkNotlar"] == DBNull.Value ? null : reader["aileEkNotlar"].ToString(),
                SonGuncelleyenKullaniciID = reader["sonGuncelleyenKullaniciID"] == DBNull.Value ? null : (int?)reader["sonGuncelleyenKullaniciID"],
                SonGuncellemeTarihi = reader["sonGuncellemeTarihi"] == DBNull.Value ? null : (DateTime?)reader["sonGuncellemeTarihi"]
            };
        }
    }
}