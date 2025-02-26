using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthQ_API.Migrations
{
    /// <inheritdoc />
    public partial class FixClinicalImpression : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "patient_id",
                schema: "public",
                table: "clinical_impressions",
                type: "character varying(254)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_clinical_impressions_patient_id",
                schema: "public",
                table: "clinical_impressions",
                column: "patient_id");

            migrationBuilder.AddForeignKey(
                name: "FK_clinical_impressions_patients_patient_id",
                schema: "public",
                table: "clinical_impressions",
                column: "patient_id",
                principalSchema: "public",
                principalTable: "patients",
                principalColumn: "user_email",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clinical_impressions_patients_patient_id",
                schema: "public",
                table: "clinical_impressions");

            migrationBuilder.DropIndex(
                name: "IX_clinical_impressions_patient_id",
                schema: "public",
                table: "clinical_impressions");

            migrationBuilder.DropColumn(
                name: "patient_id",
                schema: "public",
                table: "clinical_impressions");
        }
    }
}
