using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class TestAltKumeDal
    {
        private readonly DbHelper _dbHelper;
        public TestAltKumeDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public List<TestAltKume> TumAltKumeleriListele()
        {
            var liste = new List<TestAltKume>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TestAltKumeleriListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(new TestAltKume
                {
                    TestAltKumeID = (byte)reader["testAltKumeID"],
                    TestAltKumeAdi = reader["testAltKumeAdi"].ToString()!,
                    TestAltKumeAciklama = reader["testAltKumeAciklama"] == DBNull.Value ? null : reader["testAltKumeAciklama"].ToString(),
                    TestID = reader["testID"] == DBNull.Value ? null : (byte?)reader["testID"]
                });
            }
            return liste;
        }

        public void AltKumeEkle(TestAltKume altKume)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TestAltKumeEkle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@testAltKumeAdi", altKume.TestAltKumeAdi);
            command.Parameters.AddWithValue("@testAltKumeAciklama",
                string.IsNullOrWhiteSpace(altKume.TestAltKumeAciklama) ? DBNull.Value : (object)altKume.TestAltKumeAciklama);
            command.Parameters.AddWithValue("@testID", altKume.TestID!.Value);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void AltKumeSil(byte testAltKumeID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TestAltKumeSil", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@testAltKumeID", testAltKumeID);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<TestAltKume> AltKumeleriListeleByTestID(byte testID)
        {
            var liste = new List<TestAltKume>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TestAltKumeleriListeleByTestID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@testID", testID);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(new TestAltKume
                {
                    TestAltKumeID = (byte)reader["testAltKumeID"],
                    TestAltKumeAdi = reader["testAltKumeAdi"].ToString()!,
                    TestAltKumeAciklama = reader["testAltKumeAciklama"] == DBNull.Value ? null : reader["testAltKumeAciklama"].ToString(),
                    TestID = reader["testID"] == DBNull.Value ? null : (byte?)reader["testID"]
                });
            }
            return liste;
        }
    }
}
