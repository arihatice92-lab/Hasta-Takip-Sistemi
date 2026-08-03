using System;
using System.Collections.Generic;
using System.Data;
using HastaTakip.Entities;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class HastaTedaviDal
    {
        private readonly DbHelper _dbHelper;
        public HastaTedaviDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public void HastaTedaviEkle(HastaTedavi tedavi)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_HastaTedaviEkle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTC", tedavi.HastaTC);
            command.Parameters.AddWithValue("@doktorID", tedavi.DoktorID);
            command.Parameters.AddWithValue("@ilacID", tedavi.IlacID);
            command.Parameters.AddWithValue("@ilacDozu", tedavi.IlacDozu);
            command.Parameters.AddWithValue("@ilacBaslangicTarihi",
                tedavi.IlacBaslangicTarihi.HasValue ? (object)tedavi.IlacBaslangicTarihi.Value : DBNull.Value);
            command.Parameters.AddWithValue("@ilacBitisTarihi",
                tedavi.IlacBitisTarihi.HasValue ? (object)tedavi.IlacBitisTarihi.Value : DBNull.Value);
            command.Parameters.AddWithValue("@ilacYanEtkiler",
                string.IsNullOrWhiteSpace(tedavi.IlacYanEtkiler) ? DBNull.Value : (object)tedavi.IlacYanEtkiler);
            command.Parameters.AddWithValue("@tedaviNotlari",
                string.IsNullOrWhiteSpace(tedavi.TedaviNotlari) ? DBNull.Value : (object)tedavi.TedaviNotlari);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<HastaTedavi> HastaTedavileriListele(string hastaTC)
        {
            var liste = new List<HastaTedavi>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_HastaTedavileriListeleDetay", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTC", hastaTC);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(new HastaTedavi
                {
                    TedaviID = (int)reader["tedaviID"],
                    HastaTC = reader["hastaTC"].ToString()!,
                    DoktorID = (short)reader["doktorID"],
                    IlacID = (short)reader["ilacID"],
                    IlacDozu = reader["ilacDozu"].ToString()!,
                    IlacBaslangicTarihi = reader["ilacBaslangicTarihi"] == DBNull.Value
                        ? null : (DateTime?)reader["ilacBaslangicTarihi"],
                    IlacBitisTarihi = reader["ilacBitisTarihi"] == DBNull.Value
                        ? null : (DateTime?)reader["ilacBitisTarihi"],
                    IlacYanEtkiler = reader["ilacYanEtkiler"] == DBNull.Value
                        ? null : reader["ilacYanEtkiler"].ToString(),
                    TedaviNotlari = reader["tedaviNotlari"] == DBNull.Value
                        ? null : reader["tedaviNotlari"].ToString()
                });
            }
            return liste;
        }

        public HastaTedavi? HastaTedaviGetir(int tedaviID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_HastaTedaviGetir", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@tedaviID", tedaviID);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new HastaTedavi
                {
                    TedaviID = (int)reader["tedaviID"],
                    HastaTC = reader["hastaTC"].ToString()!,
                    DoktorID = (short)reader["doktorID"],
                    IlacID = (short)reader["ilacID"],
                    IlacDozu = reader["ilacDozu"].ToString()!,
                    IlacBaslangicTarihi = reader["ilacBaslangicTarihi"] == DBNull.Value
                        ? null : (DateTime?)reader["ilacBaslangicTarihi"],
                    IlacBitisTarihi = reader["ilacBitisTarihi"] == DBNull.Value
                        ? null : (DateTime?)reader["ilacBitisTarihi"],
                    IlacYanEtkiler = reader["ilacYanEtkiler"] == DBNull.Value
                        ? null : reader["ilacYanEtkiler"].ToString(),
                    TedaviNotlari = reader["tedaviNotlari"] == DBNull.Value
                        ? null : reader["tedaviNotlari"].ToString()
                };
            }
            return null;
        }

        public void HastaTedaviGuncelle(HastaTedavi tedavi)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_HastaTedaviGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@tedaviID", tedavi.TedaviID);
            command.Parameters.AddWithValue("@doktorID", tedavi.DoktorID);
            command.Parameters.AddWithValue("@ilacID", tedavi.IlacID);
            command.Parameters.AddWithValue("@ilacDozu", tedavi.IlacDozu);
            command.Parameters.AddWithValue("@ilacBaslangicTarihi",
                tedavi.IlacBaslangicTarihi.HasValue ? (object)tedavi.IlacBaslangicTarihi.Value : DBNull.Value);
            command.Parameters.AddWithValue("@ilacBitisTarihi",
                tedavi.IlacBitisTarihi.HasValue ? (object)tedavi.IlacBitisTarihi.Value : DBNull.Value);
            command.Parameters.AddWithValue("@ilacYanEtkiler",
                string.IsNullOrWhiteSpace(tedavi.IlacYanEtkiler) ? DBNull.Value : (object)tedavi.IlacYanEtkiler);
            command.Parameters.AddWithValue("@tedaviNotlari",
                string.IsNullOrWhiteSpace(tedavi.TedaviNotlari) ? DBNull.Value : (object)tedavi.TedaviNotlari);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}