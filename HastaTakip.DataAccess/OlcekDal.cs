using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    // OlcekDal.cs
    public class OlcekDal
    {
        private readonly DbHelper _dbHelper;
        public OlcekDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public List<Olcek> OlcekleriListele()
        {
            var olcekler = new List<Olcek>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_OlcekleriListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                olcekler.Add(new Olcek
                {
                    OlcekID = (byte)reader["olcekID"],
                    OlcekAdi = reader["olcekAdi"].ToString()!,
                    OlcekBilgi = reader["olcekBilgi"] == DBNull.Value ? null : reader["olcekBilgi"].ToString()
                });
            }
            return olcekler;
        }

        public void OlcekEkle(Olcek olcek)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_OlcekEkle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@olcekAdi", olcek.OlcekAdi);
            command.Parameters.AddWithValue("@olcekBilgi",
                string.IsNullOrWhiteSpace(olcek.OlcekBilgi) ? DBNull.Value : (object)olcek.OlcekBilgi);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public Olcek? OlcekGetir(byte olcekID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_OlcekGetir", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@olcekID", olcekID);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Olcek
                {
                    OlcekID = (byte)reader["olcekID"],
                    OlcekAdi = reader["olcekAdi"].ToString()!,
                    OlcekBilgi = reader["olcekBilgi"] == DBNull.Value ? null : reader["olcekBilgi"].ToString()
                };
            }
            return null;
        }

        public void OlcekGuncelle(Olcek olcek)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_OlcekGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@olcekID", olcek.OlcekID);
            command.Parameters.AddWithValue("@olcekAdi", olcek.OlcekAdi);
            command.Parameters.AddWithValue("@olcekBilgi",
                string.IsNullOrWhiteSpace(olcek.OlcekBilgi) ? DBNull.Value : (object)olcek.OlcekBilgi);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void OlcekSil(byte olcekID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_OlcekSil", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@olcekID", olcekID);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
