using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class Kullanici
    {
        public int KullaniciID { get; set; }
        public string? KullaniciAdi { get; set; }
        public string SifreHash { get; set; } = string.Empty;
        public string AdSoyad { get; set; } = string.Empty;
        public byte RolID { get; set; }
        public short? DoktorID { get; set; }
        public byte? PsikologID { get; set; }
        public bool KullaniciAktif { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? SonGirisTarihi { get; set; }
    }
}
