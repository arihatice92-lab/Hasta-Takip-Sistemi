using HastaTakip.DataAccess;
using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

namespace HastaTakip.Business
{
    public class TestAltKumeBusiness
    {
        private readonly TestAltKumeDal _dal;
        public TestAltKumeBusiness(TestAltKumeDal dal) { _dal = dal; }
        public List<TestAltKume> TumAltKumeleriListele() => _dal.TumAltKumeleriListele();

        public void AltKumeEkle(TestAltKume altKume) => _dal.AltKumeEkle(altKume);

        public void AltKumeSil(byte testAltKumeID)
        {
            try
            {
                _dal.AltKumeSil(testAltKumeID);
            }
            catch (SqlException)
            {
                throw new Exception("Bu alt küme daha önce bir sonuçta kullanılmış olduğu için silinemez.");
            }
        }

        public List<TestAltKume> AltKumeleriListeleByTestID(byte testID) => _dal.AltKumeleriListeleByTestID(testID);
    }
}
