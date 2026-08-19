using HastaTakip.DataAccess;
using HastaTakip.Entities;
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

        public List<Ilac> IlacAra (string? ara, string aktif){ return _ilacDal.IlacAra(ara, aktif); }
        public Ilac? IlacGetir(short ilacID) => _ilacDal.IlacGetir(ilacID);
        public void IlacGuncelle(Ilac ilac) => _ilacDal.IlacGuncelle(ilac);
        public void IlacPasifeAl(short ilacID) => _ilacDal.IlacPasifeAl(ilacID);
        public void IlacAktifEt(short ilacID) => _ilacDal.IlacAktifEt(ilacID);

    }
}
