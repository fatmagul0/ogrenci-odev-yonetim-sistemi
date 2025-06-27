using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OgrenciOdevYonetimSistemi.Models
{
    public class OgrenciNot
    {
        [Key]
        public int NotId { get; set; }

        [ForeignKey("Ogrenci")]
        public int OgrenciId { get; set; }
        public Ogrenci Ogrenci { get; set; }

        [ForeignKey("Ogretmen")]
        public int OgretmenId { get; set; }
        public Ogretmen Ogretmen { get; set; }

        public int? Vize { get; set; }
        public int? Final { get; set; }
        public int? Proje { get; set; }
    }
}
