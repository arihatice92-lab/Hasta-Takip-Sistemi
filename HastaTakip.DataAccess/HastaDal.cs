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

        public Hasta? HastaGetirById(Guid hastaGuid)
        {
            using var connection = _dbHelper.GetConnection();
            using var command = new SqlCommand("sp_HastaGetirById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@hastaGuid", hastaGuid);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapToHasta(reader);
            }
            return null;
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

        public (List<Hasta> Hastalar, int toplamKayit) HastaAra(
            string? ara,
            string siralama,
            bool? aktif,
            string? cinsiyet,
            DateTime? baslangicTarihi,
            DateTime? bitisTarihi,
            int sayfa,
            int sayfaBoyutu)
        {
            var hastalar = new List<Hasta>();
            int toplamKayit;

            using (var connection = _dbHelper.GetConnection())
            using (var command = new SqlCommand("sp_HastaAra", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Ara",
                    string.IsNullOrWhiteSpace(ara) ? DBNull.Value : (object)ara);

                command.Parameters.AddWithValue("@Siralama", siralama);

                command.Parameters.AddWithValue("@Aktif",
                    aktif.HasValue ? (object)aktif.Value : DBNull.Value);

                command.Parameters.AddWithValue("@Cinsiyet",
                    string.IsNullOrWhiteSpace(cinsiyet) ? DBNull.Value : (object)cinsiyet);

                command.Parameters.AddWithValue("@BaslangicTarihi",
                    baslangicTarihi.HasValue ? (object)baslangicTarihi.Value : DBNull.Value);

                command.Parameters.AddWithValue("@BitisTarihi",
                    bitisTarihi.HasValue ? (object)bitisTarihi.Value : DBNull.Value);
                command.Parameters.AddWithValue("@Sayfa", sayfa);
                command.Parameters.AddWithValue("@SayfaBoyutu", sayfaBoyutu);

                var toplamKayitParam = new SqlParameter("@ToplamKayit", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(toplamKayitParam);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        hastalar.Add(MapToHasta(reader));
                    }
                }

                // OUTPUT parametresinin değeri, reader kapandıktan sonra okunabilir hale gelir
                toplamKayit = (int)toplamKayitParam.Value;
            }

            return (hastalar, toplamKayit);
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
                HastaAktif = (bool)reader["hastaAktif"],
                HastaGuid = (Guid)reader["hastaGuid"]
            };
        }
    }
}
