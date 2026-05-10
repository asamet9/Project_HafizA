using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_HafizA.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class LiderlikTablosu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Main",
                columns: table => new
                {
                    Ayet_kodu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sure_kodu = table.Column<int>(type: "int", nullable: false),
                    Ayet_no = table.Column<int>(type: "int", nullable: false),
                    Metin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Secde = table.Column<bool>(type: "bit", nullable: false),
                    Uzun = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Main", x => x.Ayet_kodu);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Main");
        }
    }
}
