using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class HastaTedavi
    {
        public int TedaviID { get; set; }
        public string HastaTC { get; set; } = string.Empty;
        public short DoktorID { get; set; }
        public short IlacID { get; set; }
        public string IlacDozu { get; set; } = string.Empty;
        public DateTime? IlacBaslangicTarihi { get; set; }
        public DateTime? IlacBitisTarihi { get; set; }
        public string IlacYanEtkiler { get; set; } = string.Empty;
        public string TedaviNotlari { get; set; } = string.Empty;
    }
}
