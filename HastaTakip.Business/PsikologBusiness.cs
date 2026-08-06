using HastaTakip.DataAccess;
using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Business
{
    public class PsikologBusiness
    {
        private readonly PsikologDal _psikologDal;
        public PsikologBusiness(PsikologDal psikologDal) { _psikologDal = psikologDal; }
        public List<Psikolog> PsikologlariListele() => _psikologDal.PsikologlariListele();

        public void PsikologEkle(Psikolog psikolog)
        {
            if (psikolog.PsikologKurumBaslangicTarih.HasValue &&
                psikolog.PsikologKurumBaslangicTarih.Value.Date > DateTime.Today)
            {
                throw new Exception("Kurum başlangıç tarihi gelecekte olamaz.");
            }
            _psikologDal.PsikologEkle(psikolog);
        }

        public Psikolog? PsikologGetir(byte psikologID) => _psikologDal.PsikologGetir(psikologID);

        public void PsikologGuncelle(Psikolog psikolog)
        {
            if (psikolog.PsikologKurumBaslangicTarih.HasValue &&
                psikolog.PsikologKurumBaslangicTarih.Value.Date > DateTime.Today)
            {
                throw new Exception("Kurum başlangıç tarihi gelecekte olamaz.");
            }
            _psikologDal.PsikologGuncelle(psikolog);
        }

        public void PsikologAyrilis(byte psikologID) => _psikologDal.PsikologAyrilis(psikologID);

        public List<Psikolog> PsikologAra(string? ara, string siralama, string aktif)
            => _psikologDal.PsikologAra(ara, siralama, aktif);
    }


}
