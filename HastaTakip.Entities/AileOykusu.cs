using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class AileOykusu
    {
        public int AileOykuID { get; set; }
        public string HastaTC { get; set; } = string.Empty;
        public string? AnneBabaninKisiselOykusu { get; set; }
        public string? AnneBabaninEvlilikOykusu { get; set; } 
        public string? AileOzellikleri { get; set; } 
        public string? AnneBabaKardesler { get; set; }
        public int? SonGuncelleyenKullaniciID { get; set; }
        public DateTime? SonGuncellemeTarihi { get; set; }
    }
}
