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
    }
}
