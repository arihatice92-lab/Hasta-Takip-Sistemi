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
    }
}
