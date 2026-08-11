
using System.Collections.Generic;
using System.Data;
using HastaTakip.Entities;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class PsikologRandevuSaatDal
    {
        private readonly DbHelper _dbHelper;
        public PsikologRandevuSaatDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public List<PsikologRandevuSaat> SaatleriListele()
        {
            var saatler = new List<PsikologRandevuSaat>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_PsikologRandevuSaatleriListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                saatler.Add(new PsikologRandevuSaat
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