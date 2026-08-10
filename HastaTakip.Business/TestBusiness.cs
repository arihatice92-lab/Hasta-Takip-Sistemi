using HastaTakip.DataAccess;
using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;


namespace HastaTakip.Business
{
    public class TestBusiness
    {
        private readonly TestDal _testDal;
        public TestBusiness(TestDal testDal) { _testDal = testDal; }

        public List<Test> TestleriListele() => _testDal.TestleriListele();

        public void TestEkle(Test test)
        {
            if (string.IsNullOrWhiteSpace(test.TestAdi))
                throw new Exception("Test adı boş olamaz.");
            _testDal.TestEkle(test);
        }

        public Test? TestGetir(byte testID) => _testDal.TestGetir(testID);

        public void TestGuncelle(Test test) => _testDal.TestGuncelle(test);

        public void TestSil(byte testID)
        {
            try
            {
                _testDal.TestSil(testID);
            }
            catch (SqlException)
            {
                throw new Exception("Bu test daha önce bir hastaya uygulanmış olduğu için silinemez.");
            }
        }
    }
}