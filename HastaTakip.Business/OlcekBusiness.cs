using HastaTakip.DataAccess;
using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

namespace HastaTakip.Business
{
    public class OlcekBusiness
    {
        private readonly OlcekDal _olcekDal;
        public OlcekBusiness(OlcekDal olcekDal) { _olcekDal = olcekDal; }
        public List<Olcek> OlcekleriListele() => _olcekDal.OlcekleriListele();

        public void OlcekEkle(Olcek olcek)
        {
            if (string.IsNullOrWhiteSpace(olcek.OlcekAdi))
                throw new Exception("Ölçek adı boş olamaz.");
            _olcekDal.OlcekEkle(olcek);
        }
        public List<Olcek> OlcekAra(string? ara, string aktif) => _olcekDal.OlcekAra(ara, aktif);
        public Olcek? OlcekGetir(byte olcekID) => _olcekDal.OlcekGetir(olcekID);

        public void OlcekGuncelle(Olcek olcek) => _olcekDal.OlcekGuncelle(olcek);

        public void OlcekSil(byte olcekID)
        {
            try
            {
                _olcekDal.OlcekGetir(olcekID);
            }
            catch (SqlException)
            {
                throw new Exception("Bu ölçek daha önce bir hastaya uygulanmış olduğu için silinemez.");
            }
        }
        public void OlcekPasifeAl(byte olcekID) => _olcekDal.OlcekPasifeAl(olcekID);
        public void OlcekAktifEt(byte olcekID) => _olcekDal.OlcekAktifEt(olcekID);
    }
}
