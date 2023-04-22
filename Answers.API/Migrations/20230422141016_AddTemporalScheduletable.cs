using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Answers.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTemporalScheduletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TemporalSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionnaireId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QuestionnaireId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<float>(type: "real", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporalSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemporalSchedules_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TemporalSchedules_Questionnaires_QuestionnaireId1",
                        column: x => x.QuestionnaireId1,
                        principalTable: "Questionnaires",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemporalSchedules_QuestionnaireId1",
                table: "TemporalSchedules",
                column: "QuestionnaireId1");

            migrationBuilder.CreateIndex(
                name: "IX_TemporalSchedules_UserId1",
                table: "TemporalSchedules",
                column: "UserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemporalSchedules");
        }
    }
}
