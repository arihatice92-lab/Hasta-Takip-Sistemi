using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class Psikolog
    {
        public byte PsikologID { get; set; }
        public string PsikologSicilNo { get; set; } = string.Empty;
        public string PsikologAd { get; set; } = string.Empty;
        public string PsikologSoyad { get; set; } = string.Empty;
        public string PsikologTel { get; set; } = string.Empty;
    }
}
