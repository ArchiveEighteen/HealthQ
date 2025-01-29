using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthQ_API.Migrations
{
    /// <inheritdoc />
    public partial class ManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "FK_patient_questionnaire_patients_PatientEmail",
                schema: "public",
                table: "patient_questionnaire");

            migrationBuilder.DropForeignKey(
                name: "FK_patient_questionnaire_questionnaires_QuestionnaireId1",
                schema: "public",
                table: "patient_questionnaire");

            migrationBuilder.DropForeignKey(
                name: "FK_patient_questionnaire_users_UserEmail",
                schema: "public",
                table: "patient_questionnaire");

            migrationBuilder.DropIndex(
                name: "IX_patient_questionnaire_QuestionnaireId1",
                schema: "public",
                table: "patient_questionnaire");

            migrationBuilder.DropIndex(
                name: "IX_patient_questionnaire_UserEmail",
                schema: "public",
                table: "patient_questionnaire");

            migrationBuilder.DropIndex(
                name: "IX_doctor_patient_DoctorUserEmail",
                schema: "public",
                table: "doctor_patient");

            migrationBuilder.DropIndex(
                name: "IX_doctor_patient_PatientUserEmail",
                schema: "public",
                table: "doctor_patient");

            migrationBuilder.DropColumn(
                name: "QuestionnaireId1",
                schema: "public",
                table: "patient_questionnaire");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                schema: "public",
                table: "patient_questionnaire");

            migrationBuilder.DropColumn(
                name: "DoctorUserEmail",
                schema: "public",
                table: "doctor_patient");

            migrationBuilder.DropColumn(
                name: "PatientUserEmail",
                schema: "public",
                table: "doctor_patient");

            migrationBuilder.RenameColumn(
                name: "PatientEmail",
                schema: "public",
                table: "patient_questionnaire",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "PatientEmail",
                schema: "public",
                table: "doctor_patient",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "DoctorEmail",
                schema: "public",
                table: "doctor_patient",
                newName: "DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_doctor_patient_PatientEmail",
                schema: "public",
                table: "doctor_patient",
                newName: "IX_doctor_patient_PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_doctor_patient_doctors_DoctorId",
                schema: "public",
                table: "doctor_patient",
                column: "DoctorId",
                principalSchema: "public",
                principalTable: "doctors",
                principalColumn: "user_email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_doctor_patient_patients_PatientId",
                schema: "public",
                table: "doctor_patient",
                column: "PatientId",
                principalSchema: "public",
                principalTable: "patients",
                principalColumn: "user_email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_patient_questionnaire_patients_PatientId",
                schema: "public",
                table: "patient_questionnaire",
                column: "PatientId",
                principalSchema: "public",
                principalTable: "patients",
                principalColumn: "user_email",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctor_patient_doctors_DoctorId",
                schema: "public",
                table: "doctor_patient");

            migrationBuilder.DropForeignKey(
                name: "FK_doctor_patient_patients_PatientId",
                schema: "public",
                table: "doctor_patient");

            migrationBuilder.DropForeignKey(
                name: "FK_patient_questionnaire_patients_PatientId",
                schema: "public",
                table: "patient_questionnaire");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                schema: "public",
                table: "patient_questionnaire",
                newName: "PatientEmail");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                schema: "public",
                table: "doctor_patient",
                newName: "PatientEmail");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                schema: "public",
                table: "doctor_patient",
                newName: "DoctorEmail");

            migrationBuilder.RenameIndex(
                name: "IX_doctor_patient_PatientId",
                schema: "public",
                table: "doctor_patient",
                newName: "IX_doctor_patient_PatientEmail");

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionnaireId1",
                schema: "public",
                table: "patient_questionnaire",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                schema: "public",
                table: "patient_questionnaire",
                type: "character varying(254)",
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
                name: "FK_patient_questionnaire_patients_PatientEmail",
                schema: "public",
                table: "patient_questionnaire",
                column: "PatientEmail",
                principalSchema: "public",
                principalTable: "patients",
                principalColumn: "user_email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_patient_questionnaire_questionnaires_QuestionnaireId1",
                schema: "public",
                table: "patient_questionnaire",
                column: "QuestionnaireId1",
                principalSchema: "public",
                principalTable: "questionnaires",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_patient_questionnaire_users_UserEmail",
                schema: "public",
                table: "patient_questionnaire",
                column: "UserEmail",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "email",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
