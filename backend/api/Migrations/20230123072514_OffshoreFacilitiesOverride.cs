using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class OffshoreFacilitiesOverride : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubstructureCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubstructureId = table.Column<Guid>(name: "Substructure.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubstructureCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubstructureCostProfileOverride_Substructures_Substructure.Id",
                        column: x => x.SubstructureId,
                        principalTable: "Substructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurfCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurfId = table.Column<Guid>(name: "Surf.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurfCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurfCostProfileOverride_Surfs_Surf.Id",
                        column: x => x.SurfId,
                        principalTable: "Surfs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopsideCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopsideId = table.Column<Guid>(name: "Topside.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopsideCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopsideCostProfileOverride_Topsides_Topside.Id",
                        column: x => x.TopsideId,
                        principalTable: "Topsides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransportCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransportId = table.Column<Guid>(name: "Transport.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportCostProfileOverride_Transports_Transport.Id",
                        column: x => x.TransportId,
                        principalTable: "Transports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubstructureCostProfileOverride_Substructure.Id",
                table: "SubstructureCostProfileOverride",
                column: "Substructure.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurfCostProfileOverride_Surf.Id",
                table: "SurfCostProfileOverride",
                column: "Surf.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopsideCostProfileOverride_Topside.Id",
                table: "TopsideCostProfileOverride",
                column: "Topside.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransportCostProfileOverride_Transport.Id",
                table: "TransportCostProfileOverride",
                column: "Transport.Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubstructureCostProfileOverride");

            migrationBuilder.DropTable(
                name: "SurfCostProfileOverride");

            migrationBuilder.DropTable(
                name: "TopsideCostProfileOverride");

            migrationBuilder.DropTable(
                name: "TransportCostProfileOverride");
        }
    }
}
