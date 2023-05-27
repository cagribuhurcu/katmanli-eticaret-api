using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StokProject.Repositories.Migrations
{
    public partial class stock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Stock",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "Stock",
                table: "Products",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
