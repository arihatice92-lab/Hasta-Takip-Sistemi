using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class HastaTestSonucDal
    {
        private readonly DbHelper _dbHelper;
        public HastaTestSonucDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public int TestSonucEkle(TestSonuc testSonuc)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TestSonucEkle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTC", testSonuc.HastaTC);
            command.Parameters.AddWithValue("@psikologID", testSonuc.PsikologID);
            command.Parameters.AddWithValue("@testID", testSonuc.TestID);
            command.Parameters.AddWithValue("@testTarih", testSonuc.TestTarih);
            command.Parameters.AddWithValue("@testSonuc",
                string.IsNullOrWhiteSpace(testSonuc.SonucDegeri) ? DBNull.Value : (object)testSonuc.SonucDegeri);
            command.Parameters.AddWithValue("@testDegerlendirme",
                string.IsNullOrWhiteSpace(testSonuc.TestDegerlendirme) ? DBNull.Value : (object)testSonuc.TestDegerlendirme);

            var yeniIDParam = new SqlParameter("@YeniTestSonucID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command.Parameters.Add(yeniIDParam);

            connection.Open();
            command.ExecuteNonQuery();

            return (int)yeniIDParam.Value;
        }

        public TestSonuc? TestSonucGetir(int testSonucID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TestSonucGetir", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@testSonucID", testSonucID);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapToTestSonuc(reader);
            }
            return null;
        }

        public void TestSonucGuncelle(TestSonuc testSonuc)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TestSonucGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@testSonucID", testSonuc.TestSonucID);
            command.Parameters.AddWithValue("@psikologID", testSonuc.PsikologID);
            command.Parameters.AddWithValue("@testID", testSonuc.TestID);
            command.Parameters.AddWithValue("@testTarih", testSonuc.TestTarih);
            command.Parameters.AddWithValue("@testSonuc",
                string.IsNullOrWhiteSpace(testSonuc.SonucDegeri) ? DBNull.Value : (object)testSonuc.SonucDegeri);
            command.Parameters.AddWithValue("@testDegerlendirme",
                string.IsNullOrWhiteSpace(testSonuc.TestDegerlendirme) ? DBNull.Value : (object)testSonuc.TestDegerlendirme);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<TestSonuc> HastaTestSonuclariListele(string hastaTC)
        {
            var liste = new List<TestSonuc>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_HastaTestSonuclariListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTC", hastaTC);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(MapToTestSonuc(reader));
            }
            return liste;
        }

        private TestSonuc MapToTestSonuc(SqlDataReader reader)
        {
            return new TestSonuc
            {
                TestSonucID = (int)reader["testSonucID"],
                HastaTC = reader["hastaTC"].ToString()!,
                PsikologID = (byte)reader["psikologID"],
                TestID = (byte)reader["testID"],
                TestTarih = (DateTime)reader["testTarih"],
                SonucDegeri = reader["testSonuc"] == DBNull.Value ? null : reader["testSonuc"].ToString(),
                TestDegerlendirme = reader["testDegerlendirme"] == DBNull.Value ? null : reader["testDegerlendirme"].ToString()
            };
        }
    }
}
