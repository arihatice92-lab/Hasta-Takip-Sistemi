using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class Tani
    {
        public short TaniID { get; set; }
        public string TaniAdi { get; set; } = string.Empty;
        public string? TaniKodu { get; set; }
        public bool TaniAktif { get; set; }
    }
}
