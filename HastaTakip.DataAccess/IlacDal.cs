using System.Collections.Generic;
using System.Data;
using HastaTakip.Entities;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class IlacDal
    {
        private readonly DbHelper _dbHelper;
        public IlacDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public List<Ilac> IlaclariListele()
        {
            var ilaclar = new List<Ilac>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_IlaclariListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ilaclar.Add(new Ilac
                {
                    IlacID = (short)reader["ilacID"],
                    IlacAdi = reader["ilacAdi"].ToString()!,
                    IlacEtkenMadde = reader["ilacEtkenMadde"].ToString()!
                });
            }
            return ilaclar;
        }

        public void IlacEkle (Ilac ilac)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_IlacEkle", connection);
            command.CommandType=CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ilacAdi", ilac.IlacAdi);
            command.Parameters.AddWithValue("@ilacEtkenMadde", ilac.IlacEtkenMadde);
            connection.Open();
            command.ExecuteNonQuery();

        }

        public void IlacSil(short ilacID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_IlacSil", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ilacID", ilacID);
            connection.Open();
            command.ExecuteNonQuery ();

        }
    }
}