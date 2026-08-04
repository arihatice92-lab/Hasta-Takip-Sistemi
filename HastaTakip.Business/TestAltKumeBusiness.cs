using HastaTakip.DataAccess;
using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Business
{
    public class TestAltKumeBusiness
    {
        private readonly TestAltKumeDal _dal;
        public TestAltKumeBusiness(TestAltKumeDal dal) { _dal = dal; }
        public List<TestAltKume> TumAltKumeleriListele() => _dal.TumAltKumeleriListele();
    }
}
