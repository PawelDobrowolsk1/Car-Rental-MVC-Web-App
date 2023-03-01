using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Car_Rental_MVC.Migrations
{
    public partial class UpdatingCarTabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Transmisson",
                table: "Cars",
                newName: "Transmission");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Transmission",
                table: "Cars",
                newName: "Transmisson");
        }
    }
}
