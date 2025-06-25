using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OgrenciOdevYonetimSistemi.Models
{
    public class OdevVerViewModel
    {
        [Required(ErrorMessage = "Sınıf seçimi zorunludur.")]
        public string Sinif { get; set; }

        [Required(ErrorMessage = "Konu boş bırakılamaz.")]
        public string Konu { get; set; }

        [Required(ErrorMessage = "Teslim tarihi seçilmelidir.")]
        [DataType(DataType.Date)]
        public DateTime TeslimTarihi { get; set; }

        [Required(ErrorMessage = "Dosya yüklenmelidir.")]
        public IFormFile EkDosya { get; set; }

        public string OgrenciNo { get; set; } // Bireysel ödev atamak için

        public List<string> SeciliOgrenciler { get; set; } // Toplu atama için
    }
}
