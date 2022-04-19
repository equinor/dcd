using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class mvp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommonLibraryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommonLibraryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ProjectPhase = table.Column<int>(type: "int", nullable: false),
                    ProjectCategory = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifyTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ReferenceCase = table.Column<bool>(type: "bit", nullable: false),
                    DG1Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DG2Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DG3Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DG4Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DrainageStrategyLink = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectLink = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurfLink = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubstructureLink = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopsideLink = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransportLink = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationLink = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cases_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrainageStrategies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NGLYield = table.Column<double>(type: "float", nullable: false),
                    ProducerCount = table.Column<int>(type: "int", nullable: false),
                    GasInjectorCount = table.Column<int>(type: "int", nullable: false),
                    WaterInjectorCount = table.Column<int>(type: "int", nullable: false),
                    ArtificialLift = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrainageStrategies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrainageStrategies_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Explorations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WellType = table.Column<int>(type: "int", nullable: false),
                    RigMobDemob = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Explorations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Explorations_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Substructures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DryWeight = table.Column<double>(type: "float", nullable: false),
                    Maturity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Substructures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Substructures_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Surfs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Maturity = table.Column<int>(type: "int", nullable: false),
                    InfieldPipelineSystemLength = table.Column<double>(type: "float", nullable: false),
                    UmbilicalSystemLength = table.Column<double>(type: "float", nullable: false),
                    ArtificialLift = table.Column<int>(type: "int", nullable: false),
                    RiserCount = table.Column<int>(type: "int", nullable: false),
                    TemplateCount = table.Column<int>(type: "int", nullable: false),
                    ProductionFlowline = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surfs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Surfs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Topsides",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DryWeight = table.Column<double>(type: "float", nullable: false),
                    OilCapacity = table.Column<double>(type: "float", nullable: false),
                    GasCapacity = table.Column<double>(type: "float", nullable: false),
                    FacilitiesAvailability = table.Column<double>(type: "float", nullable: false),
                    ArtificialLift = table.Column<int>(type: "int", nullable: false),
                    Maturity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topsides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topsides_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GasExportPipelineLength = table.Column<double>(type: "float", nullable: false),
                    OilExportPipelineLength = table.Column<double>(type: "float", nullable: false),
                    Maturity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transports_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WellProjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProducerCount = table.Column<int>(type: "int", nullable: false),
                    GasInjectorCount = table.Column<int>(type: "int", nullable: false),
                    WaterInjectorCount = table.Column<int>(type: "int", nullable: false),
                    ArtificialLift = table.Column<int>(type: "int", nullable: false),
                    RigMobDemob = table.Column<double>(type: "float", nullable: false),
                    AnnualWellInterventionCost = table.Column<double>(type: "float", nullable: false),
                    PluggingAndAbandonment = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WellProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WellProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Co2Emissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Co2Emissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Co2Emissions_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FuelFlaringAndLosses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelFlaringAndLosses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelFlaringAndLosses_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NetSalesGas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetSalesGas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetSalesGas_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionProfileGas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionProfileGas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionProfileGas_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionProfileOil",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionProfileOil", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionProfileOil_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionProfileWater",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionProfileWater", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionProfileWater_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionProfileWaterInjection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionProfileWaterInjection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionProfileWaterInjection_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExplorationCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorationCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExplorationCostProfile_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExplorationDrillingSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorationDrillingSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExplorationDrillingSchedule_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GAndGAdminCost",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GAndGAdminCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GAndGAdminCost_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubstructureCostProfile",
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
                    table.PrimaryKey("PK_SubstructureCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubstructureCostProfile_Substructures_Substructure.Id",
                        column: x => x.SubstructureId,
                        principalTable: "Substructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurfCostProfile",
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
                    table.PrimaryKey("PK_SurfCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurfCostProfile_Surfs_Surf.Id",
                        column: x => x.SurfId,
                        principalTable: "Surfs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopsideCostProfile",
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
                    table.PrimaryKey("PK_TopsideCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopsideCostProfile_Topsides_Topside.Id",
                        column: x => x.TopsideId,
                        principalTable: "Topsides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransportCostProfile",
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
                    table.PrimaryKey("PK_TransportCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportCostProfile_Transports_Transport.Id",
                        column: x => x.TransportId,
                        principalTable: "Transports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrillingSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrillingSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrillingSchedule_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WellProjectCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WellProjectCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WellProjectCostProfile_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cases_ProjectId",
                table: "Cases",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Co2Emissions_DrainageStrategy.Id",
                table: "Co2Emissions",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DrainageStrategies_ProjectId",
                table: "DrainageStrategies",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DrillingSchedule_WellProject.Id",
                table: "DrillingSchedule",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationCostProfile_Exploration.Id",
                table: "ExplorationCostProfile",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationDrillingSchedule_Exploration.Id",
                table: "ExplorationDrillingSchedule",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Explorations_ProjectId",
                table: "Explorations",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelFlaringAndLosses_DrainageStrategy.Id",
                table: "FuelFlaringAndLosses",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GAndGAdminCost_Exploration.Id",
                table: "GAndGAdminCost",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NetSalesGas_DrainageStrategy.Id",
                table: "NetSalesGas",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProfileGas_DrainageStrategy.Id",
                table: "ProductionProfileGas",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProfileOil_DrainageStrategy.Id",
                table: "ProductionProfileOil",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProfileWater_DrainageStrategy.Id",
                table: "ProductionProfileWater",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProfileWaterInjection_DrainageStrategy.Id",
                table: "ProductionProfileWaterInjection",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubstructureCostProfile_Substructure.Id",
                table: "SubstructureCostProfile",
                column: "Substructure.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Substructures_ProjectId",
                table: "Substructures",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SurfCostProfile_Surf.Id",
                table: "SurfCostProfile",
                column: "Surf.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Surfs_ProjectId",
                table: "Surfs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TopsideCostProfile_Topside.Id",
                table: "TopsideCostProfile",
                column: "Topside.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topsides_ProjectId",
                table: "Topsides",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportCostProfile_Transport.Id",
                table: "TransportCostProfile",
                column: "Transport.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transports_ProjectId",
                table: "Transports",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WellProjectCostProfile_WellProject.Id",
                table: "WellProjectCostProfile",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WellProjects_ProjectId",
                table: "WellProjects",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cases");

            migrationBuilder.DropTable(
                name: "Co2Emissions");

            migrationBuilder.DropTable(
                name: "DrillingSchedule");

            migrationBuilder.DropTable(
                name: "ExplorationCostProfile");

            migrationBuilder.DropTable(
                name: "ExplorationDrillingSchedule");

            migrationBuilder.DropTable(
                name: "FuelFlaringAndLosses");

            migrationBuilder.DropTable(
                name: "GAndGAdminCost");

            migrationBuilder.DropTable(
                name: "NetSalesGas");

            migrationBuilder.DropTable(
                name: "ProductionProfileGas");

            migrationBuilder.DropTable(
                name: "ProductionProfileOil");

            migrationBuilder.DropTable(
                name: "ProductionProfileWater");

            migrationBuilder.DropTable(
                name: "ProductionProfileWaterInjection");

            migrationBuilder.DropTable(
                name: "SubstructureCostProfile");

            migrationBuilder.DropTable(
                name: "SurfCostProfile");

            migrationBuilder.DropTable(
                name: "TopsideCostProfile");

            migrationBuilder.DropTable(
                name: "TransportCostProfile");

            migrationBuilder.DropTable(
                name: "WellProjectCostProfile");

            migrationBuilder.DropTable(
                name: "Explorations");

            migrationBuilder.DropTable(
                name: "DrainageStrategies");

            migrationBuilder.DropTable(
                name: "Substructures");

            migrationBuilder.DropTable(
                name: "Surfs");

            migrationBuilder.DropTable(
                name: "Topsides");

            migrationBuilder.DropTable(
                name: "Transports");

            migrationBuilder.DropTable(
                name: "WellProjects");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
