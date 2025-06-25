using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//Bu model, bir öğrencinin ödev teslimini temsil eder:
namespace OgrenciOdevYonetimSistemi.Models
{
    //bu sınıf, OdevTeslim tablosunun veritabanı modelini (entity) temsil eder
    //Yani Entity Framework Core, bu sınıfa bakarak OdevTeslimler tablosunu oluşturur.
    public class OdevTeslim
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OgrenciId { get; set; }

        [ForeignKey("OgrenciId")]
        public Ogrenci Ogrenci { get; set; }

        [Required]
        [MaxLength(255)]
        public string DosyaAdi { get; set; }

        [Required]
        public DateTime YuklenmeTarihi { get; set; }
    }
}
