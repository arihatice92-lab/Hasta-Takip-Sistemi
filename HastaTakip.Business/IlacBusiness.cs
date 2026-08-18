using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Business
{
    public class IlacBusiness
    {
        private readonly HastaTakip.DataAccess.IlacDal _ilacDal;
        public IlacBusiness(HastaTakip.DataAccess.IlacDal ilacDal) { _ilacDal = ilacDal; }
        public System.Collections.Generic.List<HastaTakip.Entities.Ilac> IlaclariListele() => _ilacDal.IlaclariListele();

        public void IlacEkle(HastaTakip.Entities.Ilac ilac)
        {
            _ilacDal.IlacEkle(ilac);
        }
        public void IlacSil(short ilacID)
        {
            _ilacDal.IlacSil(ilacID);
        }
    }
}
