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
                    ParentEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    CalculationLengthInMilliseconds = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedRecalculations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExceptionLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimestampUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RigUpgradingCostStartYear = table.Column<int>(type: "int", nullable: false),
                    RigUpgradingCostInternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RigMobDemobCostStartYear = table.Column<int>(type: "int", nullable: false),
                    RigMobDemobCostInternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CampaignType = table.Column<int>(type: "int", nullable: false),
                    RigUpgradingCost = table.Column<double>(type: "float", nullable: false),
                    RigMobDemobCost = table.Column<double>(type: "float", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CampaignWells",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignWells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignWells_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CaseImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Archived = table.Column<bool>(type: "bit", nullable: false),
                    SharepointFileId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SharepointFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SharepointFileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DgaDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DgbDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DgcDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApboDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BorDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VpboDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Dg0Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Dg1Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Dg2Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Dg3Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Dg4Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArtificialLift = table.Column<int>(type: "int", nullable: false),
                    ProductionStrategyOverview = table.Column<int>(type: "int", nullable: false),
                    ProducerCount = table.Column<int>(type: "int", nullable: false),
                    GasInjectorCount = table.Column<int>(type: "int", nullable: false),
                    WaterInjectorCount = table.Column<int>(type: "int", nullable: false),
                    FacilitiesAvailability = table.Column<double>(type: "float", nullable: false),
                    CapexFactorFeasibilityStudies = table.Column<double>(type: "float", nullable: false),
                    CapexFactorFeedStudies = table.Column<double>(type: "float", nullable: false),
                    InitialYearsWithoutWellInterventionCost = table.Column<double>(type: "float", nullable: false),
                    FinalYearsWithoutWellInterventionCost = table.Column<double>(type: "float", nullable: false),
                    Npv = table.Column<double>(type: "float", nullable: false),
                    NpvOverride = table.Column<double>(type: "float", nullable: true),
                    BreakEven = table.Column<double>(type: "float", nullable: false),
                    BreakEvenOverride = table.Column<double>(type: "float", nullable: true),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AverageCo2Intensity = table.Column<double>(type: "float", nullable: false),
                    DiscountedCashflow = table.Column<double>(type: "float", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurfId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubstructureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopsideId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OnshorePowerSupplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalCaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cases_Cases_OriginalCaseId",
                        column: x => x.OriginalCaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrainageStrategies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NglYield = table.Column<double>(type: "float", nullable: false),
                    CondensateYield = table.Column<double>(type: "float", nullable: false),
                    GasShrinkageFactor = table.Column<double>(type: "float", nullable: false),
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
                        name: "FK_DrainageStrategies_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnshorePowerSupplies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CostYear = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
                    ProspVersion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnshorePowerSupplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnshorePowerSupplies_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferenceCaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRevision = table.Column<bool>(type: "bit", nullable: false),
                    FusionProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    Co2RemovedFromGas = table.Column<double>(type: "float", nullable: false),
                    Co2EmissionFromFuelGas = table.Column<double>(type: "float", nullable: false),
                    FlaredGasPerProducedVolume = table.Column<double>(type: "float", nullable: false),
                    Co2EmissionsFromFlaredGas = table.Column<double>(type: "float", nullable: false),
                    Co2Vented = table.Column<double>(type: "float", nullable: false),
                    DailyEmissionFromDrillingRig = table.Column<double>(type: "float", nullable: false),
                    AverageDevelopmentDrillingDays = table.Column<double>(type: "float", nullable: false),
                    OilPriceUsd = table.Column<double>(type: "float", nullable: false),
                    GasPriceNok = table.Column<double>(type: "float", nullable: false),
                    NglPriceUsd = table.Column<double>(type: "float", nullable: false),
                    DiscountRate = table.Column<double>(type: "float", nullable: false),
                    ExchangeRateUsdToNok = table.Column<double>(type: "float", nullable: false),
                    NpvYear = table.Column<int>(type: "int", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Cases_ReferenceCaseId",
                        column: x => x.ReferenceCaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Projects_OriginalProjectId",
                        column: x => x.OriginalProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Substructures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DryWeight = table.Column<double>(type: "float", nullable: false),
                    Maturity = table.Column<int>(type: "int", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostYear = table.Column<int>(type: "int", nullable: false),
                    ProspVersion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Source = table.Column<int>(type: "int", nullable: false),
                    Concept = table.Column<int>(type: "int", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Substructures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Substructures_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Surfs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    CostYear = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
                    ProspVersion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surfs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Surfs_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
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
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Topsides",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DryWeight = table.Column<double>(type: "float", nullable: false),
                    OilCapacity = table.Column<double>(type: "float", nullable: false),
                    GasCapacity = table.Column<double>(type: "float", nullable: false),
                    WaterInjectionCapacity = table.Column<double>(type: "float", nullable: false),
                    ArtificialLift = table.Column<int>(type: "int", nullable: false),
                    Maturity = table.Column<int>(type: "int", nullable: false),
                    FuelConsumption = table.Column<double>(type: "float", nullable: false),
                    FlaredGas = table.Column<double>(type: "float", nullable: false),
                    ProducerCount = table.Column<int>(type: "int", nullable: false),
                    GasInjectorCount = table.Column<int>(type: "int", nullable: false),
                    WaterInjectorCount = table.Column<int>(type: "int", nullable: false),
                    Co2ShareOilProfile = table.Column<double>(type: "float", nullable: false),
                    Co2ShareGasProfile = table.Column<double>(type: "float", nullable: false),
                    Co2ShareWaterInjectionProfile = table.Column<double>(type: "float", nullable: false),
                    Co2OnMaxOilProfile = table.Column<double>(type: "float", nullable: false),
                    Co2OnMaxGasProfile = table.Column<double>(type: "float", nullable: false),
                    Co2OnMaxWaterInjectionProfile = table.Column<double>(type: "float", nullable: false),
                    CostYear = table.Column<int>(type: "int", nullable: false),
                    ProspVersion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Source = table.Column<int>(type: "int", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                        name: "FK_Topsides_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GasExportPipelineLength = table.Column<double>(type: "float", nullable: false),
                    OilExportPipelineLength = table.Column<double>(type: "float", nullable: false),
                    Maturity = table.Column<int>(type: "int", nullable: false),
                    CostYear = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
                    ProspVersion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transports_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "ProjectImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectImages_Projects_ProjectId",
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
                    AzureAdUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_BackgroundJobLogs_TimestampUtc",
                table: "BackgroundJobLogs",
                column: "TimestampUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_CaseId",
                table: "Campaigns",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignWells_CampaignId",
                table: "CampaignWells",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignWells_WellId_CampaignId",
                table: "CampaignWells",
                columns: new[] { "WellId", "CampaignId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaseImages_CaseId",
                table: "CaseImages",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_OriginalCaseId",
                table: "Cases",
                column: "OriginalCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_ProjectId",
                table: "Cases",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_EntityId",
                table: "ChangeLogs",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_EntityName",
                table: "ChangeLogs",
                column: "EntityName");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_ParentEntityId",
                table: "ChangeLogs",
                column: "ParentEntityId");

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
                name: "IX_DrainageStrategies_CaseId",
                table: "DrainageStrategies",
                column: "CaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionLogs_TimestampUtc",
                table: "ExceptionLogs",
                column: "TimestampUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationOperationalWellCosts_ProjectId",
                table: "ExplorationOperationalWellCosts",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnshorePowerSupplies_CaseId",
                table: "OnshorePowerSupplies",
                column: "CaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectImages_ProjectId",
                table: "ProjectImages",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_AzureAdUserId_ProjectId",
                table: "ProjectMembers",
                columns: new[] { "AzureAdUserId", "ProjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ProjectId",
                table: "ProjectMembers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FusionProjectId",
                table: "Projects",
                column: "FusionProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OriginalProjectId",
                table: "Projects",
                column: "OriginalProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ReferenceCaseId",
                table: "Projects",
                column: "ReferenceCaseId");

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
                name: "IX_Substructures_CaseId",
                table: "Substructures",
                column: "CaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Surfs_CaseId",
                table: "Surfs",
                column: "CaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeSeriesProfiles_CaseId_ProfileType",
                table: "TimeSeriesProfiles",
                columns: new[] { "CaseId", "ProfileType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topsides_CaseId",
                table: "Topsides",
                column: "CaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transports_CaseId",
                table: "Transports",
                column: "CaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wells_ProjectId",
                table: "Wells",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Cases_CaseId",
                table: "Campaigns",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignWells_Wells_WellId",
                table: "CampaignWells",
                column: "WellId",
                principalTable: "Wells",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseImages_Cases_CaseId",
                table: "CaseImages",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Projects_ProjectId",
                table: "Cases",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Cases_ReferenceCaseId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "BackgroundJobLogs");

            migrationBuilder.DropTable(
                name: "BackgroundJobMachineInstanceLogs");

            migrationBuilder.DropTable(
                name: "CampaignWells");

            migrationBuilder.DropTable(
                name: "CaseImages");

            migrationBuilder.DropTable(
                name: "ChangeLogs");

            migrationBuilder.DropTable(
                name: "CompletedRecalculations");

            migrationBuilder.DropTable(
                name: "DevelopmentOperationalWellCosts");

            migrationBuilder.DropTable(
                name: "DrainageStrategies");

            migrationBuilder.DropTable(
                name: "ExceptionLogs");

            migrationBuilder.DropTable(
                name: "ExplorationOperationalWellCosts");

            migrationBuilder.DropTable(
                name: "OnshorePowerSupplies");

            migrationBuilder.DropTable(
                name: "PendingRecalculations");

            migrationBuilder.DropTable(
                name: "ProjectImages");

            migrationBuilder.DropTable(
                name: "ProjectMembers");

            migrationBuilder.DropTable(
                name: "RequestLogs");

            migrationBuilder.DropTable(
                name: "RevisionDetails");

            migrationBuilder.DropTable(
                name: "Substructures");

            migrationBuilder.DropTable(
                name: "Surfs");

            migrationBuilder.DropTable(
                name: "TimeSeriesProfiles");

            migrationBuilder.DropTable(
                name: "Topsides");

            migrationBuilder.DropTable(
                name: "Transports");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "Wells");

            migrationBuilder.DropTable(
                name: "Cases");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
