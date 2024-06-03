using Microsoft.EntityFrameworkCore.Migrations;

namespace SurveyApp.Web.Data.Migrations
{
    public partial class _2023_11_27 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "FilledSurveys");

            migrationBuilder.AddColumn<int>(
                name: "CenterId",
                table: "FilledSurveys",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatientId",
                table: "FilledSurveys",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "FilledSurveyOption",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Center",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Contact = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Center", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilledSurveys_CenterId",
                table: "FilledSurveys",
                column: "CenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilledSurveys_Center_CenterId",
                table: "FilledSurveys",
                column: "CenterId",
                principalTable: "Center",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilledSurveys_Center_CenterId",
                table: "FilledSurveys");

            migrationBuilder.DropTable(
                name: "Center");

            migrationBuilder.DropIndex(
                name: "IX_FilledSurveys_CenterId",
                table: "FilledSurveys");

            migrationBuilder.DropColumn(
                name: "CenterId",
                table: "FilledSurveys");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "FilledSurveys");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "FilledSurveyOption");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "FilledSurveys",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
