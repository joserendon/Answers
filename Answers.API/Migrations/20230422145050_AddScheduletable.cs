using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Answers.API.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemporalSchedules_AspNetUsers_UserId1",
                table: "TemporalSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TemporalSchedules_Questionnaires_QuestionnaireId1",
                table: "TemporalSchedules");

            migrationBuilder.DropIndex(
                name: "IX_TemporalSchedules_QuestionnaireId1",
                table: "TemporalSchedules");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "TemporalSchedules");

            migrationBuilder.DropColumn(
                name: "QuestionnaireId",
                table: "TemporalSchedules");

            migrationBuilder.DropColumn(
                name: "QuestionnaireId1",
                table: "TemporalSchedules");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "TemporalSchedules");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "TemporalSchedules",
                newName: "QuestionnaireIdId");

            migrationBuilder.RenameIndex(
                name: "IX_TemporalSchedules_UserId1",
                table: "TemporalSchedules",
                newName: "IX_TemporalSchedules_QuestionnaireIdId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TemporalSchedules",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "TemporalSchedules",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "EndDate",
                table: "TemporalSchedules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TemporalSchedules",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TemporalSchedules",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StardDate",
                table: "TemporalSchedules",
                type: "int",
                maxLength: 2000,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TemporalSchedules_UserId",
                table: "TemporalSchedules",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TemporalSchedules_AspNetUsers_UserId",
                table: "TemporalSchedules",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemporalSchedules_Questionnaires_QuestionnaireIdId",
                table: "TemporalSchedules",
                column: "QuestionnaireIdId",
                principalTable: "Questionnaires",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemporalSchedules_AspNetUsers_UserId",
                table: "TemporalSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TemporalSchedules_Questionnaires_QuestionnaireIdId",
                table: "TemporalSchedules");

            migrationBuilder.DropIndex(
                name: "IX_TemporalSchedules_UserId",
                table: "TemporalSchedules");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "TemporalSchedules");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TemporalSchedules");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TemporalSchedules");

            migrationBuilder.DropColumn(
                name: "StardDate",
                table: "TemporalSchedules");

            migrationBuilder.RenameColumn(
                name: "QuestionnaireIdId",
                table: "TemporalSchedules",
                newName: "UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_TemporalSchedules_QuestionnaireIdId",
                table: "TemporalSchedules",
                newName: "IX_TemporalSchedules_UserId1");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TemporalSchedules",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TemporalSchedules",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<float>(
                name: "Quantity",
                table: "TemporalSchedules",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "QuestionnaireId",
                table: "TemporalSchedules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionnaireId1",
                table: "TemporalSchedules",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "TemporalSchedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TemporalSchedules_QuestionnaireId1",
                table: "TemporalSchedules",
                column: "QuestionnaireId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TemporalSchedules_AspNetUsers_UserId1",
                table: "TemporalSchedules",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemporalSchedules_Questionnaires_QuestionnaireId1",
                table: "TemporalSchedules",
                column: "QuestionnaireId1",
                principalTable: "Questionnaires",
                principalColumn: "Id");
        }
    }
}
