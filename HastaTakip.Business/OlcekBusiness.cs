using HastaTakip.DataAccess;
using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Business
{
    public class OlcekBusiness
    {
        private readonly OlcekDal _olcekDal;
        public OlcekBusiness(OlcekDal olcekDal) { _olcekDal = olcekDal; }
        public List<Olcek> OlcekleriListele() => _olcekDal.OlcekleriListele();
    }
}
