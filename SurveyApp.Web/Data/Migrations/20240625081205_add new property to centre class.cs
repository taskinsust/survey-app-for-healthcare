using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyApp.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class addnewpropertytocentreclass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ButtonClass",
                table: "Center",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconClass",
                table: "Center",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextClass",
                table: "Center",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ButtonClass",
                table: "Center");

            migrationBuilder.DropColumn(
                name: "IconClass",
                table: "Center");

            migrationBuilder.DropColumn(
                name: "TextClass",
                table: "Center");
        }
    }
}
