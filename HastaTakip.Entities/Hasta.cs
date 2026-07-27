using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class Hasta
    {
        public int HastaID { get; set; }
        public string HastaTC { get; set; } = string.Empty;
        public string? HastaDosyaNo { get; set; }
        public string HastaAd { get; set; } = string.Empty;
        public string HastaSoyad { get; set; } = string.Empty;
        public string HastaTel { get; set; } = string.Empty;
        public string HastaAdres { get; set; } = string.Empty;
        public string HastaCinsiyet { get; set; } = string.Empty;
        public DateTime HastaDogumTarihi { get; set; }
        public string HastaOkul { get; set; } = string.Empty;
        public byte? HastaSinif { get; set; }
        public string HastaOkulBasarisi { get; set; } = string.Empty;
        public byte? HastaBoy { get; set; }
        public byte? HastaKilo { get; set; }
        public string HastaYonlendiren { get; set; } = string.Empty;
        public string HastaBasvuruNedeni { get; set; } = string.Empty;
        public DateTime HastaBasvuruTarihi { get; set; }
        public bool HastaAktif { get; set; }
    }
}
