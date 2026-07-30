using System;
using System.Collections.Generic;
using System.Data;
using HastaTakip.Entities;
using Microsoft.Data.SqlClient;

namespace HastaTakip.DataAccess
{
    public class DoktorDal
    {
        private readonly DbHelper _dbHelper;

        public DoktorDal(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public void DoktorEkle(Doktor doktor)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_DoktorKaydet", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@doktorSicilNo", doktor.DoktorSicilNo);
            command.Parameters.AddWithValue("@doktorAd", doktor.DoktorAd);
            command.Parameters.AddWithValue("@doktorSoyad", doktor.DoktorSoyad);
            command.Parameters.AddWithValue("@doktorTel", doktor.DoktorTel);
            command.Parameters.AddWithValue("@doktorBrans", doktor.DoktorBrans);
            command.Parameters.AddWithValue("@doktorKurumBaslangicTarih",
                doktor.DoktorKurumBaslangicTarih.HasValue ? (object)doktor.DoktorKurumBaslangicTarih.Value : DBNull.Value);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public Doktor? DoktorGetir(short doktorID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand(
                "SELECT * FROM tblDoktorlar WHERE doktorID = @doktorID", connection);
            command.Parameters.AddWithValue("@doktorID", doktorID);

            connection.Open();
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return MapToDoktor(reader);
            }
            return null;
        }

        public List<Doktor> DoktorListele(bool sadeceAktif = true)
        {
            var doktorlar = new List<Doktor>();
            var sorgu = sadeceAktif
                ? "SELECT * FROM tblDoktorlar WHERE doktorKurumAyrilisTarih IS NULL ORDER BY doktorAd, doktorSoyad"
                : "SELECT * FROM tblDoktorlar ORDER BY doktorAd, doktorSoyad";

            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand(sorgu, connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                doktorlar.Add(MapToDoktor(reader));
            }

            return doktorlar;
        }

        public void DoktorGuncelle(Doktor doktor)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_DoktorGuncelle", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@doktorID", doktor.DoktorID);
            command.Parameters.AddWithValue("@doktorAd", doktor.DoktorAd);
            command.Parameters.AddWithValue("@doktorSoyad", doktor.DoktorSoyad);
            command.Parameters.AddWithValue("@doktorTel", doktor.DoktorTel);
            command.Parameters.AddWithValue("@doktorBrans", doktor.DoktorBrans);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void DoktorAyrilis(short doktorID)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_DoktorAyrilis", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@doktorID", doktorID);

            connection.Open();
            command.ExecuteNonQuery();
        }

        private Doktor MapToDoktor(SqlDataReader reader)
        {
            return new Doktor
            {
                DoktorID = (short)reader["doktorID"],
                DoktorSicilNo = reader["doktorSicilNo"].ToString()!,
                DoktorAd = reader["doktorAd"].ToString()!,
                DoktorSoyad = reader["doktorSoyad"].ToString()!,
                DoktorTel = reader["doktorTel"].ToString()!,
                DoktorBrans = reader["doktorBrans"].ToString()!,
                DoktorKurumBaslangicTarih = reader["doktorKurumBaslangicTarih"] == DBNull.Value
                    ? null : (DateTime?)reader["doktorKurumBaslangicTarih"],
                DoktorKurumAyrilisTarih = reader["doktorKurumAyrilisTarih"] == DBNull.Value
                    ? null : (DateTime?)reader["doktorKurumAyrilisTarih"]
            };
        }
    }
}