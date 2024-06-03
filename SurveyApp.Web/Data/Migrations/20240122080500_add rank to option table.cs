using Microsoft.EntityFrameworkCore.Migrations;

namespace SurveyApp.Web.Data.Migrations
{
    public partial class addranktooptiontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "Options",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Options");
        }
    }
}
