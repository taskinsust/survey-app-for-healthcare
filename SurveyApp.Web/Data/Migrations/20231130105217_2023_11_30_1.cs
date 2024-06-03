using Microsoft.EntityFrameworkCore.Migrations;

namespace SurveyApp.Web.Data.Migrations
{
    public partial class _2023_11_30_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_FilledSurveyOption_Questions_QuestionId",
            //    table: "FilledSurveyOption");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_FilledSurveyOption",
            //    table: "FilledSurveyOption");

            //migrationBuilder.DropIndex(
            //    name: "IX_FilledSurveyOption_QuestionId",
            //    table: "FilledSurveyOption");

            //migrationBuilder.DropColumn(
            //    name: "QuestionId",
            //    table: "FilledSurveyOption");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "FilledSurveyOption",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .Annotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_FilledSurveyOption",
            //    table: "FilledSurveyOption",
            //    column: "Id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FilledSurveyOption_FilledSurveyId",
            //    table: "FilledSurveyOption",
            //    column: "FilledSurveyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_FilledSurveyOption",
            //    table: "FilledSurveyOption");

            //migrationBuilder.DropIndex(
            //    name: "IX_FilledSurveyOption_FilledSurveyId",
            //    table: "FilledSurveyOption");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "FilledSurveyOption",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .OldAnnotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AddColumn<int>(
            //    name: "QuestionId",
            //    table: "FilledSurveyOption",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_FilledSurveyOption",
            //    table: "FilledSurveyOption",
            //    columns: new[] { "FilledSurveyId", "OptionId" });

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
    }
}
