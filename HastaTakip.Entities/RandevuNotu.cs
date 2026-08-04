using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class RandevuNotu
    {
        public short RandevuNotID { get; set; }
        public string HastaTC { get; set; } = string.Empty;
        public short DoktorID { get; set; }
        public int RandevuTarihID { get; set; }
        public string? GorusmeTipi { get; set; }
        public string? GorusmeNotu { get; set; }
        public DateTime? SonrakiRandevuTarihi { get; set; }
    }
}
