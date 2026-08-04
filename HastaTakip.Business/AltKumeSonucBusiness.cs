using HastaTakip.DataAccess;
using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Business
{
    public class AltKumeSonucBusiness
    {
        private readonly AltKumeSonucDal _dal;
        public AltKumeSonucBusiness(AltKumeSonucDal dal) { _dal = dal; }
        public void AltKumeSonucEkle(AltKumeSonuc s) => _dal.AltKumeSonucEkle(s);
        public List<AltKumeSonuc> AltKumeSonuclariListele(int testSonucID) => _dal.AltKumeSonuclariListele(testSonucID);
        public void AltKumeSonuclariSil(int testSonucID) => _dal.AltKumeSonuclariSil(testSonucID);
    }
}
