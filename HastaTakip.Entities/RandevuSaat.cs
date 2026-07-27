using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class RandevuSaat
    {
        public byte SaatID { get; set; }
        public TimeSpan RandevuBaslangicSaat { get; set; }
        public TimeSpan RandevuBitisSaat { get; set; }
    }
}
