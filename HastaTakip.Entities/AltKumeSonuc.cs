using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class AltKumeSonuc
    {
        public int AltKumeSonucID { get; set; }
        public string HastaTC { get; set; } = string.Empty;
        public byte AltKumeID { get; set; }
        public int? TestSonucID { get; set; }
        public string? AltKumeSonucDeger { get; set; }
        public string? AltKumeYorum { get; set; }
    }
}
