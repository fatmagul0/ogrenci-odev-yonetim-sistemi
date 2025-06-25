using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http; // ✅ Bu satır eklendi
using OgrenciOdevYonetimSistemi.Models;
using System.Linq;

namespace OgrenciOdevYonetimSistemi.Controllers
{
    public class AccountController : Controller
    {
        private readonly UygulamaDbContext _context;

        public AccountController(UygulamaDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult OgrenciLogin()
        {
            var ogrenciNo = HttpContext.Session.GetString("OgrenciNo");
            if (!string.IsNullOrEmpty(ogrenciNo))
            {
                // Zaten oturum varsa, giriş ekranına gelinemez → panel'e at
                return RedirectToAction("Panel", "Ogrenci");
            }

            return View();
        }


        [HttpPost]
        public IActionResult OgrenciLogin(string OkulNo, string TCNo)
        {
            var ogrenci = _context.Ogrenciler
                           .FirstOrDefault(o => o.OkulNo == OkulNo && o.TCNo == TCNo);

            if (ogrenci != null)
            {
                HttpContext.Session.SetString("OgrenciNo", ogrenci.OkulNo);
                HttpContext.Session.SetString("AdSoyad", ogrenci.AdSoyad);
                return RedirectToAction("Panel", "Ogrenci");
            }

            ViewBag.Hata = "Böyle bir kayıt bulunamadı.";
            return View();
        }
        [HttpGet]
        public IActionResult OgretmenLogin()
        {
            var ogretmenAd = HttpContext.Session.GetString("OgretmenAd");
            if (!string.IsNullOrEmpty(ogretmenAd))
            {
                return RedirectToAction("Panel", "Ogretmen");
            }

            return View();
        }


        [HttpPost]
        public IActionResult OgretmenLogin(string KullaniciAdi, string Sifre)
        {
            var ogretmen = _context.Ogretmenler
                            .FirstOrDefault(o => o.KullaniciAdi == KullaniciAdi && o.Sifre == Sifre);

            if (ogretmen != null)
            {
                HttpContext.Session.SetString("OgretmenAd", ogretmen.AdSoyad);
                return RedirectToAction("Panel", "Ogretmen");
            }

            ViewBag.Hata = "Kullanıcı adı veya şifre yanlış.";
            return View();
        }

        [HttpGet]
        public IActionResult TumOgretmenler()
        {
            var ogretmenler = _context.Ogretmenler.ToList();
            return Json(ogretmenler);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // 🔐 Oturumu temizler
            return RedirectToAction("Index", "Home"); // veya Index
        }


    }
}
