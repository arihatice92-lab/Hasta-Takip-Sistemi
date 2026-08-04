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
    }
}
