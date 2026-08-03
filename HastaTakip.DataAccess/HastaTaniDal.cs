using System;
using System.Collections.Generic;
using System.Data;
using HastaTakip.Entities;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class HastaTaniDal
    {
        private readonly DbHelper _dbHelper;
        public HastaTaniDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public void HastaTaniEkle(HastaTani hastaTani)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_HastaTaniEkle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTC", hastaTani.HastaTC);
            command.Parameters.AddWithValue("@doktorID", hastaTani.DoktorID);
            command.Parameters.AddWithValue("@taniID", hastaTani.TaniID);
            command.Parameters.AddWithValue("@mentalDurumMuayenesi",
                string.IsNullOrWhiteSpace(hastaTani.MentalDurumMuayenesi) ? DBNull.Value : (object)hastaTani.MentalDurumMuayenesi);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<HastaTani> HastaTanilariListele(string hastaTC)
        {
            var liste = new List<HastaTani>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_HastaTanilariListeleDetay", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTC", hastaTC);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(new HastaTani
                {
                    HastaTaniID = (int)reader["hastaTaniID"],
                    HastaTC = reader["hastaTC"].ToString()!,
                    DoktorID = (short)reader["doktorID"],
                    TaniID = (short)reader["taniID"],
                    TaniTarih = (DateTime)reader["taniTarih"],
                    MentalDurumMuayenesi = reader["mentalDurumMuayenesi"] == DBNull.Value
                        ? null : reader["mentalDurumMuayenesi"].ToString()
                });
            }
            return liste;
        }

        public HastaTani? HastaTaniGetir(int hastaTaniID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_HastaTaniGetir", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTaniID", hastaTaniID);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new HastaTani
                {
                    HastaTaniID = (int)reader["hastaTaniID"],
                    HastaTC = reader["hastaTC"].ToString()!,
                    DoktorID = (short)reader["doktorID"],
                    TaniID = (short)reader["taniID"],
                    TaniTarih = (DateTime)reader["taniTarih"],
                    MentalDurumMuayenesi = reader["mentalDurumMuayenesi"] == DBNull.Value
                        ? null : reader["mentalDurumMuayenesi"].ToString()
                };
            }
            return null;
        }

        public void HastaTaniGuncelle(HastaTani hastaTani)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_HastaTaniGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTaniID", hastaTani.HastaTaniID);
            command.Parameters.AddWithValue("@doktorID", hastaTani.DoktorID);
            command.Parameters.AddWithValue("@taniID", hastaTani.TaniID);
            command.Parameters.AddWithValue("@mentalDurumMuayenesi",
                string.IsNullOrWhiteSpace(hastaTani.MentalDurumMuayenesi) ? DBNull.Value : (object)hastaTani.MentalDurumMuayenesi);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}