using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OgrenciOdevYonetimSistemi.Models
{
    public class OdevYukleViewModel
    {
        [Required(ErrorMessage = "Dosya seçilmelidir.")]
        public IFormFile Dosya { get; set; }
    }
}
