using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class DoktorTakvimSlotu
    {
        public byte SaatID { get; set; }
        public TimeSpan BaslangicSaat { get; set; }
        public TimeSpan BitisSaat { get; set; }
        public int? RandevuTarihID { get; set; }
        public string? HastaTC { get; set; }
        public string? RandevuDurum { get; set; }

        public bool Dolu => RandevuTarihID.HasValue;
    }
}
