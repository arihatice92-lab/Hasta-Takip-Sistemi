using HastaTakip.DataAccess;
using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Business
{
    public class KayitNotuBusiness
    {
        private readonly KayitNotuDal _dal;
        public KayitNotuBusiness(KayitNotuDal dal) { _dal = dal; }
        public void NotEkle(string kayitTuru, int kayitID, int kullaniciID, string notMetni) => _dal.NotEkle(kayitTuru, kayitID, kullaniciID, notMetni);
        public List<KayitNotu> NotlariListele(string kayitTuru, int kayitID) => _dal.NotlariListele(kayitTuru, kayitID);
    }
}
