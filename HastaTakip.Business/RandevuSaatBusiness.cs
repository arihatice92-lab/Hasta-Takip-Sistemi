using System.Collections.Generic;
using HastaTakip.DataAccess;
using HastaTakip.Entities;

namespace HastaTakip.Business
{
    public class RandevuSaatBusiness
    {
        private readonly RandevuSaatDal _randevuSaatDal;

        public RandevuSaatBusiness(RandevuSaatDal randevuSaatDal)
        {
            _randevuSaatDal = randevuSaatDal;
        }

        public List<RandevuSaat> SaatleriListele()
        {
            return _randevuSaatDal.SaatleriListele();
        }
    }
}