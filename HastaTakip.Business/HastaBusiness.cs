using HastaTakip.DataAccess;
using HastaTakip.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Business
{
    public class HastaBusiness
    {
        private readonly HastaDal _hastaDal;

        public HastaBusiness(HastaDal hastaDal)
        {
            _hastaDal = hastaDal;
        }

        public void HastaKaydet(Hasta hasta)
        {
            var yas = HesaplaYas(hasta.HastaDogumTarihi, hasta.HastaBasvuruTarihi);
            if (yas >= 18)
            {
                throw new Exception("Hasta başvuru tarihinde 18 yaşından küçük olmalıdır.");
            }

            _hastaDal.HastaEkle(hasta);
        }

        public Hasta? HastaGetir(string tc)
        {
            return _hastaDal.HastaGetir(tc);
        }

        public List<Hasta> HastaListele(bool sadeceAktif = true)
        {
            return _hastaDal.HastaListele(sadeceAktif);
        }
        public (List<Hasta> Hastalar, int ToplamKayit) HastaAra(
     string? ara,
     string siralama,
     bool? aktif,
     string? cinsiyet,
     DateTime? baslangicTarihi,
     DateTime? bitisTarihi,
     int sayfa,
     int sayfaBoyutu)
        {
            return _hastaDal.HastaAra(ara, siralama, aktif, cinsiyet, baslangicTarihi, bitisTarihi, sayfa, sayfaBoyutu);
        }
        public void HastaGuncelle(Hasta hasta)
        {
            var yas = HesaplaYas(hasta.HastaDogumTarihi, hasta.HastaBasvuruTarihi);
            if (yas >= 18)
            {
                throw new Exception("Hasta başvuru tarihinde 18 yaşından küçük olmalıdır.");
            }

            _hastaDal.HastaGuncelle(hasta);
        }

        public void HastaSil(string tc)
        {
            _hastaDal.HastaSil(tc);
        }

        public void HastaPasifeAl(string tc)
        {
            _hastaDal.HastaPasifeAl(tc);
        }

        public void HastaAktifEt(string tc)
        {
            _hastaDal.HastaAktifEt(tc);
        }

        private int HesaplaYas(DateTime dogumTarihi, DateTime basvuruTarihi)
        {
            int yas = basvuruTarihi.Year - dogumTarihi.Year;
            if (dogumTarihi.Date > basvuruTarihi.AddYears(-yas))
                yas--;
            return yas;
        }
    }
}
