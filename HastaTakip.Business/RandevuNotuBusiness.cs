namespace HastaTakip.Business
{
    public class RandevuNotuBusiness
    {
        private readonly HastaTakip.DataAccess.RandevuNotuDal _dal;
        public RandevuNotuBusiness(HastaTakip.DataAccess.RandevuNotuDal dal) { _dal = dal; }

        public void RandevuNotuEkle(HastaTakip.Entities.RandevuNotu notu) => _dal.RandevuNotuEkle(notu);
        public HastaTakip.Entities.RandevuNotu? RandevuNotuGetir(short id) => _dal.RandevuNotuGetir(id);
        public HastaTakip.Entities.RandevuNotu? RandevuNotuGetirByRandevuTarihID(int randevuTarihID) => _dal.RandevuNotuGetirByRandevuTarihID(randevuTarihID);
        public void RandevuNotuGuncelle(HastaTakip.Entities.RandevuNotu notu) => _dal.RandevuNotuGuncelle(notu);
        public System.Collections.Generic.List<HastaTakip.Entities.RandevuNotu> HastaRandevuNotlariListele(string hastaTC) => _dal.HastaRandevuNotlariListele(hastaTC);
    }
}