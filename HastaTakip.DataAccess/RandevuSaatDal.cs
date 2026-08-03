using System.Collections.Generic;
using System.Data;
using HastaTakip.Entities;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class RandevuSaatDal
    {
        private readonly DbHelper _dbHelper;

        public RandevuSaatDal(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<RandevuSaat> SaatleriListele()
        {
            var saatler = new List<RandevuSaat>();

            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand(
                "SELECT * FROM tblRandevuSaatler ORDER BY randevuBaslangicSaat", connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                saatler.Add(new RandevuSaat
                {
                    SaatID = (byte)reader["saatID"],
                    RandevuBaslangicSaat = (TimeSpan)reader["randevuBaslangicSaat"],
                    RandevuBitisSaat = (TimeSpan)reader["randevuBitisSaat"]
                });
            }

            return saatler;
        }
    }
}