using Microsoft.EntityFrameworkCore.Migrations;

namespace SurveyApp.Web.Data.Migrations
{
    public partial class _2023_11_27_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "FilledSurveyOption",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "FilledSurveyOption");
        }
    }
}
