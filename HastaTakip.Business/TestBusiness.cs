using HastaTakip.DataAccess;
using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Business
{
    public class TestBusiness
    {
        private readonly TestDal _testDal;
        public TestBusiness(TestDal testDal) { _testDal = testDal; }
        public List<Test> TestleriListele() => _testDal.TestleriListele();
    }
}
