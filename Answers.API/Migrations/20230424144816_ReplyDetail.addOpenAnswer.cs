using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Answers.API.Migrations
{
    /// <inheritdoc />
    public partial class ReplyDetailaddOpenAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReplyDetails_ReplyId_AnswerId",
                table: "ReplyDetails");

            migrationBuilder.AlterColumn<Guid>(
                name: "AnswerId",
                table: "ReplyDetails",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "OpenAnswer",
                table: "ReplyDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReplyDetails_ReplyId_AnswerId",
                table: "ReplyDetails",
                columns: new[] { "ReplyId", "AnswerId" },
                unique: true,
                filter: "[AnswerId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReplyDetails_ReplyId_AnswerId",
                table: "ReplyDetails");

            migrationBuilder.DropColumn(
                name: "OpenAnswer",
                table: "ReplyDetails");

            migrationBuilder.AlterColumn<Guid>(
                name: "AnswerId",
                table: "ReplyDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReplyDetails_ReplyId_AnswerId",
                table: "ReplyDetails",
                columns: new[] { "ReplyId", "AnswerId" },
                unique: true);
        }
    }
}
