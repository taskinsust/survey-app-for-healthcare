using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyApp.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class addshorttitletoquestiontabletogeneartequery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortTitle",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortTitle",
                table: "Questions");
        }
    }
}
