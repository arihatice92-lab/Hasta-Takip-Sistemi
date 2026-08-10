using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class KayitNotu
    {
        public int NotID { get; set; }
        public string KayitTuru { get; set; } = string.Empty;
        public int KayitID { get; set; }
        public int KullaniciID { get; set; }
        public string NotMetni { get; set; } = string.Empty;
        public DateTime NotTarihi { get; set; }
    }
}
