using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace OgrenciOdevYonetimSistemi.Models
{
    public class UygulamaDbContext : DbContext
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options)
            : base(options)
        {
        }

        // ✅ Buraya tabloları temsil eden DbSet'ler yazılır
        public DbSet<Ogrenci> Ogrenciler { get; set; }
        public DbSet<OgrenciNot> OgrenciNotlari { get; set; }
        public DbSet<OdevTeslim> OdevTeslimler { get; set; }
        public DbSet<Ogretmen> Ogretmenler { get; set; }

    }
}
