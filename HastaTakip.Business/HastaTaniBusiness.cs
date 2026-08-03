using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Business
{
    public class HastaTaniBusiness
    {
        private readonly HastaTakip.DataAccess.HastaTaniDal _hastaTaniDal;
        public HastaTaniBusiness(HastaTakip.DataAccess.HastaTaniDal hastaTaniDal) { _hastaTaniDal = hastaTaniDal; }
        public void HastaTaniEkle(HastaTakip.Entities.HastaTani hastaTani) => _hastaTaniDal.HastaTaniEkle(hastaTani);
        public System.Collections.Generic.List<HastaTakip.Entities.HastaTani> HastaTanilariListele(string hastaTC)
            => _hastaTaniDal.HastaTanilariListele(hastaTC);

        public HastaTani? HastaTaniGetir(int hastaTaniID) => _hastaTaniDal.HastaTaniGetir(hastaTaniID);
        public void HastaTaniGuncelle(HastaTani hastaTani) => _hastaTaniDal.HastaTaniGuncelle(hastaTani);
    }
}
