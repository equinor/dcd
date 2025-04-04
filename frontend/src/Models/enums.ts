/* This file is autogenerated by the backend. Do not modify manually. */

export enum ProfileTypes {
    AdditionalOpexCostProfile = "AdditionalOpexCostProfile",
    AdditionalProductionProfileGas = "AdditionalProductionProfileGas",
    AdditionalProductionProfileOil = "AdditionalProductionProfileOil",
    AppraisalWellCostProfile = "AppraisalWellCostProfile",
    AppraisalWellCostProfileOverride = "AppraisalWellCostProfileOverride",
    CalculatedDiscountedCashflowService = "CalculatedDiscountedCashflowService",
    CalculatedTotalCostCostProfileUsd = "CalculatedTotalCostCostProfileUsd",
    CalculatedTotalIncomeCostProfileUsd = "CalculatedTotalIncomeCostProfileUsd",
    CessationOffshoreFacilitiesCost = "CessationOffshoreFacilitiesCost",
    CessationOffshoreFacilitiesCostOverride = "CessationOffshoreFacilitiesCostOverride",
    CessationOnshoreFacilitiesCostProfile = "CessationOnshoreFacilitiesCostProfile",
    CessationWellsCost = "CessationWellsCost",
    CessationWellsCostOverride = "CessationWellsCostOverride",
    Co2Emissions = "Co2Emissions",
    Co2EmissionsOverride = "Co2EmissionsOverride",
    Co2Intensity = "Co2Intensity",
    Co2IntensityOverride = "Co2IntensityOverride",
    CondensateProduction = "CondensateProduction",
    CondensateProductionOverride = "CondensateProductionOverride",
    CountryOfficeCost = "CountryOfficeCost",
    DeferredGasProduction = "DeferredGasProduction",
    DeferredOilProduction = "DeferredOilProduction",
    DevelopmentRigMobDemob = "DevelopmentRigMobDemob",
    DevelopmentRigMobDemobOverride = "DevelopmentRigMobDemobOverride",
    DevelopmentRigUpgradingCostProfile = "DevelopmentRigUpgradingCostProfile",
    DevelopmentRigUpgradingCostProfileOverride = "DevelopmentRigUpgradingCostProfileOverride",
    ExplorationRigMobDemob = "ExplorationRigMobDemob",
    ExplorationRigMobDemobOverride = "ExplorationRigMobDemobOverride",
    ExplorationRigUpgradingCostProfile = "ExplorationRigUpgradingCostProfile",
    ExplorationRigUpgradingCostProfileOverride = "ExplorationRigUpgradingCostProfileOverride",
    ExplorationWellCostProfile = "ExplorationWellCostProfile",
    ExplorationWellCostProfileOverride = "ExplorationWellCostProfileOverride",
    FuelFlaringAndLosses = "FuelFlaringAndLosses",
    FuelFlaringAndLossesOverride = "FuelFlaringAndLossesOverride",
    GAndGAdminCost = "GAndGAdminCost",
    GAndGAdminCostOverride = "GAndGAdminCostOverride",
    GasInjectorCostProfile = "GasInjectorCostProfile",
    GasInjectorCostProfileOverride = "GasInjectorCostProfileOverride",
    GasProducerCostProfile = "GasProducerCostProfile",
    GasProducerCostProfileOverride = "GasProducerCostProfileOverride",
    HistoricCostCostProfile = "HistoricCostCostProfile",
    ImportedElectricity = "ImportedElectricity",
    ImportedElectricityOverride = "ImportedElectricityOverride",
    NetSalesGas = "NetSalesGas",
    NetSalesGasOverride = "NetSalesGasOverride",
    OffshoreFacilitiesOperationsCostProfile = "OffshoreFacilitiesOperationsCostProfile",
    OffshoreFacilitiesOperationsCostProfileOverride = "OffshoreFacilitiesOperationsCostProfileOverride",
    OilProducerCostProfile = "OilProducerCostProfile",
    OilProducerCostProfileOverride = "OilProducerCostProfileOverride",
    OnshorePowerSupplyCostProfile = "OnshorePowerSupplyCostProfile",
    OnshorePowerSupplyCostProfileOverride = "OnshorePowerSupplyCostProfileOverride",
    OnshoreRelatedOpexCostProfile = "OnshoreRelatedOpexCostProfile",
    ProductionProfileGas = "ProductionProfileGas",
    ProductionProfileNgl = "ProductionProfileNgl",
    ProductionProfileNglOverride = "ProductionProfileNglOverride",
    ProductionProfileOil = "ProductionProfileOil",
    ProductionProfileWater = "ProductionProfileWater",
    ProductionProfileWaterInjection = "ProductionProfileWaterInjection",
    ProjectSpecificDrillingCostProfile = "ProjectSpecificDrillingCostProfile",
    SeismicAcquisitionAndProcessing = "SeismicAcquisitionAndProcessing",
    SidetrackCostProfile = "SidetrackCostProfile",
    SidetrackCostProfileOverride = "SidetrackCostProfileOverride",
    SubstructureCessationCostProfile = "SubstructureCessationCostProfile",
    SubstructureCostProfile = "SubstructureCostProfile",
    SubstructureCostProfileOverride = "SubstructureCostProfileOverride",
    SurfCessationCostProfile = "SurfCessationCostProfile",
    SurfCostProfile = "SurfCostProfile",
    SurfCostProfileOverride = "SurfCostProfileOverride",
    TopsideCessationCostProfile = "TopsideCessationCostProfile",
    TopsideCostProfile = "TopsideCostProfile",
    TopsideCostProfileOverride = "TopsideCostProfileOverride",
    TotalExportedVolumes = "TotalExportedVolumes",
    TotalExportedVolumesOverride = "TotalExportedVolumesOverride",
    TotalFeasibilityAndConceptStudies = "TotalFeasibilityAndConceptStudies",
    TotalFeasibilityAndConceptStudiesOverride = "TotalFeasibilityAndConceptStudiesOverride",
    TotalFeedStudies = "TotalFeedStudies",
    TotalFeedStudiesOverride = "TotalFeedStudiesOverride",
    TotalOtherStudiesCostProfile = "TotalOtherStudiesCostProfile",
    TransportCessationCostProfile = "TransportCessationCostProfile",
    TransportCostProfile = "TransportCostProfile",
    TransportCostProfileOverride = "TransportCostProfileOverride",
    WaterInjectorCostProfile = "WaterInjectorCostProfile",
    WaterInjectorCostProfileOverride = "WaterInjectorCostProfileOverride",
    WellInterventionCostProfile = "WellInterventionCostProfile",
    WellInterventionCostProfileOverride = "WellInterventionCostProfileOverride",
}

export enum ArtificialLift {
    NoArtificialLift = 0,
    GasLift = 1,
    ElectricalSubmergedPumps = 2,
    SubseaBoosterPumps = 3,
}

export enum CampaignCostType {
    RigUpgrading = 0,
    RigMobDemob = 1,
}

export enum CampaignType {
    DevelopmentCampaign = 1,
    ExplorationCampaign = 2,
}

export enum ChangeLogCategory {
    None = 0,
    WellCostTab = 1,
    AccessManagementTab = 2,
    SettingsTab = 3,
    ProjectOverviewTab = 4,
    ProspTab = 5,
}

export enum Concept {
    NoConcept = 0,
    TieBack = 1,
    Jacket = 2,
    Gbs = 3,
    Tlp = 4,
    Spar = 5,
    Semi = 6,
    CircularBarge = 7,
    Barge = 8,
    Fpso = 9,
    Tanker = 10,
    JackUp = 11,
    SubseaToShore = 12,
}

export enum Currency {
    Nok = 1,
    Usd = 2,
}

export enum GasSolution {
    Export = 0,
    Injection = 1,
}

export enum InternalProjectPhase {
    APbo = 0,
    Bor = 1,
    VPbo = 2,
}

export enum Maturity {
    A = 0,
    B = 1,
    C = 2,
    D = 3,
}

export enum NoAccessReason {
    ProjectDoesNotExist = 1,
    ClassificationInternal = 2,
    ClassificationRestricted = 3,
    ClassificationConfidential = 4,
}

export enum PhysUnit {
    Si = 0,
    OilField = 1,
}

export enum ProductionFlowline {
    NoProductionFlowline = 0,
    Carbon = 1,
    SsClad = 2,
    Cr13 = 3,
    CarbonInsulation = 4,
    SsCladInsulation = 5,
    Cr13Insulation = 6,
    CarbonInsulationDeh = 7,
    SsCladInsulationDeh = 8,
    Cr13InsulationDeh = 9,
    CarbonPip = 10,
    SsCladPip = 11,
    Cr13Pip = 12,
    HdpeLinedCs = 13,
}

export enum ProductionStrategyOverview {
    Depletion = 0,
    WaterInjection = 1,
    GasInjection = 2,
    Wag = 3,
    Mixed = 4,
}

export enum ProjectCategory {
    Null = 0,
    Brownfield = 1,
    Cessation = 2,
    DrillingUpgrade = 3,
    Onshore = 4,
    Pipeline = 5,
    PlatformFpso = 6,
    Subsea = 7,
    Solar = 8,
    Co2Storage = 9,
    Efuel = 10,
    Nuclear = 11,
    Co2Capture = 12,
    Fpso = 13,
    Hydrogen = 14,
    Hse = 15,
    OffshoreWind = 16,
    Platform = 17,
    PowerFromShore = 18,
    TieIn = 19,
    RenewableOther = 20,
    Ccs = 21,
}

export enum ProjectClassification {
    Open = 0,
    Internal = 1,
    Restricted = 2,
    Confidential = 3,
}

export enum ProjectMemberRole {
    Observer = 0,
    Editor = 1,
}

export enum ProjectPhase {
    Null = 0,
    BidPreparations = 1,
    BusinessIdentification = 2,
    BusinessPlanning = 3,
    ConceptPlanning = 4,
    ConcessionNegotiations = 5,
    Definition = 6,
    Execution = 7,
    Operation = 8,
    ScreeningBusinessOpportunities = 9,
}

export enum Source {
    ConceptApp = 0,
    Prosp = 1,
}

export enum WellCategory {
    OilProducer = 0,
    GasProducer = 1,
    WaterInjector = 2,
    GasInjector = 3,
    ExplorationWell = 4,
    AppraisalWell = 5,
    Sidetrack = 6,
}
