using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OgrenciOdevYonetimSistemi.Models;
using OgrenciOdevYonetimSistemi.Filters;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace OgrenciOdevYonetimSistemi.Controllers
{
    [LoginCheck]
    public class OgretmenController : Controller
    {
        private readonly UygulamaDbContext _context;

        public OgretmenController(UygulamaDbContext context)
        {
            _context = context;
        }

        public IActionResult Panel() => View();

        public IActionResult OgrenciListesi()
        {
            var ogrenciler = _context.Ogrenciler.Select(o => new OgrenciViewModel
            {
                OgrenciNo = o.OkulNo,
                AdSoyad = o.AdSoyad,
                Sinif = o.Sinif,
                Ortalama = 0
            }).ToList();

            return View(ogrenciler);
        }

        public IActionResult KotaYonetimi()
        {
            var kotalar = _context.Ogrenciler.Select(o => new OgrenciKota
            {
                OgrenciNo = o.OkulNo,
                AdSoyad = o.AdSoyad,
                Sinif = o.Sinif,
                MaksimumOdevSayisi = 5,
                YapilanOdevSayisi = _context.OdevTeslimler.Count(x => x.OgrenciId == o.OgrenciId)
            }).ToList();

            return View(kotalar);
        }

        [HttpGet]
        public IActionResult OdevVer()
        {
            ViewBag.Siniflar = _context.Ogrenciler.Select(o => o.Sinif).Distinct().OrderBy(s => s).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> OdevVer(OdevVerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Siniflar = _context.Ogrenciler.Select(o => o.Sinif).Distinct().ToList();
                return View(model);
            }

            List<string> hedefOgrenciler = new();

            if (!string.IsNullOrEmpty(model.OgrenciNo)) // bireysel
            {
                hedefOgrenciler.Add(model.OgrenciNo);
            }
            else if (model.SeciliOgrenciler != null && model.SeciliOgrenciler.Any()) // çoklu
            {
                hedefOgrenciler = model.SeciliOgrenciler;
            }
            else
            {
                TempData["Uyari"] = "Hiçbir öğrenci seçilmedi."; // ✅ DEĞİŞTİRİLDİ
                return RedirectToAction("OdevVer");              // ✅ DEĞİŞTİRİLDİ
            }

            string dosyaAdi = Path.GetFileName(model.EkDosya.FileName);
            string klasorYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "verilenOdevDosyalari");
            if (!Directory.Exists(klasorYolu))
                Directory.CreateDirectory(klasorYolu);

            string tamYol = Path.Combine(klasorYolu, dosyaAdi);
            using (var stream = new FileStream(tamYol, FileMode.Create))
            {
                await model.EkDosya.CopyToAsync(stream);
            }

            foreach (var ogrNo in hedefOgrenciler)
            {
                var ogr = _context.Ogrenciler.FirstOrDefault(o => o.OkulNo == ogrNo);
                if (ogr == null) continue;
                int mevcut = _context.OdevTeslimler.Count(x => x.OgrenciId == ogr.OgrenciId);
                if (mevcut >= 5) continue;

                _context.OdevTeslimler.Add(new OdevTeslim
                {
                    OgrenciId = ogr.OgrenciId,
                    DosyaAdi = dosyaAdi,
                    YuklenmeTarihi = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();

            TempData["Basarili"] = "Ödev başarıyla verildi!"; // ✅ ALERT MESAJI
            return RedirectToAction("OdevVer");               // ✅ REDIRECT ŞART
        }


        [HttpGet]
        public IActionResult OdevVerOgrenciSec()
        {
            var siniflar = _context.Ogrenciler
                            .Select(o => o.Sinif)
                            .Distinct()
                            .OrderBy(s => s)
                            .ToList();

            ViewBag.Siniflar = siniflar;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> OdevVerOgrenciSec(string[] secilenOgrenciler, string konu, DateTime teslimTarihi, IFormFile ekDosya)
        {
            if (secilenOgrenciler == null || secilenOgrenciler.Length == 0)
            {
                TempData["Uyari"] = "Lütfen en az bir öğrenci seçiniz.";
                return RedirectToAction("OdevVerOgrenciSec");
            }

            if (string.IsNullOrEmpty(konu) || ekDosya == null || teslimTarihi == default)
            {
                TempData["Uyari"] = "Tüm alanlar doldurulmalıdır.";
                return RedirectToAction("OdevVerOgrenciSec");
            }

            var dosyaAdi = Path.GetFileName(ekDosya.FileName);
            var klasorYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "verilenOdevDosyalari");
            if (!Directory.Exists(klasorYolu))
                Directory.CreateDirectory(klasorYolu);

            var tamYol = Path.Combine(klasorYolu, dosyaAdi);
            using (var stream = new FileStream(tamYol, FileMode.Create))
            {
                await ekDosya.CopyToAsync(stream);
            }

            foreach (var no in secilenOgrenciler)
            {
                var ogrenci = _context.Ogrenciler.FirstOrDefault(o => o.OkulNo == no);
                if (ogrenci == null) continue;

                _context.OdevTeslimler.Add(new OdevTeslim
                {
                    OgrenciId = ogrenci.OgrenciId,
                    DosyaAdi = dosyaAdi,
                    YuklenmeTarihi = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();
            TempData["Basarili"] = "Seçilen öğrencilere ödev ataması yapıldı.";
            return RedirectToAction("OdevVerOgrenciSec");
        }


        public IActionResult VerilenOdevler()
        {
            var odevler = _context.OdevTeslimler
                .Include(o => o.Ogrenci) // 👈 Ödevle ilişkili öğrenciyi dahil ediyoruz
                .ToList();

            return View(odevler); // 👈 Bu view artık güncel VerilenOdevler.cshtml'e uygun
        }


        public IActionResult BasariGrafik(string ogrenciNo)
        {
            var ogrenci = _context.Ogrenciler.FirstOrDefault(o => o.OkulNo == ogrenciNo);
            if (ogrenci == null) return NotFound();

            var not = _context.OgrenciNotlari.FirstOrDefault(n => n.OgrenciId == ogrenci.OgrenciId);
            double sinavOrt = (not?.Vize ?? 0 + not?.Final ?? 0 + not?.Proje ?? 0) / 3.0;
            double odevOrani = (_context.OdevTeslimler.Count(x => x.OgrenciId == ogrenci.OgrenciId) / 5.0) * 100;

            ViewBag.OgrenciAd = ogrenci.AdSoyad;
            ViewBag.Sinif = ogrenci.Sinif;
            ViewBag.SinavOrtalama = sinavOrt;
            ViewBag.OdevOrani = odevOrani;

            return View();
        }

        [HttpGet]
        public IActionResult NotGir()
        {
            var ogrenciler = _context.Ogrenciler.Select(o => new NotGirViewModel
            {
                OgrenciId = o.OgrenciId,
                AdSoyad = o.AdSoyad
            }).ToList();

            return View(ogrenciler);
        }

        [HttpPost]
        public IActionResult NotGir(List<NotGirViewModel> girilenNotlar)
        {
            foreach (var item in girilenNotlar)
            {
                var mevcut = _context.OgrenciNotlari.FirstOrDefault(n => n.OgrenciId == item.OgrenciId);
                if (mevcut == null)
                {
                    _context.OgrenciNotlari.Add(new OgrenciNot
                    {
                        OgrenciId = item.OgrenciId,
                        Vize = item.Vize,
                        Final = item.Final,
                        Proje = item.Proje
                    });
                }
                else
                {
                    mevcut.Vize = item.Vize;
                    mevcut.Final = item.Final;
                    mevcut.Proje = item.Proje;
                }
            } 

            _context.SaveChanges();
            ViewBag.Basarili = "Notlar başarıyla kaydedildi.";
            return RedirectToAction("Panel");
        }

        [HttpGet]
        public IActionResult OgrencileriGetir(string sinif)
        {
            var ogrenciler = _context.Ogrenciler
                .Where(o => o.Sinif == sinif)
                .Select(o => new { o.OkulNo, o.AdSoyad })
                .ToList();

            return Json(ogrenciler);
        }
    }
}
