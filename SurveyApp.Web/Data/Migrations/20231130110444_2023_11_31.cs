using Microsoft.EntityFrameworkCore.Migrations;

namespace SurveyApp.Web.Data.Migrations
{
    public partial class _2023_11_31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_FilledSurveyOption_FilledSurveys_FilledSurveyId",
            //    table: "FilledSurveyOption");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_FilledSurveyOption_Options_OptionId",
            //    table: "FilledSurveyOption");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_FilledSurveyOption",
            //    table: "FilledSurveyOption");

            //migrationBuilder.RenameTable(
            //    name: "FilledSurveyOption",
            //    newName: "FilledSurveyOptions");

            //migrationBuilder.RenameIndex(
            //    name: "IX_FilledSurveyOption_OptionId",
            //    table: "FilledSurveyOptions",
            //    newName: "IX_FilledSurveyOptions_OptionId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_FilledSurveyOption_FilledSurveyId",
            //    table: "FilledSurveyOptions",
            //    newName: "IX_FilledSurveyOptions_FilledSurveyId");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_FilledSurveyOptions",
            //    table: "FilledSurveyOptions",
            //    column: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_FilledSurveyOptions_FilledSurveys_FilledSurveyId",
            //    table: "FilledSurveyOptions",
            //    column: "FilledSurveyId",
            //    principalTable: "FilledSurveys",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_FilledSurveyOptions_Options_OptionId",
            //    table: "FilledSurveyOptions",
            //    column: "OptionId",
            //    principalTable: "Options",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_FilledSurveyOptions_FilledSurveys_FilledSurveyId",
            //    table: "FilledSurveyOptions");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_FilledSurveyOptions_Options_OptionId",
            //    table: "FilledSurveyOptions");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_FilledSurveyOptions",
            //    table: "FilledSurveyOptions");

            //migrationBuilder.RenameTable(
            //    name: "FilledSurveyOptions",
            //    newName: "FilledSurveyOption");

            //migrationBuilder.RenameIndex(
            //    name: "IX_FilledSurveyOptions_OptionId",
            //    table: "FilledSurveyOption",
            //    newName: "IX_FilledSurveyOption_OptionId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_FilledSurveyOptions_FilledSurveyId",
            //    table: "FilledSurveyOption",
            //    newName: "IX_FilledSurveyOption_FilledSurveyId");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_FilledSurveyOption",
            //    table: "FilledSurveyOption",
            //    column: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_FilledSurveyOption_FilledSurveys_FilledSurveyId",
            //    table: "FilledSurveyOption",
            //    column: "FilledSurveyId",
            //    principalTable: "FilledSurveys",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_FilledSurveyOption_Options_OptionId",
            //    table: "FilledSurveyOption",
            //    column: "OptionId",
            //    principalTable: "Options",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
