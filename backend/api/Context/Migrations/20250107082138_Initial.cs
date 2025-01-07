using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChangeLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimestampUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntityState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DrillingSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrillingSchedule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExceptionLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UtcTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HttpStatusCode = table.Column<int>(type: "int", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Environment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InnerExceptionStackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InnerExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LazyLoadingOccurrences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimestampUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LazyLoadingOccurrences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRevision = table.Column<bool>(type: "bit", nullable: false),
                    CommonLibraryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FusionProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceCaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommonLibraryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    PhysicalUnit = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjectPhase = table.Column<int>(type: "int", nullable: false),
                    InternalProjectPhase = table.Column<int>(type: "int", nullable: false),
                    Classification = table.Column<int>(type: "int", nullable: false),
                    ProjectCategory = table.Column<int>(type: "int", nullable: false),
                    SharepointSiteUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CO2RemovedFromGas = table.Column<double>(type: "float", nullable: false),
                    CO2EmissionFromFuelGas = table.Column<double>(type: "float", nullable: false),
                    FlaredGasPerProducedVolume = table.Column<double>(type: "float", nullable: false),
                    CO2EmissionsFromFlaredGas = table.Column<double>(type: "float", nullable: false),
                    CO2Vented = table.Column<double>(type: "float", nullable: false),
                    DailyEmissionFromDrillingRig = table.Column<double>(type: "float", nullable: false),
                    AverageDevelopmentDrillingDays = table.Column<double>(type: "float", nullable: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OilPriceUSD = table.Column<double>(type: "float", nullable: false),
                    GasPriceNOK = table.Column<double>(type: "float", nullable: false),
                    DiscountRate = table.Column<double>(type: "float", nullable: false),
                    ExchangeRateUSDToNOK = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Projects_OriginalProjectId",
                        column: x => x.OriginalProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrlPattern = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Verb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestLengthInMilliseconds = table.Column<long>(type: "bigint", nullable: false),
                    RequestStartUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestEndUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DevelopmentOperationalWellCosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RigUpgrading = table.Column<double>(type: "float", nullable: false),
                    RigMobDemob = table.Column<double>(type: "float", nullable: false),
                    AnnualWellInterventionCostPerWell = table.Column<double>(type: "float", nullable: false),
                    PluggingAndAbandonment = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevelopmentOperationalWellCosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DevelopmentOperationalWellCosts_Projects_ProjectId",
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
                    ArtificialLift = table.Column<int>(type: "int", nullable: false),
                    GasSolution = table.Column<int>(type: "int", nullable: false)
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
                name: "ExplorationOperationalWellCosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationRigUpgrading = table.Column<double>(type: "float", nullable: false),
                    ExplorationRigMobDemob = table.Column<double>(type: "float", nullable: false),
                    ExplorationProjectDrillingCosts = table.Column<double>(type: "float", nullable: false),
                    AppraisalRigMobDemob = table.Column<double>(type: "float", nullable: false),
                    AppraisalProjectDrillingCosts = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorationOperationalWellCosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExplorationOperationalWellCosts_Projects_ProjectId",
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
                    RigMobDemob = table.Column<double>(type: "float", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
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
                name: "OnshorePowerSupplies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastChangedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CostYear = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
                    ProspVersion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DG3Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DG4Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnshorePowerSupplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnshorePowerSupplies_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    FromOrgChart = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RevisionDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RevisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RevisionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevisionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Arena = table.Column<bool>(type: "bit", nullable: false),
                    Mdqc = table.Column<bool>(type: "bit", nullable: false),
                    Classification = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevisionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RevisionDetails_Projects_RevisionId",
                        column: x => x.RevisionId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Substructures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DryWeight = table.Column<double>(type: "float", nullable: false),
                    Maturity = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostYear = table.Column<int>(type: "int", nullable: false),
                    ProspVersion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Source = table.Column<int>(type: "int", nullable: false),
                    LastChangedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Concept = table.Column<int>(type: "int", nullable: false),
                    DG3Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DG4Date = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CessationCost = table.Column<double>(type: "float", nullable: false),
                    Maturity = table.Column<int>(type: "int", nullable: false),
                    InfieldPipelineSystemLength = table.Column<double>(type: "float", nullable: false),
                    UmbilicalSystemLength = table.Column<double>(type: "float", nullable: false),
                    ArtificialLift = table.Column<int>(type: "int", nullable: false),
                    RiserCount = table.Column<int>(type: "int", nullable: false),
                    TemplateCount = table.Column<int>(type: "int", nullable: false),
                    ProducerCount = table.Column<int>(type: "int", nullable: false),
                    GasInjectorCount = table.Column<int>(type: "int", nullable: false),
                    WaterInjectorCount = table.Column<int>(type: "int", nullable: false),
                    ProductionFlowline = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    LastChangedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CostYear = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
                    ProspVersion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DG3Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DG4Date = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DryWeight = table.Column<double>(type: "float", nullable: false),
                    OilCapacity = table.Column<double>(type: "float", nullable: false),
                    GasCapacity = table.Column<double>(type: "float", nullable: false),
                    WaterInjectionCapacity = table.Column<double>(type: "float", nullable: false),
                    ArtificialLift = table.Column<int>(type: "int", nullable: false),
                    Maturity = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    FuelConsumption = table.Column<double>(type: "float", nullable: false),
                    FlaredGas = table.Column<double>(type: "float", nullable: false),
                    ProducerCount = table.Column<int>(type: "int", nullable: false),
                    GasInjectorCount = table.Column<int>(type: "int", nullable: false),
                    WaterInjectorCount = table.Column<int>(type: "int", nullable: false),
                    CO2ShareOilProfile = table.Column<double>(type: "float", nullable: false),
                    CO2ShareGasProfile = table.Column<double>(type: "float", nullable: false),
                    CO2ShareWaterInjectionProfile = table.Column<double>(type: "float", nullable: false),
                    CO2OnMaxOilProfile = table.Column<double>(type: "float", nullable: false),
                    CO2OnMaxGasProfile = table.Column<double>(type: "float", nullable: false),
                    CO2OnMaxWaterInjectionProfile = table.Column<double>(type: "float", nullable: false),
                    CostYear = table.Column<int>(type: "int", nullable: false),
                    ProspVersion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastChangedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Source = table.Column<int>(type: "int", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DG3Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DG4Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FacilityOpex = table.Column<double>(type: "float", nullable: false),
                    PeakElectricityImported = table.Column<double>(type: "float", nullable: false)
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
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GasExportPipelineLength = table.Column<double>(type: "float", nullable: false),
                    OilExportPipelineLength = table.Column<double>(type: "float", nullable: false),
                    Maturity = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    LastChangedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CostYear = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
                    ProspVersion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DG3Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DG4Date = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    ArtificialLift = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
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
                name: "Wells",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WellCategory = table.Column<int>(type: "int", nullable: false),
                    WellCost = table.Column<double>(type: "float", nullable: false),
                    DrillingDays = table.Column<double>(type: "float", nullable: false),
                    PlugingAndAbandonmentCost = table.Column<double>(type: "float", nullable: false),
                    WellInterventionCost = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wells_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdditionalProductionProfileGas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalProductionProfileGas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalProductionProfileGas_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdditionalProductionProfileOil",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalProductionProfileOil", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalProductionProfileOil_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
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
                name: "Co2EmissionsOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Co2EmissionsOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Co2EmissionsOverride_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeferredGasProduction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeferredGasProduction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeferredGasProduction_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeferredOilProduction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeferredOilProduction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeferredOilProduction_DrainageStrategies_DrainageStrategy.Id",
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
                name: "FuelFlaringAndLossesOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelFlaringAndLossesOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelFlaringAndLossesOverride_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportedElectricity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedElectricity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportedElectricity_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportedElectricityOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedElectricityOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportedElectricityOverride_DrainageStrategies_DrainageStrategy.Id",
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
                name: "NetSalesGasOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetSalesGasOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetSalesGasOverride_DrainageStrategies_DrainageStrategy.Id",
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
                name: "ProductionProfileNgl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionProfileNgl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionProfileNgl_DrainageStrategies_DrainageStrategy.Id",
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
                name: "AppraisalWellCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppraisalWellCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppraisalWellCostProfile_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryOfficeCost",
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
                    table.PrimaryKey("PK_CountryOfficeCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryOfficeCost_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExplorationWellCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorationWellCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExplorationWellCostProfile_Explorations_Exploration.Id",
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
                name: "GAndGAdminCostOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GAndGAdminCostOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GAndGAdminCostOverride_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeismicAcquisitionAndProcessing",
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
                    table.PrimaryKey("PK_SeismicAcquisitionAndProcessing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeismicAcquisitionAndProcessing_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SidetrackCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SidetrackCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SidetrackCostProfile_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnshorePowerSupplyCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OnshorePowerSupplyId = table.Column<Guid>(name: "OnshorePowerSupply.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnshorePowerSupplyCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnshorePowerSupplyCostProfile_OnshorePowerSupplies_OnshorePowerSupply.Id",
                        column: x => x.OnshorePowerSupplyId,
                        principalTable: "OnshorePowerSupplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnshorePowerSupplyCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OnshorePowerSupplyId = table.Column<Guid>(name: "OnshorePowerSupply.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnshorePowerSupplyCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnshorePowerSupplyCostProfileOverride_OnshorePowerSupplies_OnshorePowerSupply.Id",
                        column: x => x.OnshorePowerSupplyId,
                        principalTable: "OnshorePowerSupplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "SubstructureCostProfiles",
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
                    table.PrimaryKey("PK_SubstructureCostProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubstructureCostProfiles_Substructures_Substructure.Id",
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
                name: "TopsideCostProfiles",
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
                    table.PrimaryKey("PK_TopsideCostProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopsideCostProfiles_Topsides_Topside.Id",
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

            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceCase = table.Column<bool>(type: "bit", nullable: false),
                    Archived = table.Column<bool>(type: "bit", nullable: false),
                    SharepointFileId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SharepointFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SharepointFileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DGADate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DGBDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DGCDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    APBODate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BORDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VPBODate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DG0Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DG1Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DG2Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DG3Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DG4Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArtificialLift = table.Column<int>(type: "int", nullable: false),
                    ProductionStrategyOverview = table.Column<int>(type: "int", nullable: false),
                    ProducerCount = table.Column<int>(type: "int", nullable: false),
                    GasInjectorCount = table.Column<int>(type: "int", nullable: false),
                    WaterInjectorCount = table.Column<int>(type: "int", nullable: false),
                    FacilitiesAvailability = table.Column<double>(type: "float", nullable: false),
                    CapexFactorFeasibilityStudies = table.Column<double>(type: "float", nullable: false),
                    CapexFactorFEEDStudies = table.Column<double>(type: "float", nullable: false),
                    NPV = table.Column<double>(type: "float", nullable: false),
                    NPVOverride = table.Column<double>(type: "float", nullable: true),
                    BreakEven = table.Column<double>(type: "float", nullable: false),
                    BreakEvenOverride = table.Column<double>(type: "float", nullable: true),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrainageStrategyLink = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectLink = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurfLink = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubstructureLink = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopsideLink = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransportLink = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OnshorePowerSupplyLink = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationLink = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cases_DrainageStrategies_DrainageStrategyLink",
                        column: x => x.DrainageStrategyLink,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cases_Explorations_ExplorationLink",
                        column: x => x.ExplorationLink,
                        principalTable: "Explorations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cases_OnshorePowerSupplies_OnshorePowerSupplyLink",
                        column: x => x.OnshorePowerSupplyLink,
                        principalTable: "OnshorePowerSupplies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cases_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cases_Substructures_SubstructureLink",
                        column: x => x.SubstructureLink,
                        principalTable: "Substructures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cases_Surfs_SurfLink",
                        column: x => x.SurfLink,
                        principalTable: "Surfs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cases_Topsides_TopsideLink",
                        column: x => x.TopsideLink,
                        principalTable: "Topsides",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cases_Transports_TransportLink",
                        column: x => x.TransportLink,
                        principalTable: "Transports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cases_WellProjects_WellProjectLink",
                        column: x => x.WellProjectLink,
                        principalTable: "WellProjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GasInjectorCostProfile",
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
                    table.PrimaryKey("PK_GasInjectorCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GasInjectorCostProfile_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GasInjectorCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GasInjectorCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GasInjectorCostProfileOverride_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GasProducerCostProfile",
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
                    table.PrimaryKey("PK_GasProducerCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GasProducerCostProfile_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GasProducerCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GasProducerCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GasProducerCostProfileOverride_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OilProducerCostProfile",
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
                    table.PrimaryKey("PK_OilProducerCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OilProducerCostProfile_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OilProducerCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OilProducerCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OilProducerCostProfileOverride_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WaterInjectorCostProfile",
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
                    table.PrimaryKey("PK_WaterInjectorCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterInjectorCostProfile_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WaterInjectorCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterInjectorCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterInjectorCostProfileOverride_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExplorationWell",
                columns: table => new
                {
                    WellId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrillingScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorationWell", x => new { x.ExplorationId, x.WellId });
                    table.ForeignKey(
                        name: "FK_ExplorationWell_DrillingSchedule_DrillingScheduleId",
                        column: x => x.DrillingScheduleId,
                        principalTable: "DrillingSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExplorationWell_Explorations_ExplorationId",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExplorationWell_Wells_WellId",
                        column: x => x.WellId,
                        principalTable: "Wells",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WellProjectWell",
                columns: table => new
                {
                    WellProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrillingScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WellProjectWell", x => new { x.WellProjectId, x.WellId });
                    table.ForeignKey(
                        name: "FK_WellProjectWell_DrillingSchedule_DrillingScheduleId",
                        column: x => x.DrillingScheduleId,
                        principalTable: "DrillingSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WellProjectWell_WellProjects_WellProjectId",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WellProjectWell_Wells_WellId",
                        column: x => x.WellId,
                        principalTable: "Wells",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdditionalOPEXCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalOPEXCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalOPEXCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalculatedTotalCostCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculatedTotalCostCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculatedTotalCostCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalculatedTotalIncomeCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculatedTotalIncomeCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculatedTotalIncomeCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CessationOffshoreFacilitiesCost",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CessationOffshoreFacilitiesCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CessationOffshoreFacilitiesCost_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CessationOffshoreFacilitiesCostOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CessationOffshoreFacilitiesCostOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CessationOffshoreFacilitiesCostOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CessationOnshoreFacilitiesCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CessationOnshoreFacilitiesCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CessationOnshoreFacilitiesCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CessationWellsCost",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CessationWellsCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CessationWellsCost_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CessationWellsCostOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CessationWellsCostOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CessationWellsCostOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoricCostCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricCostCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricCostCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OffshoreFacilitiesOperationsCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffshoreFacilitiesOperationsCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OffshoreFacilitiesOperationsCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OffshoreFacilitiesOperationsCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffshoreFacilitiesOperationsCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OffshoreFacilitiesOperationsCostProfileOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnshoreRelatedOPEXCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnshoreRelatedOPEXCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnshoreRelatedOPEXCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TotalFeasibilityAndConceptStudies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalFeasibilityAndConceptStudies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotalFeasibilityAndConceptStudies_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TotalFeasibilityAndConceptStudiesOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalFeasibilityAndConceptStudiesOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotalFeasibilityAndConceptStudiesOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TotalFEEDStudies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalFEEDStudies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotalFEEDStudies_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TotalFEEDStudiesOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalFEEDStudiesOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotalFEEDStudiesOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TotalOtherStudiesCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalOtherStudiesCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotalOtherStudiesCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WellInterventionCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WellInterventionCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WellInterventionCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WellInterventionCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WellInterventionCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WellInterventionCostProfileOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalOPEXCostProfile_Case.Id",
                table: "AdditionalOPEXCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalProductionProfileGas_DrainageStrategy.Id",
                table: "AdditionalProductionProfileGas",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalProductionProfileOil_DrainageStrategy.Id",
                table: "AdditionalProductionProfileOil",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalWellCostProfile_Exploration.Id",
                table: "AppraisalWellCostProfile",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedTotalCostCostProfile_Case.Id",
                table: "CalculatedTotalCostCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedTotalIncomeCostProfile_Case.Id",
                table: "CalculatedTotalIncomeCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cases_DrainageStrategyLink",
                table: "Cases",
                column: "DrainageStrategyLink");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_ExplorationLink",
                table: "Cases",
                column: "ExplorationLink");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_OnshorePowerSupplyLink",
                table: "Cases",
                column: "OnshorePowerSupplyLink");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_ProjectId",
                table: "Cases",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_SubstructureLink",
                table: "Cases",
                column: "SubstructureLink");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_SurfLink",
                table: "Cases",
                column: "SurfLink");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_TopsideLink",
                table: "Cases",
                column: "TopsideLink");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_TransportLink",
                table: "Cases",
                column: "TransportLink");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_WellProjectLink",
                table: "Cases",
                column: "WellProjectLink");

            migrationBuilder.CreateIndex(
                name: "IX_CessationOffshoreFacilitiesCost_Case.Id",
                table: "CessationOffshoreFacilitiesCost",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CessationOffshoreFacilitiesCostOverride_Case.Id",
                table: "CessationOffshoreFacilitiesCostOverride",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CessationOnshoreFacilitiesCostProfile_Case.Id",
                table: "CessationOnshoreFacilitiesCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CessationWellsCost_Case.Id",
                table: "CessationWellsCost",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CessationWellsCostOverride_Case.Id",
                table: "CessationWellsCostOverride",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_EntityId",
                table: "ChangeLogs",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_EntityName",
                table: "ChangeLogs",
                column: "EntityName");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_PropertyName",
                table: "ChangeLogs",
                column: "PropertyName");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_TimestampUtc",
                table: "ChangeLogs",
                column: "TimestampUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Co2Emissions_DrainageStrategy.Id",
                table: "Co2Emissions",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Co2EmissionsOverride_DrainageStrategy.Id",
                table: "Co2EmissionsOverride",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountryOfficeCost_Exploration.Id",
                table: "CountryOfficeCost",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeferredGasProduction_DrainageStrategy.Id",
                table: "DeferredGasProduction",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeferredOilProduction_DrainageStrategy.Id",
                table: "DeferredOilProduction",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DevelopmentOperationalWellCosts_ProjectId",
                table: "DevelopmentOperationalWellCosts",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DrainageStrategies_ProjectId",
                table: "DrainageStrategies",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationOperationalWellCosts_ProjectId",
                table: "ExplorationOperationalWellCosts",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Explorations_ProjectId",
                table: "Explorations",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationWell_DrillingScheduleId",
                table: "ExplorationWell",
                column: "DrillingScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationWell_WellId",
                table: "ExplorationWell",
                column: "WellId");

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationWellCostProfile_Exploration.Id",
                table: "ExplorationWellCostProfile",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FuelFlaringAndLosses_DrainageStrategy.Id",
                table: "FuelFlaringAndLosses",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FuelFlaringAndLossesOverride_DrainageStrategy.Id",
                table: "FuelFlaringAndLossesOverride",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GAndGAdminCost_Exploration.Id",
                table: "GAndGAdminCost",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GAndGAdminCostOverride_Exploration.Id",
                table: "GAndGAdminCostOverride",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GasInjectorCostProfile_WellProject.Id",
                table: "GasInjectorCostProfile",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GasInjectorCostProfileOverride_WellProject.Id",
                table: "GasInjectorCostProfileOverride",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GasProducerCostProfile_WellProject.Id",
                table: "GasProducerCostProfile",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GasProducerCostProfileOverride_WellProject.Id",
                table: "GasProducerCostProfileOverride",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoricCostCostProfile_Case.Id",
                table: "HistoricCostCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_CaseId",
                table: "Images",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedElectricity_DrainageStrategy.Id",
                table: "ImportedElectricity",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportedElectricityOverride_DrainageStrategy.Id",
                table: "ImportedElectricityOverride",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NetSalesGas_DrainageStrategy.Id",
                table: "NetSalesGas",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NetSalesGasOverride_DrainageStrategy.Id",
                table: "NetSalesGasOverride",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OffshoreFacilitiesOperationsCostProfile_Case.Id",
                table: "OffshoreFacilitiesOperationsCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OffshoreFacilitiesOperationsCostProfileOverride_Case.Id",
                table: "OffshoreFacilitiesOperationsCostProfileOverride",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OilProducerCostProfile_WellProject.Id",
                table: "OilProducerCostProfile",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OilProducerCostProfileOverride_WellProject.Id",
                table: "OilProducerCostProfileOverride",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnshorePowerSupplies_ProjectId",
                table: "OnshorePowerSupplies",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_OnshorePowerSupplyCostProfile_OnshorePowerSupply.Id",
                table: "OnshorePowerSupplyCostProfile",
                column: "OnshorePowerSupply.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnshorePowerSupplyCostProfileOverride_OnshorePowerSupply.Id",
                table: "OnshorePowerSupplyCostProfileOverride",
                column: "OnshorePowerSupply.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnshoreRelatedOPEXCostProfile_Case.Id",
                table: "OnshoreRelatedOPEXCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProfileGas_DrainageStrategy.Id",
                table: "ProductionProfileGas",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProfileNgl_DrainageStrategy.Id",
                table: "ProductionProfileNgl",
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
                name: "IX_ProjectMembers_ProjectId",
                table: "ProjectMembers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_UserId_ProjectId",
                table: "ProjectMembers",
                columns: new[] { "UserId", "ProjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FusionProjectId",
                table: "Projects",
                column: "FusionProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OriginalProjectId",
                table: "Projects",
                column: "OriginalProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RevisionDetails_RevisionId",
                table: "RevisionDetails",
                column: "RevisionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeismicAcquisitionAndProcessing_Exploration.Id",
                table: "SeismicAcquisitionAndProcessing",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SidetrackCostProfile_Exploration.Id",
                table: "SidetrackCostProfile",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubstructureCessationCostProfiles_Substructure.Id",
                table: "SubstructureCessationCostProfiles",
                column: "Substructure.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubstructureCostProfileOverride_Substructure.Id",
                table: "SubstructureCostProfileOverride",
                column: "Substructure.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubstructureCostProfiles_Substructure.Id",
                table: "SubstructureCostProfiles",
                column: "Substructure.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Substructures_ProjectId",
                table: "Substructures",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SurfCessationCostProfiles_Surf.Id",
                table: "SurfCessationCostProfiles",
                column: "Surf.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurfCostProfile_Surf.Id",
                table: "SurfCostProfile",
                column: "Surf.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurfCostProfileOverride_Surf.Id",
                table: "SurfCostProfileOverride",
                column: "Surf.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Surfs_ProjectId",
                table: "Surfs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TopsideCessationCostProfiles_Topside.Id",
                table: "TopsideCessationCostProfiles",
                column: "Topside.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopsideCostProfileOverride_Topside.Id",
                table: "TopsideCostProfileOverride",
                column: "Topside.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopsideCostProfiles_Topside.Id",
                table: "TopsideCostProfiles",
                column: "Topside.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topsides_ProjectId",
                table: "Topsides",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TotalFeasibilityAndConceptStudies_Case.Id",
                table: "TotalFeasibilityAndConceptStudies",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TotalFeasibilityAndConceptStudiesOverride_Case.Id",
                table: "TotalFeasibilityAndConceptStudiesOverride",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TotalFEEDStudies_Case.Id",
                table: "TotalFEEDStudies",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TotalFEEDStudiesOverride_Case.Id",
                table: "TotalFEEDStudiesOverride",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TotalOtherStudiesCostProfile_Case.Id",
                table: "TotalOtherStudiesCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransportCessationCostProfiles_Transport.Id",
                table: "TransportCessationCostProfiles",
                column: "Transport.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransportCostProfile_Transport.Id",
                table: "TransportCostProfile",
                column: "Transport.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransportCostProfileOverride_Transport.Id",
                table: "TransportCostProfileOverride",
                column: "Transport.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transports_ProjectId",
                table: "Transports",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterInjectorCostProfile_WellProject.Id",
                table: "WaterInjectorCostProfile",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WaterInjectorCostProfileOverride_WellProject.Id",
                table: "WaterInjectorCostProfileOverride",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WellInterventionCostProfile_Case.Id",
                table: "WellInterventionCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WellInterventionCostProfileOverride_Case.Id",
                table: "WellInterventionCostProfileOverride",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WellProjects_ProjectId",
                table: "WellProjects",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WellProjectWell_DrillingScheduleId",
                table: "WellProjectWell",
                column: "DrillingScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_WellProjectWell_WellId",
                table: "WellProjectWell",
                column: "WellId");

            migrationBuilder.CreateIndex(
                name: "IX_Wells_ProjectId",
                table: "Wells",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalOPEXCostProfile");

            migrationBuilder.DropTable(
                name: "AdditionalProductionProfileGas");

            migrationBuilder.DropTable(
                name: "AdditionalProductionProfileOil");

            migrationBuilder.DropTable(
                name: "AppraisalWellCostProfile");

            migrationBuilder.DropTable(
                name: "CalculatedTotalCostCostProfile");

            migrationBuilder.DropTable(
                name: "CalculatedTotalIncomeCostProfile");

            migrationBuilder.DropTable(
                name: "CessationOffshoreFacilitiesCost");

            migrationBuilder.DropTable(
                name: "CessationOffshoreFacilitiesCostOverride");

            migrationBuilder.DropTable(
                name: "CessationOnshoreFacilitiesCostProfile");

            migrationBuilder.DropTable(
                name: "CessationWellsCost");

            migrationBuilder.DropTable(
                name: "CessationWellsCostOverride");

            migrationBuilder.DropTable(
                name: "ChangeLogs");

            migrationBuilder.DropTable(
                name: "Co2Emissions");

            migrationBuilder.DropTable(
                name: "Co2EmissionsOverride");

            migrationBuilder.DropTable(
                name: "CountryOfficeCost");

            migrationBuilder.DropTable(
                name: "DeferredGasProduction");

            migrationBuilder.DropTable(
                name: "DeferredOilProduction");

            migrationBuilder.DropTable(
                name: "DevelopmentOperationalWellCosts");

            migrationBuilder.DropTable(
                name: "ExceptionLogs");

            migrationBuilder.DropTable(
                name: "ExplorationOperationalWellCosts");

            migrationBuilder.DropTable(
                name: "ExplorationWell");

            migrationBuilder.DropTable(
                name: "ExplorationWellCostProfile");

            migrationBuilder.DropTable(
                name: "FuelFlaringAndLosses");

            migrationBuilder.DropTable(
                name: "FuelFlaringAndLossesOverride");

            migrationBuilder.DropTable(
                name: "GAndGAdminCost");

            migrationBuilder.DropTable(
                name: "GAndGAdminCostOverride");

            migrationBuilder.DropTable(
                name: "GasInjectorCostProfile");

            migrationBuilder.DropTable(
                name: "GasInjectorCostProfileOverride");

            migrationBuilder.DropTable(
                name: "GasProducerCostProfile");

            migrationBuilder.DropTable(
                name: "GasProducerCostProfileOverride");

            migrationBuilder.DropTable(
                name: "HistoricCostCostProfile");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "ImportedElectricity");

            migrationBuilder.DropTable(
                name: "ImportedElectricityOverride");

            migrationBuilder.DropTable(
                name: "LazyLoadingOccurrences");

            migrationBuilder.DropTable(
                name: "NetSalesGas");

            migrationBuilder.DropTable(
                name: "NetSalesGasOverride");

            migrationBuilder.DropTable(
                name: "OffshoreFacilitiesOperationsCostProfile");

            migrationBuilder.DropTable(
                name: "OffshoreFacilitiesOperationsCostProfileOverride");

            migrationBuilder.DropTable(
                name: "OilProducerCostProfile");

            migrationBuilder.DropTable(
                name: "OilProducerCostProfileOverride");

            migrationBuilder.DropTable(
                name: "OnshorePowerSupplyCostProfile");

            migrationBuilder.DropTable(
                name: "OnshorePowerSupplyCostProfileOverride");

            migrationBuilder.DropTable(
                name: "OnshoreRelatedOPEXCostProfile");

            migrationBuilder.DropTable(
                name: "ProductionProfileGas");

            migrationBuilder.DropTable(
                name: "ProductionProfileNgl");

            migrationBuilder.DropTable(
                name: "ProductionProfileOil");

            migrationBuilder.DropTable(
                name: "ProductionProfileWater");

            migrationBuilder.DropTable(
                name: "ProductionProfileWaterInjection");

            migrationBuilder.DropTable(
                name: "ProjectMembers");

            migrationBuilder.DropTable(
                name: "RequestLogs");

            migrationBuilder.DropTable(
                name: "RevisionDetails");

            migrationBuilder.DropTable(
                name: "SeismicAcquisitionAndProcessing");

            migrationBuilder.DropTable(
                name: "SidetrackCostProfile");

            migrationBuilder.DropTable(
                name: "SubstructureCessationCostProfiles");

            migrationBuilder.DropTable(
                name: "SubstructureCostProfileOverride");

            migrationBuilder.DropTable(
                name: "SubstructureCostProfiles");

            migrationBuilder.DropTable(
                name: "SurfCessationCostProfiles");

            migrationBuilder.DropTable(
                name: "SurfCostProfile");

            migrationBuilder.DropTable(
                name: "SurfCostProfileOverride");

            migrationBuilder.DropTable(
                name: "TopsideCessationCostProfiles");

            migrationBuilder.DropTable(
                name: "TopsideCostProfileOverride");

            migrationBuilder.DropTable(
                name: "TopsideCostProfiles");

            migrationBuilder.DropTable(
                name: "TotalFeasibilityAndConceptStudies");

            migrationBuilder.DropTable(
                name: "TotalFeasibilityAndConceptStudiesOverride");

            migrationBuilder.DropTable(
                name: "TotalFEEDStudies");

            migrationBuilder.DropTable(
                name: "TotalFEEDStudiesOverride");

            migrationBuilder.DropTable(
                name: "TotalOtherStudiesCostProfile");

            migrationBuilder.DropTable(
                name: "TransportCessationCostProfiles");

            migrationBuilder.DropTable(
                name: "TransportCostProfile");

            migrationBuilder.DropTable(
                name: "TransportCostProfileOverride");

            migrationBuilder.DropTable(
                name: "WaterInjectorCostProfile");

            migrationBuilder.DropTable(
                name: "WaterInjectorCostProfileOverride");

            migrationBuilder.DropTable(
                name: "WellInterventionCostProfile");

            migrationBuilder.DropTable(
                name: "WellInterventionCostProfileOverride");

            migrationBuilder.DropTable(
                name: "WellProjectWell");

            migrationBuilder.DropTable(
                name: "Cases");

            migrationBuilder.DropTable(
                name: "DrillingSchedule");

            migrationBuilder.DropTable(
                name: "Wells");

            migrationBuilder.DropTable(
                name: "DrainageStrategies");

            migrationBuilder.DropTable(
                name: "Explorations");

            migrationBuilder.DropTable(
                name: "OnshorePowerSupplies");

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
