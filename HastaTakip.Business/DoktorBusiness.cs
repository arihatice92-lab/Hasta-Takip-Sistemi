using System;
using System.Collections.Generic;
using HastaTakip.DataAccess;
using HastaTakip.Entities;

namespace HastaTakip.Business
{
    public class DoktorBusiness
    {
        private readonly DoktorDal _doktorDal;

        public DoktorBusiness(DoktorDal doktorDal)
        {
            _doktorDal = doktorDal;
        }

        public void DoktorKaydet(Doktor doktor)
        {
            if (doktor.DoktorKurumBaslangicTarih.HasValue &&
                doktor.DoktorKurumBaslangicTarih.Value.Date > DateTime.Today)
            {
                throw new Exception("Kurum başlangıç tarihi gelecekte olamaz.");
            }

            _doktorDal.DoktorEkle(doktor);
        }

        public Doktor? DoktorGetir(short doktorID)
        {
            return _doktorDal.DoktorGetir(doktorID);
        }

        public List<Doktor> DoktorListele(bool sadeceAktif = true)
        {
            return _doktorDal.DoktorListele(sadeceAktif);
        }

        public void DoktorGuncelle(Doktor doktor)
        {
            _doktorDal.DoktorGuncelle(doktor);
        }

        public void DoktorAyrilis(short doktorID)
        {
            _doktorDal.DoktorAyrilis(doktorID);
        }
    }
}