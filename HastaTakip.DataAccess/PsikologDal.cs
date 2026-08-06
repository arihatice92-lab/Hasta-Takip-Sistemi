using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    
    public class PsikologDal
    {
        private readonly DbHelper _dbHelper;
        public PsikologDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public List<Psikolog> PsikologlariListele()
        {
            var psikologlar = new List<Psikolog>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_PsikologlariListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                psikologlar.Add(new Psikolog
                {
                    PsikologID = (byte)reader["psikologID"],
                    PsikologSicilNo = reader["psikologSicilNo"] == DBNull.Value ? null : reader["psikologSicilNo"].ToString(),
                    PsikologAd = reader["psikologAd"].ToString()!,
                    PsikologSoyad = reader["psikologSoyad"].ToString()!,
                    PsikologTel = reader["psikologTel"].ToString()!
                });
            }
            return psikologlar;
        }
        public void PsikologEkle(Psikolog psikolog)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_PsikologKaydet", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@psikologSicilNo",
                string.IsNullOrWhiteSpace(psikolog.PsikologSicilNo) ? DBNull.Value : (object)psikolog.PsikologSicilNo);
            command.Parameters.AddWithValue("@psikologAd", psikolog.PsikologAd);
            command.Parameters.AddWithValue("@psikologSoyad", psikolog.PsikologSoyad);
            command.Parameters.AddWithValue("@psikologTel", psikolog.PsikologTel);
            command.Parameters.AddWithValue("@psikologKurumBaslangicTarih",
                psikolog.PsikologKurumBaslangicTarih.HasValue ? (object)psikolog.PsikologKurumBaslangicTarih.Value : DBNull.Value);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public Psikolog? PsikologGetir(byte psikologID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_PsikologGetir", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@psikologID", psikologID);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapToPsikolog(reader);
            }
            return null;
        }

        public void PsikologGuncelle(Psikolog psikolog)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_PsikologGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@psikologSicilNo",
                string.IsNullOrWhiteSpace(psikolog.PsikologSicilNo) ? DBNull.Value : (object)psikolog.PsikologSicilNo);
            command.Parameters.AddWithValue("@psikologID", psikolog.PsikologID);
            command.Parameters.AddWithValue("@psikologAd", psikolog.PsikologAd);
            command.Parameters.AddWithValue("@psikologSoyad", psikolog.PsikologSoyad);
            command.Parameters.AddWithValue("@psikologTel", psikolog.PsikologTel);
            command.Parameters.AddWithValue("@psikologKurumBaslangicTarih",
                psikolog.PsikologKurumBaslangicTarih.HasValue ? (object)psikolog.PsikologKurumBaslangicTarih.Value : DBNull.Value);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void PsikologAyrilis(byte psikologID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_PsikologAyrilis", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@psikologID", psikologID);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<Psikolog> PsikologAra(string? ara, string siralama, string aktif)
        {
            var liste = new List<Psikolog>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_PsikologAra", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Ara",
                string.IsNullOrWhiteSpace(ara) ? DBNull.Value : (object)ara);
            command.Parameters.AddWithValue("@Siralama", siralama);
            command.Parameters.AddWithValue("@Aktif", aktif);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(MapToPsikolog(reader));
            }
            return liste;
        }

        private Psikolog MapToPsikolog(SqlDataReader reader)
        {
            return new Psikolog
            {
                PsikologID = (byte)reader["psikologID"],
                PsikologSicilNo = reader["psikologSicilNo"] == DBNull.Value ? null : reader["psikologSicilNo"].ToString(),
                PsikologAd = reader["psikologAd"].ToString()!,
                PsikologSoyad = reader["psikologSoyad"].ToString()!,
                PsikologTel = reader["psikologTel"].ToString()!,
                PsikologKurumBaslangicTarih = reader["psikologKurumBaslangicTarih"] == DBNull.Value
                    ? null : (DateTime?)reader["psikologKurumBaslangicTarih"],
                PsikologKurumAyrilisTarih = reader["psikologKurumAyrilisTarih"] == DBNull.Value
                    ? null : (DateTime?)reader["psikologKurumAyrilisTarih"]
            };
        }
    }


}
