using System.Collections.Generic;
using System.Data;
using HastaTakip.Entities;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class IstatistikDal
    {
        private readonly DbHelper _dbHelper;
        public IstatistikDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public List<TaniCinsiyetIstatistigi> TaniCinsiyetIstatistigiGetir()
        {
            var liste = new List<TaniCinsiyetIstatistigi>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TaniCinsiyetIstatistigi", connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(new TaniCinsiyetIstatistigi
                {
                    TaniAdi = reader["taniAdi"].ToString()!,
                    Cinsiyet = reader["hastaCinsiyet"].ToString()!,
                    HastaSayisi = (int)reader["HastaSayisi"]
                });
            }
            return liste;
        }

        public List<YasDagilimiIstatistigi> YasDagilimiIstatistigiGetir()
        {
            var liste = new List<YasDagilimiIstatistigi>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_YasDagilimiIstatistigi", connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(new YasDagilimiIstatistigi
                {
                    YasGrubu = reader["YasGrubu"].ToString()!,
                    HastaSayisi = (int)reader["HastaSayisi"]
                });
            }
            return liste;
        }

        public List<TaniOkulBasarisiCinsiyetIstatistigi> TaniOkulBasarisiCinsiyetIstatistigiGetir()
        {
            var liste = new List<TaniOkulBasarisiCinsiyetIstatistigi>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TaniOkulBasarisiCinsiyetIstatistigi", connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(new TaniOkulBasarisiCinsiyetIstatistigi
                {
                    TaniAdi = reader["taniAdi"].ToString()!,
                    Cinsiyet = reader["hastaCinsiyet"].ToString()!,
                    OkulBasarisi = reader["hastaOkulBasarisi"].ToString()!,
                    HastaSayisi = (int)reader["HastaSayisi"]
                });
            }
            return liste;
        }

        public List<TaniCinsiyetBMIIstatistigi> TaniCinsiyetBMIIstatistigiGetir()
        {
            var liste = new List<TaniCinsiyetBMIIstatistigi>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_TaniCinsiyetBMIIstatistigi", connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(new TaniCinsiyetBMIIstatistigi
                {
                    TaniAdi = reader["taniAdi"].ToString()!,
                    Cinsiyet = reader["hastaCinsiyet"].ToString()!,
                    HastaSayisi = (int)reader["HastaSayisi"],
                    OrtalamaBMI = (decimal)reader["OrtalamaBMI"],
                    MinBMI = (decimal)reader["MinBMI"],
                    MaxBMI = (decimal)reader["MaxBMI"]
                });
            }
            return liste;
        }
    }
}