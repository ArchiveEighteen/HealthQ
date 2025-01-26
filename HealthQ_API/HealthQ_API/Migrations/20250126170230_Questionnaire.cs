using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthQ_API.Migrations
{
    /// <inheritdoc />
    public partial class Questionnaire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "questionnaires",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    questionnaire_content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questionnaires", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_questionnaire",
                schema: "public",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    QuestionnaireId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_questionnaire", x => new { x.UserId, x.QuestionnaireId });
                    table.ForeignKey(
                        name: "FK_user_questionnaire_questionnaires_QuestionnaireId",
                        column: x => x.QuestionnaireId,
                        principalSchema: "public",
                        principalTable: "questionnaires",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_questionnaire_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_questionnaire_QuestionnaireId",
                schema: "public",
                table: "user_questionnaire",
                column: "QuestionnaireId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_questionnaire",
                schema: "public");

            migrationBuilder.DropTable(
                name: "questionnaires",
                schema: "public");
        }
    }
}
