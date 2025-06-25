using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OgrenciOdevYonetimSistemi.Migrations
{
    /// <inheritdoc />
    public partial class IlkOlusturma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ogrenciler",
                columns: table => new
                {
                    OgrenciId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OkulNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TCNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sinif = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ogrenciler", x => x.OgrenciId);
                });

            migrationBuilder.CreateTable(
                name: "OgrenciNotlari",
                columns: table => new
                {
                    NotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OgrenciId = table.Column<int>(type: "int", nullable: false),
                    Vize = table.Column<int>(type: "int", nullable: true),
                    Final = table.Column<int>(type: "int", nullable: true),
                    Proje = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OgrenciNotlari", x => x.NotId);
                    table.ForeignKey(
                        name: "FK_OgrenciNotlari_Ogrenciler_OgrenciId",
                        column: x => x.OgrenciId,
                        principalTable: "Ogrenciler",
                        principalColumn: "OgrenciId",
                        onDelete: ReferentialAction.Cascade);
                });

            //Yabancı anahtar kolonu için indeks oluşturuluyor.
            migrationBuilder.CreateIndex(
                name: "IX_OgrenciNotlari_OgrenciId",
                table: "OgrenciNotlari",
                column: "OgrenciId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OgrenciNotlari");

            migrationBuilder.DropTable(
                name: "Ogrenciler");

            //Eğer migration geri alınırsa (örneğin dotnet ef database update LastMigrationName ile),
           //bu tablolar silinir.
        }
    }
}

//Bu migration dosyası, veritabanında:
//Ogrenciler tablosunu(temel öğrenci bilgileri),
//OgrenciNotlari tablosunu(not bilgileri ve öğrenciye bağlanması),
//oluşturur.
