using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class HastaTani
    {
        public int HastaTaniID { get; set; }
        public string HastaTC { get; set; } = string.Empty;
        public short DoktorID { get; set; }
        public short TaniID { get; set; }
        public DateTime TaniTarih { get; set; }
        public string? MentalDurumMuayenesi { get; set; }
    }
}
