using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class OlcekSonuc
    {
        public int OlcekSonucID { get; set; }
        public string HastaTC { get; set; } = string.Empty;
        public short DoktorID { get; set; }
        public byte OlcekID { get; set; }
        public DateTime OlcekTarih { get; set; }
        public byte? OlcekPuan { get; set; }
        public string? OlcekYorum { get; set; }
        public string? OlcekUygulanan { get; set; }
    }
}
