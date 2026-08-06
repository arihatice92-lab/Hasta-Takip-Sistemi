using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class RandevuTarihi
    {
        public int RandevuTarihID { get; set; }
        public string HastaTC { get; set; } = string.Empty;
        public short DoktorID { get; set; }
        public byte SaatID { get; set; }
        public DateTime RandevuTarih { get; set; }
        public DateTime RandevuOlusturmaTarihi { get; set; }
        public string RandevuDurum { get; set; } = string.Empty;
        public DateTime? HastaGelisZamani { get; set; }
        public DateTime? MuayeneBaslangicZamani { get; set; }
    }
}
