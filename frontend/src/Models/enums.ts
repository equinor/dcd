/* This file is autogenerated by the backend. Do not modify manually. */

export enum ProfileTypes {
    AdditionalOPEXCostProfile = "AdditionalOPEXCostProfile",
    AdditionalProductionProfileGas = "AdditionalProductionProfileGas",
    AdditionalProductionProfileOil = "AdditionalProductionProfileOil",
    AppraisalWellCostProfile = "AppraisalWellCostProfile",
    CalculatedTotalCostCostProfile = "CalculatedTotalCostCostProfile",
    CalculatedTotalIncomeCostProfile = "CalculatedTotalIncomeCostProfile",
    CessationOffshoreFacilitiesCost = "CessationOffshoreFacilitiesCost",
    CessationOffshoreFacilitiesCostOverride = "CessationOffshoreFacilitiesCostOverride",
    CessationOnshoreFacilitiesCostProfile = "CessationOnshoreFacilitiesCostProfile",
    CessationWellsCost = "CessationWellsCost",
    CessationWellsCostOverride = "CessationWellsCostOverride",
    Co2Emissions = "Co2Emissions",
    Co2EmissionsOverride = "Co2EmissionsOverride",
    Co2Intensity = "Co2Intensity",
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
    OnshoreRelatedOPEXCostProfile = "OnshoreRelatedOPEXCostProfile",
    ProductionProfileGas = "ProductionProfileGas",
    ProductionProfileNgl = "ProductionProfileNgl",
    ProductionProfileOil = "ProductionProfileOil",
    ProductionProfileWater = "ProductionProfileWater",
    ProductionProfileWaterInjection = "ProductionProfileWaterInjection",
    ProjectSpecificDrillingCostProfile = "ProjectSpecificDrillingCostProfile",
    SeismicAcquisitionAndProcessing = "SeismicAcquisitionAndProcessing",
    SidetrackCostProfile = "SidetrackCostProfile",
    SubstructureCessationCostProfile = "SubstructureCessationCostProfile",
    SubstructureCostProfile = "SubstructureCostProfile",
    SubstructureCostProfileOverride = "SubstructureCostProfileOverride",
    SurfCessationCostProfile = "SurfCessationCostProfile",
    SurfCostProfile = "SurfCostProfile",
    SurfCostProfileOverride = "SurfCostProfileOverride",
    TopsideCessationCostProfile = "TopsideCessationCostProfile",
    TopsideCostProfile = "TopsideCostProfile",
    TopsideCostProfileOverride = "TopsideCostProfileOverride",
    TotalFeasibilityAndConceptStudies = "TotalFeasibilityAndConceptStudies",
    TotalFeasibilityAndConceptStudiesOverride = "TotalFeasibilityAndConceptStudiesOverride",
    TotalFEEDStudies = "TotalFEEDStudies",
    TotalFEEDStudiesOverride = "TotalFEEDStudiesOverride",
    TotalOtherStudiesCostProfile = "TotalOtherStudiesCostProfile",
    TransportCessationCostProfile = "TransportCessationCostProfile",
    TransportCostProfile = "TransportCostProfile",
    TransportCostProfileOverride = "TransportCostProfileOverride",
    WaterInjectorCostProfile = "WaterInjectorCostProfile",
    WaterInjectorCostProfileOverride = "WaterInjectorCostProfileOverride",
    WellInterventionCostProfile = "WellInterventionCostProfile",
    WellInterventionCostProfileOverride = "WellInterventionCostProfileOverride",
}

export enum CampaignType {
    DevelopmentCampaign = 1,
    ExplorationCampaign = 2,
}

export enum Concept {
    NO_CONCEPT = 0,
    TIE_BACK = 1,
    JACKET = 2,
    GBS = 3,
    TLP = 4,
    SPAR = 5,
    SEMI = 6,
    CIRCULAR_BARGE = 7,
    BARGE = 8,
    FPSO = 9,
    TANKER = 10,
    JACK_UP = 11,
    SUBSEA_TO_SHORE = 12,
}

export enum Currency {
    NOK = 1,
    USD = 2,
}

export enum NoAccessReason {
    ProjectDoesNotExist = 1,
    ClassificationInternal = 2,
    ClassificationRestricted = 3,
    ClassificationConfidential = 4,
}

export enum PhysUnit {
    SI = 0,
    OilField = 1,
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
