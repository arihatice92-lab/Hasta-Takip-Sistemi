using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class TestSonuc
    {
        public int TestSonucID { get; set; }
        public string HastaTC { get; set; } = string.Empty;
        public byte PsikologID { get; set; }
        public byte TestID { get; set; }
        public DateTime TestTarih { get; set; }
        public string SonucDegeri { get; set; } = string.Empty;
        public string TestDegerlendirme { get; set; } = string.Empty;
    }
}
