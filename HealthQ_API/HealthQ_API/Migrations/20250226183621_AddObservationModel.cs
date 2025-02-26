using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthQ_API.Migrations
{
    /// <inheritdoc />
    public partial class AddObservationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "observations",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    clinical_impression_id = table.Column<Guid>(type: "uuid", nullable: false),
                    observation_content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_observations", x => x.id);
                    table.ForeignKey(
                        name: "FK_observations_clinical_impressions_clinical_impression_id",
                        column: x => x.clinical_impression_id,
                        principalSchema: "public",
                        principalTable: "clinical_impressions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_observations_clinical_impression_id",
                schema: "public",
                table: "observations",
                column: "clinical_impression_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "observations",
                schema: "public");
        }
    }
}
