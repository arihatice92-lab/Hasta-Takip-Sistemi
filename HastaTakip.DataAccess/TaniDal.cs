using System.Collections.Generic;
using System.Data;
using HastaTakip.Entities;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class TaniDal
    {
        private readonly DbHelper _dbHelper;
        public TaniDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public List<Tani> TanilariListele()
        {
            var tanilar = new List<Tani>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TanilariListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                tanilar.Add(new Tani
                {
                    TaniID = (short)reader["taniID"],
                    TaniAdi = reader["taniAdi"].ToString()!,
                    TaniKodu = reader["taniKodu"].ToString()!
                });
            }
            return tanilar;
        }
        public void TaniEkle(Tani tani)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TaniEkle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@taniAdi", tani.TaniAdi);
            command.Parameters.AddWithValue("@taniKodu",
                string.IsNullOrWhiteSpace(tani.TaniKodu) ? DBNull.Value : (object)tani.TaniKodu);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public Tani? TaniGetir(short taniID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TaniGetir", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@taniID", taniID);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Tani
                {
                    TaniID = (short)reader["taniID"],
                    TaniAdi = reader["taniAdi"].ToString()!,
                    TaniKodu = reader["taniKodu"] == DBNull.Value ? null : reader["taniKodu"].ToString()
                };
            }
            return null;
        }

        public void TaniGuncelle(Tani tani)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TaniGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@taniID", tani.TaniID);
            command.Parameters.AddWithValue("@taniAdi", tani.TaniAdi);
            command.Parameters.AddWithValue("@taniKodu",
                string.IsNullOrWhiteSpace(tani.TaniKodu) ? DBNull.Value : (object)tani.TaniKodu);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void TaniSil(short taniID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TaniSil", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@taniID", taniID);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}