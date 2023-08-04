using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvertisementApp.DataAccess.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkExperiene",
                table: "AdvertisementAppUser",
                newName: "WorkExperience");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkExperience",
                table: "AdvertisementAppUser",
                newName: "WorkExperiene");
        }
    }
}
