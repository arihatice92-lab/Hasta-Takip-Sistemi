using System;
using Microsoft.Data.SqlClient;

namespace HastaTakip.Business
{
    public class TaniBusiness
    {
        private readonly HastaTakip.DataAccess.TaniDal _taniDal;
        public TaniBusiness(HastaTakip.DataAccess.TaniDal taniDal) { _taniDal = taniDal; }

        public System.Collections.Generic.List<HastaTakip.Entities.Tani> TanilariListele() => _taniDal.TanilariListele();

        public void TaniEkle(HastaTakip.Entities.Tani tani)
        {
            if (string.IsNullOrWhiteSpace(tani.TaniAdi))
                throw new Exception("Tanı adı boş olamaz.");
            _taniDal.TaniEkle(tani);
        }

        public HastaTakip.Entities.Tani? TaniGetir(short taniID) => _taniDal.TaniGetir(taniID);

        public void TaniGuncelle(HastaTakip.Entities.Tani tani) => _taniDal.TaniGuncelle(tani);

        public void TaniSil(short taniID)
        {
            try
            {
                _taniDal.TaniSil(taniID);
            }
            catch (SqlException)
            {
                throw new Exception("Bu tanı daha önce bir hastaya konulmuş olduğu için silinemez.");
            }
        }
    }
}