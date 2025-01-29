using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthQ_API.Migrations
{
    /// <inheritdoc />
    public partial class DoctorTbl_PatientTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctor_patient_users_DoctorEmail",
                schema: "public",
                table: "doctor_patient");

            migrationBuilder.DropForeignKey(
                name: "FK_doctor_patient_users_PatientEmail",
                schema: "public",
                table: "doctor_patient");

            migrationBuilder.DropTable(
                name: "user_questionnaire",
                schema: "public");

            migrationBuilder.AddColumn<string>(
                name: "owner_email",
                schema: "public",
                table: "questionnaires",
                type: "character varying(254)",
                maxLength: 254,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DoctorUserEmail",
                schema: "public",
                table: "doctor_patient",
                type: "character varying(254)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatientUserEmail",
                schema: "public",
                table: "doctor_patient",
                type: "character varying(254)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "doctors",
                schema: "public",
                columns: table => new
                {
                    user_email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doctors", x => x.user_email);
                    table.ForeignKey(
                        name: "FK_doctors_users_user_email",
                        column: x => x.user_email,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "email");
                });

            migrationBuilder.CreateTable(
                name: "patients",
                schema: "public",
                columns: table => new
                {
                    user_email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patients", x => x.user_email);
                    table.ForeignKey(
                        name: "FK_patients_users_user_email",
                        column: x => x.user_email,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "email");
                });

            migrationBuilder.CreateTable(
                name: "patient_questionnaire",
                schema: "public",
                columns: table => new
                {
                    PatientEmail = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    QuestionnaireId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserEmail = table.Column<string>(type: "character varying(254)", nullable: false),
                    QuestionnaireId1 = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patient_questionnaire", x => new { x.PatientEmail, x.QuestionnaireId });
                    table.ForeignKey(
                        name: "FK_patient_questionnaire_patients_PatientEmail",
                        column: x => x.PatientEmail,
                        principalSchema: "public",
                        principalTable: "patients",
                        principalColumn: "user_email",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_patient_questionnaire_questionnaires_QuestionnaireId",
                        column: x => x.QuestionnaireId,
                        principalSchema: "public",
                        principalTable: "questionnaires",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_patient_questionnaire_questionnaires_QuestionnaireId1",
                        column: x => x.QuestionnaireId1,
                        principalSchema: "public",
                        principalTable: "questionnaires",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_patient_questionnaire_users_UserEmail",
                        column: x => x.UserEmail,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_questionnaires_owner_email",
                schema: "public",
                table: "questionnaires",
                column: "owner_email");

            migrationBuilder.CreateIndex(
                name: "IX_doctor_patient_DoctorUserEmail",
                schema: "public",
                table: "doctor_patient",
                column: "DoctorUserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_doctor_patient_PatientUserEmail",
                schema: "public",
                table: "doctor_patient",
                column: "PatientUserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_patient_questionnaire_QuestionnaireId",
                schema: "public",
                table: "patient_questionnaire",
                column: "QuestionnaireId");

            migrationBuilder.CreateIndex(
                name: "IX_patient_questionnaire_QuestionnaireId1",
                schema: "public",
                table: "patient_questionnaire",
                column: "QuestionnaireId1");

            migrationBuilder.CreateIndex(
                name: "IX_patient_questionnaire_UserEmail",
                schema: "public",
                table: "patient_questionnaire",
                column: "UserEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_doctor_patient_doctors_DoctorEmail",
                schema: "public",
                table: "doctor_patient",
                column: "DoctorEmail",
                principalSchema: "public",
                principalTable: "doctors",
                principalColumn: "user_email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_doctor_patient_doctors_DoctorUserEmail",
                schema: "public",
                table: "doctor_patient",
                column: "DoctorUserEmail",
                principalSchema: "public",
                principalTable: "doctors",
                principalColumn: "user_email");

            migrationBuilder.AddForeignKey(
                name: "FK_doctor_patient_patients_PatientEmail",
                schema: "public",
                table: "doctor_patient",
                column: "PatientEmail",
                principalSchema: "public",
                principalTable: "patients",
                principalColumn: "user_email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_doctor_patient_patients_PatientUserEmail",
                schema: "public",
                table: "doctor_patient",
                column: "PatientUserEmail",
                principalSchema: "public",
                principalTable: "patients",
                principalColumn: "user_email");

            migrationBuilder.AddForeignKey(
                name: "FK_questionnaires_doctors_owner_email",
                schema: "public",
                table: "questionnaires",
                column: "owner_email",
                principalSchema: "public",
                principalTable: "doctors",
                principalColumn: "user_email",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctor_patient_doctors_DoctorEmail",
                schema: "public",
                table: "doctor_patient");

            migrationBuilder.DropForeignKey(
                name: "FK_doctor_patient_doctors_DoctorUserEmail",
                schema: "public",
                table: "doctor_patient");

            migrationBuilder.DropForeignKey(
                name: "FK_doctor_patient_patients_PatientEmail",
                schema: "public",
                table: "doctor_patient");

            migrationBuilder.DropForeignKey(
                name: "FK_doctor_patient_patients_PatientUserEmail",
                schema: "public",
                table: "doctor_patient");

            migrationBuilder.DropForeignKey(
                name: "FK_questionnaires_doctors_owner_email",
                schema: "public",
                table: "questionnaires");

            migrationBuilder.DropTable(
                name: "doctors",
                schema: "public");

            migrationBuilder.DropTable(
                name: "patient_questionnaire",
                schema: "public");

            migrationBuilder.DropTable(
                name: "patients",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_questionnaires_owner_email",
                schema: "public",
                table: "questionnaires");

            migrationBuilder.DropIndex(
                name: "IX_doctor_patient_DoctorUserEmail",
                schema: "public",
                table: "doctor_patient");

            migrationBuilder.DropIndex(
                name: "IX_doctor_patient_PatientUserEmail",
                schema: "public",
                table: "doctor_patient");

            migrationBuilder.DropColumn(
                name: "owner_email",
                schema: "public",
                table: "questionnaires");

            migrationBuilder.DropColumn(
                name: "DoctorUserEmail",
                schema: "public",
                table: "doctor_patient");

            migrationBuilder.DropColumn(
                name: "PatientUserEmail",
                schema: "public",
                table: "doctor_patient");

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

            migrationBuilder.AddForeignKey(
                name: "FK_doctor_patient_users_DoctorEmail",
                schema: "public",
                table: "doctor_patient",
                column: "DoctorEmail",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_doctor_patient_users_PatientEmail",
                schema: "public",
                table: "doctor_patient",
                column: "PatientEmail",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "email",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
