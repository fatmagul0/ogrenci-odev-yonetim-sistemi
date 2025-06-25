using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OgrenciOdevYonetimSistemi.Migrations
{
    /// <inheritdoc />
    public partial class OdevTeslimKolonGuncelleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OgrenciNo",
                table: "OdevTeslimler");
            //artık öğrenciyle string OgrenciNo üzerinden değil, foreign key ile bağlantı kurulacak.

            migrationBuilder.RenameColumn(
                name: "TeslimTarihi",
                table: "OdevTeslimler",
                newName: "YuklenmeTarihi");
            //kolonadı yüklenmeTarihi olarak değiştirildi

            migrationBuilder.AlterColumn<string>(
                name: "DosyaAdi",
                table: "OdevTeslimler",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
            //dosyaAdı sütununa uzunluk kısıtı eklendi. 255 karakter olabilir.

            migrationBuilder.AddColumn<int>(
                name: "OgrenciId",
                table: "OdevTeslimler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            //OgrenciNo string yerine, artık ilişkisel ve güvenli şekilde OgrenciId kullanılacak.

            migrationBuilder.CreateIndex(
                name: "IX_OdevTeslimler_OgrenciId",
                table: "OdevTeslimler",
                column: "OgrenciId");

            migrationBuilder.AddForeignKey(
                name: "FK_OdevTeslimler_Ogrenciler_OgrenciId",
                table: "OdevTeslimler",
                column: "OgrenciId",
                principalTable: "Ogrenciler",
                principalColumn: "OgrenciId",
                onDelete: ReferentialAction.Cascade);
            //OdevTeslimler artık Ogrenciler tablosuna bağlı.

        }

        /// <inheritdoc />
        /// buradaki down() metodu yapılan her iişlemi geri alıyor. ÖRNKE:
       
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OdevTeslimler_Ogrenciler_OgrenciId",
                table: "OdevTeslimler");//ForeignKey kaldırılır.

            migrationBuilder.DropIndex(
                name: "IX_OdevTeslimler_OgrenciId",
                table: "OdevTeslimler");//Index silinir


            migrationBuilder.DropColumn(//OgrenciId kolonu silinir
                name: "OgrenciId",
                table: "OdevTeslimler");

            migrationBuilder.RenameColumn(
                name: "YuklenmeTarihi",
                table: "OdevTeslimler",
                newName: "TeslimTarihi");
            //YuklenmeTarihi tekrar TeslimTarihi olur


            migrationBuilder.AlterColumn<string>(
                name: "DosyaAdi",
                table: "OdevTeslimler",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);
            //DosyaAdi eski haline döner

            migrationBuilder.AddColumn<string>(
                name: "OgrenciNo",
                table: "OdevTeslimler",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
            //OgrenciNo kolonu geri eklenir
        }
    }
}
