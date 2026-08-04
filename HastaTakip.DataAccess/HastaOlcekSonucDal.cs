using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class HastaOlcekSonucDal
    {
        private readonly DbHelper _dbHelper;
        public HastaOlcekSonucDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public void OlcekSonucEkle(OlcekSonuc olcekSonuc)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_OlcekSonucEkle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTC", olcekSonuc.HastaTC);
            command.Parameters.AddWithValue("@doktorID", olcekSonuc.DoktorID);
            command.Parameters.AddWithValue("@olcekID", olcekSonuc.OlcekID);
            command.Parameters.AddWithValue("@olcekTarih", olcekSonuc.OlcekTarih);
            command.Parameters.AddWithValue("@olcekPuan",
                olcekSonuc.OlcekPuan.HasValue ? (object)olcekSonuc.OlcekPuan.Value : DBNull.Value);
            command.Parameters.AddWithValue("@olcekYorum",
                string.IsNullOrWhiteSpace(olcekSonuc.OlcekYorum) ? DBNull.Value : (object)olcekSonuc.OlcekYorum);
            command.Parameters.AddWithValue("@olcekUygulanan",
                string.IsNullOrWhiteSpace(olcekSonuc.OlcekUygulanan) ? DBNull.Value : (object)olcekSonuc.OlcekUygulanan);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public OlcekSonuc? OlcekSonucGetir(int olcekSonucID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_OlcekSonucGetir", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@olcekSonucID", olcekSonucID);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapToOlcekSonuc(reader);
            }
            return null;
        }

        public void OlcekSonucGuncelle(OlcekSonuc olcekSonuc)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_OlcekSonucGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@olcekSonucID", olcekSonuc.OlcekSonucID);
            command.Parameters.AddWithValue("@doktorID", olcekSonuc.DoktorID);
            command.Parameters.AddWithValue("@olcekID", olcekSonuc.OlcekID);
            command.Parameters.AddWithValue("@olcekTarih", olcekSonuc.OlcekTarih);
            command.Parameters.AddWithValue("@olcekPuan",
                olcekSonuc.OlcekPuan.HasValue ? (object)olcekSonuc.OlcekPuan.Value : DBNull.Value);
            command.Parameters.AddWithValue("@olcekYorum",
                string.IsNullOrWhiteSpace(olcekSonuc.OlcekYorum) ? DBNull.Value : (object)olcekSonuc.OlcekYorum);
            command.Parameters.AddWithValue("@olcekUygulanan",
                string.IsNullOrWhiteSpace(olcekSonuc.OlcekUygulanan) ? DBNull.Value : (object)olcekSonuc.OlcekUygulanan);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<OlcekSonuc> HastaOlcekSonuclariListele(string hastaTC)
        {
            var liste = new List<OlcekSonuc>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_HastaOlcekSonuclariListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTC", hastaTC);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(MapToOlcekSonuc(reader));
            }
            return liste;
        }

        private OlcekSonuc MapToOlcekSonuc(SqlDataReader reader)
        {
            return new OlcekSonuc
            {
                OlcekSonucID = (int)reader["olcekSonucID"],
                HastaTC = reader["hastaTC"].ToString()!,
                DoktorID = (short)reader["doktorID"],
                OlcekID = (byte)reader["olcekID"],
                OlcekTarih = (DateTime)reader["olcekTarih"],
                OlcekPuan = reader["olcekPuan"] == DBNull.Value ? null : (byte?)reader["olcekPuan"],
                OlcekYorum = reader["olcekYorum"] == DBNull.Value ? null : reader["olcekYorum"].ToString(),
                OlcekUygulanan = reader["olcekUygulanan"] == DBNull.Value ? null : reader["olcekUygulanan"].ToString()
            };
        }
    }
}
