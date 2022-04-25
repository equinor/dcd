using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class CessationCost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubstructureCessationCostProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubstructureId = table.Column<Guid>(name: "Substructure.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubstructureCessationCostProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubstructureCessationCostProfiles_Substructures_Substructure.Id",
                        column: x => x.SubstructureId,
                        principalTable: "Substructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurfCessationCostProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurfId = table.Column<Guid>(name: "Surf.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurfCessationCostProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurfCessationCostProfiles_Surfs_Surf.Id",
                        column: x => x.SurfId,
                        principalTable: "Surfs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopsideCessationCostProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopsideId = table.Column<Guid>(name: "Topside.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopsideCessationCostProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopsideCessationCostProfiles_Topsides_Topside.Id",
                        column: x => x.TopsideId,
                        principalTable: "Topsides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransportCessationCostProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransportId = table.Column<Guid>(name: "Transport.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportCessationCostProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportCessationCostProfiles_Transports_Transport.Id",
                        column: x => x.TransportId,
                        principalTable: "Transports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubstructureCessationCostProfiles_Substructure.Id",
                table: "SubstructureCessationCostProfiles",
                column: "Substructure.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurfCessationCostProfiles_Surf.Id",
                table: "SurfCessationCostProfiles",
                column: "Surf.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopsideCessationCostProfiles_Topside.Id",
                table: "TopsideCessationCostProfiles",
                column: "Topside.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransportCessationCostProfiles_Transport.Id",
                table: "TransportCessationCostProfiles",
                column: "Transport.Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubstructureCessationCostProfiles");

            migrationBuilder.DropTable(
                name: "SurfCessationCostProfiles");

            migrationBuilder.DropTable(
                name: "TopsideCessationCostProfiles");

            migrationBuilder.DropTable(
                name: "TransportCessationCostProfiles");
        }
    }
}
