using System;
using System.Collections.Generic;
using System.Data;
using HastaTakip.Entities;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class RandevuNotuDal
    {
        private readonly DbHelper _dbHelper;
        public RandevuNotuDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public void RandevuNotuEkle(RandevuNotu notu)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_RandevuNotuEkle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTC", notu.HastaTC);
            command.Parameters.AddWithValue("@doktorID", notu.DoktorID);
            command.Parameters.AddWithValue("@randevuTarihID", notu.RandevuTarihID);
            command.Parameters.AddWithValue("@gorusmeTipi",
                string.IsNullOrWhiteSpace(notu.GorusmeTipi) ? DBNull.Value : (object)notu.GorusmeTipi);
            command.Parameters.AddWithValue("@gorusmeNotu",
                string.IsNullOrWhiteSpace(notu.GorusmeNotu) ? DBNull.Value : (object)notu.GorusmeNotu);
            command.Parameters.AddWithValue("@sonrakiRandevuTarihi",
                notu.SonrakiRandevuTarihi.HasValue ? (object)notu.SonrakiRandevuTarihi.Value : DBNull.Value);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public RandevuNotu? RandevuNotuGetir(short randevuNotID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_RandevuNotuGetir", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@randevuNotID", randevuNotID);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapToRandevuNotu(reader);
            }
            return null;
        }

        public RandevuNotu? RandevuNotuGetirByRandevuTarihID(int randevuTarihID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_RandevuNotuGetirByRandevuTarihID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@randevuTarihID", randevuTarihID);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapToRandevuNotu(reader);
            }
            return null;
        }

        public void RandevuNotuGuncelle(RandevuNotu notu)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_RandevuNotuGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@randevuNotID", notu.RandevuNotID);
            command.Parameters.AddWithValue("@gorusmeTipi",
                string.IsNullOrWhiteSpace(notu.GorusmeTipi) ? DBNull.Value : (object)notu.GorusmeTipi);
            command.Parameters.AddWithValue("@gorusmeNotu",
                string.IsNullOrWhiteSpace(notu.GorusmeNotu) ? DBNull.Value : (object)notu.GorusmeNotu);
            command.Parameters.AddWithValue("@sonrakiRandevuTarihi",
                notu.SonrakiRandevuTarihi.HasValue ? (object)notu.SonrakiRandevuTarihi.Value : DBNull.Value);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<RandevuNotu> HastaRandevuNotlariListele(string hastaTC)
        {
            var liste = new List<RandevuNotu>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_HastaRandevuNotlariListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTC", hastaTC);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(MapToRandevuNotu(reader));
            }
            return liste;
        }

        private RandevuNotu MapToRandevuNotu(SqlDataReader reader)
        {
            return new RandevuNotu
            {
                RandevuNotID = (short)reader["randevuNotID"],
                HastaTC = reader["hastaTC"].ToString()!,
                DoktorID = (short)reader["doktorID"],
                RandevuTarihID = (int)reader["randevuTarihID"],
                GorusmeTipi = reader["gorusmeTipi"] == DBNull.Value ? null : reader["gorusmeTipi"].ToString(),
                GorusmeNotu = reader["gorusmeNotu"] == DBNull.Value ? null : reader["gorusmeNotu"].ToString(),
                SonrakiRandevuTarihi = reader["sonrakiRandevuTarihi"] == DBNull.Value ? null : (DateTime?)reader["sonrakiRandevuTarihi"]
            };
        }
    }
}