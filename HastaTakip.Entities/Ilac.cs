using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class Ilac
    {
        public short IlacID { get; set; }
        public string IlacAdi { get; set; } = string.Empty;
        public string IlacEtkenMadde { get; set; } = string.Empty;
    }
}
