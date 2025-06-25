using System.ComponentModel.DataAnnotations;

namespace OgrenciOdevYonetimSistemi.Models
{
    public class Ogrenci
    {
        [Key]
        public int OgrenciId { get; set; }

        [Required]
        public string OkulNo { get; set; }

        [Required]
        public string TCNo { get; set; }

        [Required]
        public string AdSoyad { get; set; }

        public string Sinif { get; set; }
    }
}
