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
    }
}