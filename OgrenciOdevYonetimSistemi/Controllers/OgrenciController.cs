using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OgrenciOdevYonetimSistemi.Models;
using System.Linq;
using OgrenciOdevYonetimSistemi.Filters;

namespace OgrenciOdevYonetimSistemi.Controllers
{
    [LoginCheck] // ✅ Giriş kontrol filtresi
    public class OgrenciController : Controller
    {
        private readonly UygulamaDbContext _context;

        public OgrenciController(UygulamaDbContext context)
        {
            _context = context;
        }

        // 🏠 PANEL SAYFASI
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Panel()
        {
            var okulNo = HttpContext.Session.GetString("OgrenciNo");

            if (string.IsNullOrEmpty(okulNo))
                return RedirectToAction("OgrenciLogin", "Account");

            ViewBag.AdSoyad = HttpContext.Session.GetString("AdSoyad");
            ViewBag.OkulNo = okulNo;

            return View();
        }

        // 📘 ÖDEVLERİM
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Odevlerim()
        {
            var okulNo = HttpContext.Session.GetString("OgrenciNo");
            if (string.IsNullOrEmpty(okulNo))
                return RedirectToAction("OgrenciLogin", "Account");

            var ogrenci = _context.Ogrenciler.FirstOrDefault(o => o.OkulNo == okulNo);
            if (ogrenci == null)
                return RedirectToAction("OgrenciLogin", "Account");

            var odevler = _context.OdevTeslimler
                            .Where(o => o.OgrenciId == ogrenci.OgrenciId)
                            .ToList();

            return View(odevler);
        }

        // 📝 NOTLARIM
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Notlarim()
        {
            var okulNo = HttpContext.Session.GetString("OgrenciNo");
            if (string.IsNullOrEmpty(okulNo))
                return RedirectToAction("OgrenciLogin", "Account");

            var ogrenci = _context.Ogrenciler.FirstOrDefault(o => o.OkulNo == okulNo);
            if (ogrenci == null)
                return RedirectToAction("OgrenciLogin", "Account");

            var notlar = _context.OgrenciNotlari
                                 .FirstOrDefault(n => n.OgrenciId == ogrenci.OgrenciId);

            double? ortalama = null;
            if (notlar != null)
            {
                var notList = new List<int?> { notlar.Vize, notlar.Final, notlar.Proje }
                                .Where(n => n.HasValue)
                                .Select(n => n.Value);

                if (notList.Any())
                    ortalama = Math.Round(notList.Average(), 2);
            }

            ViewBag.Ortalama = ortalama;

            string basariDurumu = "Yetersiz";
            string renk = "danger";

            if (ortalama >= 85) { basariDurumu = "Pekiyi"; renk = "success"; }
            else if (ortalama >= 70) { basariDurumu = "İyi"; renk = "info"; }
            else if (ortalama >= 50) { basariDurumu = "Orta"; renk = "warning"; }

            ViewBag.BasariDurumu = basariDurumu;
            ViewBag.Renk = renk;

            return View(notlar);
        }

        // 📈 KOTA BİLGİM
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult KotaBilgim()
        {
            var okulNo = HttpContext.Session.GetString("OgrenciNo");
            if (string.IsNullOrEmpty(okulNo))
                return RedirectToAction("OgrenciLogin", "Account");

            var ogrenci = _context.Ogrenciler.FirstOrDefault(o => o.OkulNo == okulNo);
            if (ogrenci == null)
                return RedirectToAction("OgrenciLogin", "Account");

            var yapilanOdevSayisi = _context.OdevTeslimler.Count(x => x.OgrenciId == ogrenci.OgrenciId);

            var model = new OgrenciKota
            {
                OgrenciNo = ogrenci.OkulNo,
                AdSoyad = ogrenci.AdSoyad,
                Sinif = ogrenci.Sinif,
                MaksimumOdevSayisi = 5,
                YapilanOdevSayisi = yapilanOdevSayisi
            };

            return View(model);
        }
    }
}
