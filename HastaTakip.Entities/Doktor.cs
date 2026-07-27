using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class Doktor
    {
        public short DoktorID { get; set; }
        public string DoktorSicilNo { get; set; } = string.Empty;
        public string DoktorAd { get; set; } = string.Empty;
        public string DoktorSoyad { get; set; } = string.Empty;
        public string DoktorTel { get; set; } = string.Empty;
        public string DoktorBrans { get; set; } = string.Empty;
        public DateTime? DoktorKurumBaslangicTarih { get; set; }
        public DateTime? DoktorKurumAyrilisTarih { get; set; }
    }
}
