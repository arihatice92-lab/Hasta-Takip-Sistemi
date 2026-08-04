using HastaTakip.DataAccess;
using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Business
{
    public class HastaTestSonucBusiness
    {
        private readonly HastaTestSonucDal _dal;
        public HastaTestSonucBusiness(HastaTestSonucDal dal) { _dal = dal; }
        public int TestSonucEkle(TestSonuc t) => _dal.TestSonucEkle(t);
        public TestSonuc? TestSonucGetir(int id) => _dal.TestSonucGetir(id);
        public void TestSonucGuncelle(TestSonuc t) => _dal.TestSonucGuncelle(t);
        public List<TestSonuc> HastaTestSonuclariListele(string hastaTC) => _dal.HastaTestSonuclariListele(hastaTC);
    }

}
