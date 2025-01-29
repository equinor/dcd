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
                name: "BackgroundJobLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutionDurationInMilliseconds = table.Column<long>(type: "bigint", nullable: false),
                    TimestampUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackgroundJobLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BackgroundJobMachineInstanceLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastSeenUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsJobRunner = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackgroundJobMachineInstanceLogs", x => x.Id);
                });

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
                name: "CompletedRecalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CalculationLengthInMilliseconds = table.Column<int>(type: "int", nullable: false),
                    DebugLog = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedRecalculations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DrillingSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "FrontendExceptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DetailsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrontendExceptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LazyLoadingOccurrences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullStackTrace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimestampUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LazyLoadingOccurrences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PendingRecalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingRecalculations", x => x.Id);
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
                    OilPriceUSD = table.Column<double>(type: "float", nullable: false),
                    GasPriceNOK = table.Column<double>(type: "float", nullable: false),
                    DiscountRate = table.Column<double>(type: "float", nullable: false),
                    ExchangeRateUSDToNOK = table.Column<double>(type: "float", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    PluggingAndAbandonment = table.Column<double>(type: "float", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    GasSolution = table.Column<int>(type: "int", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    AppraisalProjectDrillingCosts = table.Column<double>(type: "float", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    Currency = table.Column<int>(type: "int", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    DG4Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    FromOrgChart = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    Arena = table.Column<bool>(type: "bit", nullable: false),
                    Mdqc = table.Column<bool>(type: "bit", nullable: false),
                    Classification = table.Column<int>(type: "int", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    DG4Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    DG4Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    PeakElectricityImported = table.Column<double>(type: "float", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    DG4Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    Currency = table.Column<int>(type: "int", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    WellInterventionCost = table.Column<double>(type: "float", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "ExplorationWell",
                columns: table => new
                {
                    WellId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrillingScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    DrillingScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Images_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeSeriesProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSeriesProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeSeriesProfiles_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BackgroundJobLogs_TimestampUtc",
                table: "BackgroundJobLogs",
                column: "TimestampUtc");

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
                name: "IX_DevelopmentOperationalWellCosts_ProjectId",
                table: "DevelopmentOperationalWellCosts",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DrainageStrategies_ProjectId",
                table: "DrainageStrategies",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionLogs_UtcTimestamp",
                table: "ExceptionLogs",
                column: "UtcTimestamp");

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
                name: "IX_FrontendExceptions_CreatedUtc",
                table: "FrontendExceptions",
                column: "CreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CaseId",
                table: "Images",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProjectId",
                table: "Images",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_OnshorePowerSupplies_ProjectId",
                table: "OnshorePowerSupplies",
                column: "ProjectId");

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
                name: "IX_RequestLogs_RequestStartUtc",
                table: "RequestLogs",
                column: "RequestStartUtc");

            migrationBuilder.CreateIndex(
                name: "IX_RevisionDetails_RevisionId",
                table: "RevisionDetails",
                column: "RevisionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Substructures_ProjectId",
                table: "Substructures",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Surfs_ProjectId",
                table: "Surfs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSeriesProfiles_CaseId_ProfileType",
                table: "TimeSeriesProfiles",
                columns: new[] { "CaseId", "ProfileType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topsides_ProjectId",
                table: "Topsides",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Transports_ProjectId",
                table: "Transports",
                column: "ProjectId");

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
                name: "BackgroundJobLogs");

            migrationBuilder.DropTable(
                name: "BackgroundJobMachineInstanceLogs");

            migrationBuilder.DropTable(
                name: "ChangeLogs");

            migrationBuilder.DropTable(
                name: "CompletedRecalculations");

            migrationBuilder.DropTable(
                name: "DevelopmentOperationalWellCosts");

            migrationBuilder.DropTable(
                name: "ExceptionLogs");

            migrationBuilder.DropTable(
                name: "ExplorationOperationalWellCosts");

            migrationBuilder.DropTable(
                name: "ExplorationWell");

            migrationBuilder.DropTable(
                name: "FrontendExceptions");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "LazyLoadingOccurrences");

            migrationBuilder.DropTable(
                name: "PendingRecalculations");

            migrationBuilder.DropTable(
                name: "ProjectMembers");

            migrationBuilder.DropTable(
                name: "RequestLogs");

            migrationBuilder.DropTable(
                name: "RevisionDetails");

            migrationBuilder.DropTable(
                name: "TimeSeriesProfiles");

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
