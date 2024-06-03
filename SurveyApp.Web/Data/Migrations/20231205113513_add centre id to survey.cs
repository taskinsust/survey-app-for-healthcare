using Microsoft.EntityFrameworkCore.Migrations;

namespace SurveyApp.Web.Data.Migrations
{
    public partial class addcentreidtosurvey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CentreId",
                table: "Surveys",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_CentreId",
                table: "Surveys",
                column: "CentreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Surveys_Center_CentreId",
                table: "Surveys",
                column: "CentreId",
                principalTable: "Center",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Surveys_Center_CentreId",
                table: "Surveys");

            migrationBuilder.DropIndex(
                name: "IX_Surveys_CentreId",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "CentreId",
                table: "Surveys");
        }
    }
}
