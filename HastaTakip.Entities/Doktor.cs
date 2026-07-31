using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HastaTakip.Entities
{
    public class Doktor
    {
        public short DoktorID { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string DoktorSicilNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string DoktorAd { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string DoktorSoyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string DoktorTel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string DoktorBrans { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? DoktorKurumBaslangicTarih { get; set; }
        public DateTime? DoktorKurumAyrilisTarih { get; set; }
    }
}
