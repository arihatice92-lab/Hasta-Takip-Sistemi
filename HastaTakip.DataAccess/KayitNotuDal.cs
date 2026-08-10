using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class KayitNotuDal
    {
        private readonly DbHelper _dbHelper;
        public KayitNotuDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public void NotEkle(string kayitTuru, int kayitID, int kullaniciID, string notMetni)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_KayitNotuEkle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@kayitTuru", kayitTuru);
            command.Parameters.AddWithValue("@kayitID", kayitID);
            command.Parameters.AddWithValue("@kullaniciID", kullaniciID);
            command.Parameters.AddWithValue("@notMetni", notMetni);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<KayitNotu> NotlariListele(string kayitTuru, int kayitID)
        {
            var liste = new List<KayitNotu>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_KayitNotlariListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@kayitTuru", kayitTuru);
            command.Parameters.AddWithValue("@kayitID", kayitID);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(new KayitNotu
                {
                    NotID = (int)reader["notID"],
                    KayitTuru = reader["kayitTuru"].ToString()!,
                    KayitID = (int)reader["kayitID"],
                    KullaniciID = (int)reader["kullaniciID"],
                    NotMetni = reader["notMetni"].ToString()!,
                    NotTarihi = (DateTime)reader["notTarihi"]
                });
            }
            return liste;
        }
    }
}
