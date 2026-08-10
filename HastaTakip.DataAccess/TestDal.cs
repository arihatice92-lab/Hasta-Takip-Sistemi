using HastaTakip.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace HastaTakip.DataAccess
{
    // TestDal.cs
    public class TestDal
    {
        private readonly DbHelper _dbHelper;
        public TestDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public List<Test> TestleriListele()
        {
            var testler = new List<Test>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TestleriListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                testler.Add(new Test
                {
                    TestID = (byte)reader["testID"],
                    TestAdi = reader["testAdi"].ToString()!,
                    TestBilgi = reader["testBilgi"] == DBNull.Value ? null : reader["testBilgi"].ToString()
                });
            }
            return testler;
        }

        public void TestEkle(Test test)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TestEkle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@testAdi", test.TestAdi);
            command.Parameters.AddWithValue("@testBilgi",
                string.IsNullOrWhiteSpace(test.TestBilgi) ? DBNull.Value : (object)test.TestBilgi);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public Test? TestGetir(byte testID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TestGetir", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@testID", testID);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Test
                {
                    TestID = (byte)reader["testID"],
                    TestAdi = reader["testAdi"].ToString()!,
                    TestBilgi = reader["testBilgi"] == DBNull.Value ? null : reader["testBilgi"].ToString()
                };
            }
            return null;
        }

        public void TestGuncelle(Test test)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TestGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@testID", test.TestID);
            command.Parameters.AddWithValue("@testAdi", test.TestAdi);
            command.Parameters.AddWithValue("@testBilgi",
                string.IsNullOrWhiteSpace(test.TestBilgi) ? DBNull.Value : (object)test.TestBilgi);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void TestSil(byte testID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TestSil", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@testID", testID);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
