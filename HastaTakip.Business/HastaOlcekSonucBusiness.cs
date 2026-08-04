using HastaTakip.DataAccess;
using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Business
{
    public class HastaOlcekSonucBusiness
    {
        private readonly HastaOlcekSonucDal _dal;
        public HastaOlcekSonucBusiness(HastaOlcekSonucDal dal) { _dal = dal; }
        public void OlcekSonucEkle(OlcekSonuc o) => _dal.OlcekSonucEkle(o);
        public OlcekSonuc? OlcekSonucGetir(int id) => _dal.OlcekSonucGetir(id);
        public void OlcekSonucGuncelle(OlcekSonuc o) => _dal.OlcekSonucGuncelle(o);
        public List<OlcekSonuc> HastaOlcekSonuclariListele(string hastaTC) => _dal.HastaOlcekSonuclariListele(hastaTC);

    }

}
