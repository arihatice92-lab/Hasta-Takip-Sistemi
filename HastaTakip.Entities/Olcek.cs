using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class Olcek
    {
        public byte OlcekID { get; set; }
        public string OlcekAdi { get; set; } = string.Empty;
        public string? OlcekBilgi { get; set; }
    }
}
