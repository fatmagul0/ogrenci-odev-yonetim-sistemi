using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OgrenciOdevYonetimSistemi.Migrations
{
    /// <inheritdoc />
    public partial class OdevTeslimTablosu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OdevTeslimler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OgrenciNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DosyaAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeslimTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdevTeslimler", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OdevTeslimler");
        }
    }
}

//Bu migration, öğrencilerin teslim ettiği ödevlerin:
//hangi öğrenci tarafından,
//hangi dosya adıyla,
//hangi tarihte teslim edildiğini
//kaydedeceği bir veritabanı tablosu oluşturur.