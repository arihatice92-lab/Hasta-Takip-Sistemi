using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class AltKumeSonucDal
    {
        private readonly DbHelper _dbHelper;
        public AltKumeSonucDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public void AltKumeSonucEkle(AltKumeSonuc sonuc)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_AltKumeSonucEkle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTC", sonuc.HastaTC);
            command.Parameters.AddWithValue("@altKumeID", sonuc.AltKumeID);
            command.Parameters.AddWithValue("@testSonucID",
                sonuc.TestSonucID.HasValue ? (object)sonuc.TestSonucID.Value : DBNull.Value);
            command.Parameters.AddWithValue("@altKumeSonuc",
                string.IsNullOrWhiteSpace(sonuc.AltKumeSonucDeger) ? DBNull.Value : (object)sonuc.AltKumeSonucDeger);
            command.Parameters.AddWithValue("@altKumeYorum",
                string.IsNullOrWhiteSpace(sonuc.AltKumeYorum) ? DBNull.Value : (object)sonuc.AltKumeYorum);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<AltKumeSonuc> AltKumeSonuclariListele(int testSonucID)
        {
            var liste = new List<AltKumeSonuc>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_AltKumeSonuclariListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@testSonucID", testSonucID);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(new AltKumeSonuc
                {
                    AltKumeSonucID = (int)reader["altKumeSonucID"],
                    HastaTC = reader["hastaTC"].ToString()!,
                    AltKumeID = (byte)reader["altKumeID"],
                    TestSonucID = reader["testSonucID"] == DBNull.Value ? null : (int?)reader["testSonucID"],
                    AltKumeSonucDeger = reader["altKumeSonuc"] == DBNull.Value ? null : reader["altKumeSonuc"].ToString(),
                    AltKumeYorum = reader["altKumeYorum"] == DBNull.Value ? null : reader["altKumeYorum"].ToString()
                });
            }
            return liste;
        }

        public void AltKumeSonuclariSil(int testSonucID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_AltKumeSonuclariSil", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@testSonucID", testSonucID);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
