using Microsoft.EntityFrameworkCore.Migrations;

namespace SurveyApp.Web.Data.Migrations
{
    public partial class _2023_11_30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "QuestionId",
            //    table: "FilledSurveyOption",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_FilledSurveyOption_QuestionId",
            //    table: "FilledSurveyOption",
            //    column: "QuestionId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_FilledSurveyOption_Questions_QuestionId",
            //    table: "FilledSurveyOption",
            //    column: "QuestionId",
            //    principalTable: "Questions",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_FilledSurveyOption_Questions_QuestionId",
            //    table: "FilledSurveyOption");

            //migrationBuilder.DropIndex(
            //    name: "IX_FilledSurveyOption_QuestionId",
            //    table: "FilledSurveyOption");

            //migrationBuilder.DropColumn(
            //    name: "QuestionId",
            //    table: "FilledSurveyOption");
        }
    }
}
