using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Car_Rental_MVC.Migrations
{
    public partial class DropUniqueConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RentInfo_CarId",
                table: "RentInfo");

            migrationBuilder.DropIndex(
                name: "IX_RentInfo_UserId",
                table: "RentInfo");

            migrationBuilder.CreateIndex(
                name: "IX_RentInfo_CarId",
                table: "RentInfo",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_RentInfo_UserId",
                table: "RentInfo",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RentInfo_CarId",
                table: "RentInfo");

            migrationBuilder.DropIndex(
                name: "IX_RentInfo_UserId",
                table: "RentInfo");

            migrationBuilder.CreateIndex(
                name: "IX_RentInfo_CarId",
                table: "RentInfo",
                column: "CarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentInfo_UserId",
                table: "RentInfo",
                column: "UserId",
                unique: true);
        }
    }
}
