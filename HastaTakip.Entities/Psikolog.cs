using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HastaTakip.Entities
{
    public class Psikolog
    {
        public byte PsikologID { get; set; }
        public string? PsikologSicilNo { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string PsikologAd { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string PsikologSoyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string PsikologTel { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? PsikologKurumBaslangicTarih { get; set; }

        public DateTime? PsikologKurumAyrilisTarih { get; set; }
    }
}
