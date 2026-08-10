using System;
using System.Collections.Generic;
using System.Data;
using HastaTakip.Entities;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class AileOykusuDal
    {
        private readonly DbHelper _dbHelper;
        public AileOykusuDal(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public int AileOykusuEkle(AileOykusu oyku, int kullaniciID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_AileOykusuEkle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTC", oyku.HastaTC);
            command.Parameters.AddWithValue("@kullaniciID", kullaniciID);
            command.Parameters.AddWithValue("@anneBabaninKisiselOykusu",
                string.IsNullOrWhiteSpace(oyku.AnneBabaninKisiselOykusu) ? DBNull.Value : (object)oyku.AnneBabaninKisiselOykusu);
            command.Parameters.AddWithValue("@anneBabaninEvlilikOykusu",
                string.IsNullOrWhiteSpace(oyku.AnneBabaninEvlilikOykusu) ? DBNull.Value : (object)oyku.AnneBabaninEvlilikOykusu);
            command.Parameters.AddWithValue("@aileOzellikleri",
                string.IsNullOrWhiteSpace(oyku.AileOzellikleri) ? DBNull.Value : (object)oyku.AileOzellikleri);
            command.Parameters.AddWithValue("@anneBabaKardesler",
                string.IsNullOrWhiteSpace(oyku.AnneBabaKardesler) ? DBNull.Value : (object)oyku.AnneBabaKardesler);

            var yeniIDParam = new SqlParameter("@YeniID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command.Parameters.Add(yeniIDParam);

            connection.Open();
            command.ExecuteNonQuery();
            return (int)yeniIDParam.Value;
        }

        public AileOykusu? AileOykusuGetir(int aileOykuID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_AileOykusuGetir", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@aileOykuID", aileOykuID);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapToAileOykusu(reader);
            }
            return null;
        }

        public void AileOykusuGuncelle(AileOykusu oyku, int kullaniciID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_AileOykusuGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@aileOykuID", oyku.AileOykuID);
            command.Parameters.AddWithValue("@kullaniciID", kullaniciID);
            command.Parameters.AddWithValue("@anneBabaninKisiselOykusu",
                string.IsNullOrWhiteSpace(oyku.AnneBabaninKisiselOykusu) ? DBNull.Value : (object)oyku.AnneBabaninKisiselOykusu);
            command.Parameters.AddWithValue("@anneBabaninEvlilikOykusu",
                string.IsNullOrWhiteSpace(oyku.AnneBabaninEvlilikOykusu) ? DBNull.Value : (object)oyku.AnneBabaninEvlilikOykusu);
            command.Parameters.AddWithValue("@aileOzellikleri",
                string.IsNullOrWhiteSpace(oyku.AileOzellikleri) ? DBNull.Value : (object)oyku.AileOzellikleri);
            command.Parameters.AddWithValue("@anneBabaKardesler",
                string.IsNullOrWhiteSpace(oyku.AnneBabaKardesler) ? DBNull.Value : (object)oyku.AnneBabaKardesler);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<AileOykusu> HastaAileOykusuListele(string hastaTC)
        {
            var liste = new List<AileOykusu>();
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_HastaAileOykusuListele", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaTC", hastaTC);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                liste.Add(MapToAileOykusu(reader));
            }
            return liste;
        }

        private AileOykusu MapToAileOykusu(SqlDataReader reader)
        {
            return new AileOykusu
            {
                AileOykuID = (int)reader["aileOykuID"],
                HastaTC = reader["hastaTC"].ToString()!,
                AnneBabaninKisiselOykusu = reader["anneBabaninKisiselOykusu"] == DBNull.Value ? null : reader["anneBabaninKisiselOykusu"].ToString(),
                AnneBabaninEvlilikOykusu = reader["anneBabaninEvlilikOykusu"] == DBNull.Value ? null : reader["anneBabaninEvlilikOykusu"].ToString(),
                AileOzellikleri = reader["aileOzellikleri"] == DBNull.Value ? null : reader["aileOzellikleri"].ToString(),
                AnneBabaKardesler = reader["anneBabaKardesler"] == DBNull.Value ? null : reader["anneBabaKardesler"].ToString(),
                SonGuncelleyenKullaniciID = reader["sonGuncelleyenKullaniciID"] == DBNull.Value ? null : (int?)reader["sonGuncelleyenKullaniciID"],
                SonGuncellemeTarihi = reader["sonGuncellemeTarihi"] == DBNull.Value ? null : (DateTime?)reader["sonGuncellemeTarihi"]
            };
        }
    }
}