using HastaTakip.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HastaTakip.DataAccess
{
    public class HastaDal
    {
        private readonly DbHelper _dbHelper;

        public HastaDal(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public void HastaEkle(Hasta hasta)
        {
            using (var connection = _dbHelper.GetConnection())
            using (var command = new SqlCommand("sp_HastaKaydet", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@hastaTC", hasta.HastaTC);
                command.Parameters.AddWithValue("@hastaAd", hasta.HastaAd);
                command.Parameters.AddWithValue("@hastaSoyad", hasta.HastaSoyad);
                command.Parameters.AddWithValue("@hastaTel", hasta.HastaTel);
                command.Parameters.AddWithValue("@hastaAdres", hasta.HastaAdres);
                command.Parameters.AddWithValue("@hastaCinsiyet", hasta.HastaCinsiyet);
                command.Parameters.AddWithValue("@hastaDogumTarihi", hasta.HastaDogumTarihi);
                command.Parameters.AddWithValue("@hastaOkul", hasta.HastaOkul);
                command.Parameters.AddWithValue("@hastaSinif", hasta.HastaSinif.HasValue ?(object)hasta.HastaSinif.Value : DBNull.Value);
                command.Parameters.AddWithValue("@hastaOkulBasarisi", hasta.HastaOkulBasarisi);
                command.Parameters.AddWithValue("@hastaBoy", hasta.HastaBoy.HasValue ? (object)hasta.HastaBoy.Value : DBNull.Value);
                command.Parameters.AddWithValue("@hastaKilo", hasta.HastaKilo.HasValue ? (object)hasta.HastaKilo.Value : DBNull.Value);
                command.Parameters.AddWithValue("@hastaYonlendiren", hasta.HastaYonlendiren);
                command.Parameters.AddWithValue("@hastaBasvuruNedeni", hasta.HastaBasvuruNedeni);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public Hasta? HastaGetir(string tc)
        {
            using (var connection = _dbHelper.GetConnection())
            using (var command = new SqlCommand("sp_HastaBilgisi", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TC", tc);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapToHasta(reader);
                    }
                    return null;
                }
            }
        }

        public List<Hasta> HastaListele(bool sadeceAktif = true)
        {
            var hastalar = new List<Hasta>();
            var sorgu = sadeceAktif
                ? "SELECT * FROM tblHastalar WHERE hastaAktif = 1 ORDER BY hastaAd, hastaSoyad"
                : "SELECT * FROM tblHastalar ORDER BY hastaAd, hastaSoyad";

            using (var connection = _dbHelper.GetConnection())
            using (var command = new SqlCommand(sorgu, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        hastalar.Add(MapToHasta(reader));
                    }
                }
            }

            return hastalar;
        }

        public void HastaGuncelle(Hasta hasta)
        {
            using (var connection = _dbHelper.GetConnection())
            using (var command = new SqlCommand("sp_HastaGuncelle", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@hastaTC", hasta.HastaTC);
                command.Parameters.AddWithValue("@hastaAd", hasta.HastaAd);
                command.Parameters.AddWithValue("@hastaSoyad", hasta.HastaSoyad);
                command.Parameters.AddWithValue("@hastaTel", hasta.HastaTel);
                command.Parameters.AddWithValue("@hastaAdres", hasta.HastaAdres);
                command.Parameters.AddWithValue("@hastaCinsiyet", hasta.HastaCinsiyet);
                command.Parameters.AddWithValue("@hastaDogumTarihi", hasta.HastaDogumTarihi);
                command.Parameters.AddWithValue("@hastaOkul", hasta.HastaOkul);
                command.Parameters.AddWithValue("@hastaSinif", hasta.HastaSinif.HasValue ? (object)hasta.HastaSinif.Value : DBNull.Value);
                command.Parameters.AddWithValue("@hastaOkulBasarisi", hasta.HastaOkulBasarisi);
                command.Parameters.AddWithValue("@hastaBoy", hasta.HastaBoy.HasValue ? (object)hasta.HastaBoy.Value : DBNull.Value);
                command.Parameters.AddWithValue("@hastaKilo", hasta.HastaKilo.HasValue ? (object)hasta.HastaKilo.Value : DBNull.Value);
                command.Parameters.AddWithValue("@hastaYonlendiren", hasta.HastaYonlendiren);
                command.Parameters.AddWithValue("@hastaBasvuruNedeni", hasta.HastaBasvuruNedeni);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void HastaSil(string tc)
        {
            using (var connection = _dbHelper.GetConnection())
            using (var command = new SqlCommand("sp_HastaSil", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TC", tc);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void HastaPasifeAl(string tc)
        {
            using (var connection = _dbHelper.GetConnection())
            using (var command = new SqlCommand("sp_HastaPasifeAl", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@hastaTC", tc);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void HastaAktifEt(string tc)
        {
            using (var connection = _dbHelper.GetConnection())
            using (var command = new SqlCommand("sp_HastaAktifEt", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@hastaTC", tc);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private Hasta MapToHasta(SqlDataReader reader)
        {
            return new Hasta
            {
                HastaID = (int)reader["hastaID"],
                HastaTC = reader["hastaTC"].ToString()!,
                HastaDosyaNo = reader["hastaDosyaNo"] == DBNull.Value ? null : reader["hastaDosyaNo"].ToString(),
                HastaAd = reader["hastaAd"].ToString()!,
                HastaSoyad = reader["hastaSoyad"].ToString()!,
                HastaTel = reader["hastaTel"].ToString()!,
                HastaAdres = reader["hastaAdres"].ToString()!,
                HastaCinsiyet = reader["hastaCinsiyet"].ToString()!,
                HastaDogumTarihi = (DateTime)reader["hastaDogumTarihi"],
                HastaOkul = reader["hastaOkul"].ToString()!,
                HastaSinif = reader["hastaSinif"] == DBNull.Value ? (byte?)null : (byte)reader["hastaSinif"],
                HastaOkulBasarisi = reader["hastaOkulBasarisi"].ToString()!,
                HastaBoy = reader["hastaBoy"] == DBNull.Value ? (byte?)null : (byte)reader["hastaBoy"],
                HastaKilo = reader["hastaKilo"] == DBNull.Value ? (byte?)null : (byte)reader["hastaKilo"],
                HastaYonlendiren = reader["hastaYonlendiren"].ToString()!,
                HastaBasvuruNedeni = reader["hastaBasvuruNedeni"].ToString()!,
                HastaBasvuruTarihi = (DateTime)reader["hastaBasvuruTarihi"],
                HastaAktif = (bool)reader["hastaAktif"]
            };
        }
    }
}
