using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Business
{
    public class TaniBusiness
    {
        private readonly HastaTakip.DataAccess.TaniDal _taniDal;
        public TaniBusiness(HastaTakip.DataAccess.TaniDal taniDal) { _taniDal = taniDal; }
        public System.Collections.Generic.List<HastaTakip.Entities.Tani> TanilariListele() => _taniDal.TanilariListele();
    }
}
