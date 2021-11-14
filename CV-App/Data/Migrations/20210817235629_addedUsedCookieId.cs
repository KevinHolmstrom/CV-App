using Microsoft.EntityFrameworkCore.Migrations;

namespace CV_App.Data.Migrations
{
    public partial class addedUsedCookieId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserCookieId",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserCookieId",
                table: "Resumes");
        }
    }
}
