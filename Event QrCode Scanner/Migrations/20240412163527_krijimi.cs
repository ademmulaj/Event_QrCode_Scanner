using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Event_QrCode_Scanner.Migrations
{
    /// <inheritdoc />
    public partial class krijimi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QrCodeData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Koha_skanimit = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QrCode_Data = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrCodeData", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QrCodeData");
        }
    }
}
