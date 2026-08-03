using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Business
{
    public class HastaTedaviBusiness
    {
        private readonly HastaTakip.DataAccess.HastaTedaviDal _hastaTedaviDal;
        public HastaTedaviBusiness(HastaTakip.DataAccess.HastaTedaviDal hastaTedaviDal) { _hastaTedaviDal = hastaTedaviDal; }
        public void HastaTedaviEkle(HastaTakip.Entities.HastaTedavi tedavi) => _hastaTedaviDal.HastaTedaviEkle(tedavi);
        public System.Collections.Generic.List<HastaTakip.Entities.HastaTedavi> HastaTedavileriListele(string hastaTC)
            => _hastaTedaviDal.HastaTedavileriListele(hastaTC);
        public HastaTedavi? HastaTedaviGetir(int tedaviID) => _hastaTedaviDal.HastaTedaviGetir(tedaviID);
        public void HastaTedaviGuncelle(HastaTedavi tedavi) => _hastaTedaviDal.HastaTedaviGuncelle(tedavi);
    }
}
