using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthQ_API.Migrations
{
    /// <inheritdoc />
    public partial class DeleteBehaviour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctors_users_user_email",
                schema: "public",
                table: "doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_patients_users_user_email",
                schema: "public",
                table: "patients");

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_users_user_email",
                schema: "public",
                table: "doctors",
                column: "user_email",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_patients_users_user_email",
                schema: "public",
                table: "patients",
                column: "user_email",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "email",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctors_users_user_email",
                schema: "public",
                table: "doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_patients_users_user_email",
                schema: "public",
                table: "patients");

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_users_user_email",
                schema: "public",
                table: "doctors",
                column: "user_email",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "email");

            migrationBuilder.AddForeignKey(
                name: "FK_patients_users_user_email",
                schema: "public",
                table: "patients",
                column: "user_email",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "email");
        }
    }
}
