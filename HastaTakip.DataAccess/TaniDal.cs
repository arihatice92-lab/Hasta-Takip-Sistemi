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
    }
}