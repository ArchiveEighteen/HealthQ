using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthQ_API.Migrations
{
    /// <inheritdoc />
    public partial class DoctorPatient_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "doctor_patient",
                schema: "public",
                columns: table => new
                {
                    DoctorEmail = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    PatientEmail = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doctor_patient", x => new { x.DoctorEmail, x.PatientEmail });
                    table.ForeignKey(
                        name: "FK_doctor_patient_users_DoctorEmail",
                        column: x => x.DoctorEmail,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "email",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_doctor_patient_users_PatientEmail",
                        column: x => x.PatientEmail,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_doctor_patient_PatientEmail",
                schema: "public",
                table: "doctor_patient",
                column: "PatientEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "doctor_patient",
                schema: "public");
        }
    }
}
