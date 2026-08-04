using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    
    public class PsikologDal
    {
        private readonly DbHelper _dbHelper;
        public PsikologDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public List<Psikolog> PsikologlariListele()
        {
            var psikologlar = new List<Psikolog>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_PsikologlariListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                psikologlar.Add(new Psikolog
                {
                    PsikologID = (byte)reader["psikologID"],
                    PsikologSicilNo = reader["psikologSicilNo"] == DBNull.Value ? null : reader["psikologSicilNo"].ToString(),
                    PsikologAd = reader["psikologAd"].ToString()!,
                    PsikologSoyad = reader["psikologSoyad"].ToString()!,
                    PsikologTel = reader["psikologTel"].ToString()!
                });
            }
            return psikologlar;
        }
    }
}
