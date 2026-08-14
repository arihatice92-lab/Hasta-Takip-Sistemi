using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HastaTakip.Entities
{
    public class Hasta
    {
        public int HastaID { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik No 11 haneli olmalıdır.")]
        public string HastaTC { get; set; } = string.Empty;
        public string? HastaDosyaNo { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string HastaAd { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string HastaSoyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string HastaTel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string HastaAdres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string HastaCinsiyet { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public DateTime HastaDogumTarihi { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string HastaOkul { get; set; } = string.Empty;
        public byte? HastaSinif { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string HastaOkulBasarisi { get; set; } = string.Empty;
        public byte? HastaBoy { get; set; }
        public byte? HastaKilo { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string HastaYonlendiren { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string HastaBasvuruNedeni { get; set; } = string.Empty;
        public DateTime HastaBasvuruTarihi { get; set; }
        public bool HastaAktif { get; set; }
        public Guid HastaGuid { get; set; }
    }
}
