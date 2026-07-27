using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class AileOykusu
    {
        public int AileOykuID { get; set; }
        public string HastaTC { get; set; } = string.Empty;
        public string AnneBabaninKisiselOykusu { get; set; } = string.Empty;
        public string AnneBabaninEvlilikOykusu { get; set; } = string.Empty;
        public string AileOzellikleri { get; set; } = string.Empty;
        public string AnneBabaKardesler { get; set; } = string.Empty;
    }
}
