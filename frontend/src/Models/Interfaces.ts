import { ITimeSeries } from "./ITimeSeries"

export type ResourceName =
    "case" |
    "topside" |
    "surf" |
    "transport" |
    "substructure" |
    "drainageStrategy" |
    "fuelFlaringAndLossesOverride" |
    "wellProject" |
    "exploration" | ProfileNames

export type ProfileNames = "cessationWellsCostOverride" | "cessationOffshoreFacilitiesCostOverride" | "totalFeasibilityAndConceptStudiesOverride" |
    "totalFEEDStudiesOverride" | "historicCostCostProfile" | "wellInterventionCostProfileOverride" | "offshoreFacilitiesOperationsCostProfileOverride" |
    "onshoreRelatedOPEXCostProfile" | "additionalOPEXCostProfile" | "totalOtherStudies" |
    "topsideCostProfileOverride" |
    "surfCostProfileOverride" |
    "transportCostProfileOverride" |
    "substructureCostProfileOverride" |
    "productionProfileOil" | "productionProfileGas" | "productionProfileWater" | "productionProfileWaterInjection" | "productionProfileFuelFlaringAndLossesOverride" |
    "productionProfileNetSalesGasOverride" | "productionProfileImportedElectricityOverride" | "deferredOilProduction" | "deferredGasProduction" |
    "netSalesGasOverride" | "co2EmissionsOverride" | "importedElectricityOverride" | "deferredOilProduction" | "deferredGasProduction" |
    "oilProducerCostProfileOverride" | "gasProducerCostProfileOverride" | "waterInjectorCostProfileOverride" | "gasInjectorCostProfileOverride" |
    "seismicAcquisitionAndProcessing" | "countryOfficeCost"

export type ResourceObject =
    Components.Schemas.TopsideDto |
    Components.Schemas.SurfDto |
    Components.Schemas.SubstructureDto |
    Components.Schemas.TransportDto |
    Components.Schemas.CaseDto |
    Components.Schemas.DrainageStrategyDto |
    Components.Schemas.WellProjectDto |
    Components.Schemas.ExplorationDto |
    ProfileObject

export type ProfileObject =
    Components.Schemas.CessationWellsCostOverrideDto |
    Components.Schemas.CessationOffshoreFacilitiesCostOverrideDto |
    Components.Schemas.TotalFeasibilityAndConceptStudiesOverrideDto |
    Components.Schemas.TotalFEEDStudiesOverrideDto |
    Components.Schemas.HistoricCostCostProfileDto |
    Components.Schemas.WellInterventionCostProfileOverrideDto |
    Components.Schemas.OffshoreFacilitiesOperationsCostProfileOverrideDto |
    Components.Schemas.OnshoreRelatedOPEXCostProfileDto |
    Components.Schemas.AdditionalOPEXCostProfileDto |
    Components.Schemas.TopsideCostProfileOverrideDto |
    Components.Schemas.SurfCostProfileOverrideDto |
    Components.Schemas.TransportCostProfileOverrideDto |
    Components.Schemas.SubstructureCostProfileOverrideDto |
    Components.Schemas.ProductionProfileOilDto |
    Components.Schemas.ProductionProfileGasDto |
    Components.Schemas.ProductionProfileWaterDto |
    Components.Schemas.ProductionProfileWaterInjectionDto |
    Components.Schemas.NetSalesGasOverrideDto |
    Components.Schemas.Co2EmissionsOverrideDto |
    Components.Schemas.ImportedElectricityOverrideDto |
    Components.Schemas.DeferredOilProductionDto |
    Components.Schemas.DeferredGasProductionDto |
    Components.Schemas.OilProducerCostProfileOverrideDto |
    Components.Schemas.GasProducerCostProfileOverrideDto |
    Components.Schemas.WaterInjectorCostProfileOverrideDto |
    Components.Schemas.GasInjectorCostProfileOverrideDto |
    Components.Schemas.SeismicAcquisitionAndProcessingDto |
    Components.Schemas.CountryOfficeCostDto;

export type ResourcePropertyKey =
    keyof Components.Schemas.TopsideDto |
    keyof Components.Schemas.SurfDto |
    keyof Components.Schemas.SubstructureDto |
    keyof Components.Schemas.TransportDto |
    keyof Components.Schemas.CaseDto |
    keyof Components.Schemas.DrainageStrategyDto |
    keyof Components.Schemas.WellProjectDto |
    keyof Components.Schemas.ExplorationDto |
    ProfilePropertyKey

export type ProfilePropertyKey =
    keyof Components.Schemas.CessationWellsCostOverrideDto |
    keyof Components.Schemas.CessationOffshoreFacilitiesCostOverrideDto |
    keyof Components.Schemas.TotalFeasibilityAndConceptStudiesOverrideDto |
    keyof Components.Schemas.TotalFEEDStudiesOverrideDto |
    keyof Components.Schemas.HistoricCostCostProfileDto |
    keyof Components.Schemas.WellInterventionCostProfileOverrideDto |
    keyof Components.Schemas.OffshoreFacilitiesOperationsCostProfileOverrideDto |
    keyof Components.Schemas.OnshoreRelatedOPEXCostProfileDto |
    keyof Components.Schemas.AdditionalOPEXCostProfileDto |
    keyof Components.Schemas.TopsideCostProfileOverrideDto |
    keyof Components.Schemas.SurfCostProfileOverrideDto |
    keyof Components.Schemas.TransportCostProfileOverrideDto |
    keyof Components.Schemas.SubstructureCostProfileOverrideDto |
    keyof Components.Schemas.ProductionProfileOilDto |
    keyof Components.Schemas.ProductionProfileGasDto |
    keyof Components.Schemas.ProductionProfileWaterDto |
    keyof Components.Schemas.ProductionProfileWaterInjectionDto |
    keyof Components.Schemas.NetSalesGasOverrideDto |
    keyof Components.Schemas.Co2EmissionsOverrideDto |
    keyof Components.Schemas.ImportedElectricityOverrideDto |
    keyof Components.Schemas.DeferredOilProductionDto |
    keyof Components.Schemas.DeferredGasProductionDto |
    keyof Components.Schemas.OilProducerCostProfileOverrideDto |
    keyof Components.Schemas.GasProducerCostProfileOverrideDto |
    keyof Components.Schemas.WaterInjectorCostProfileOverrideDto |
    keyof Components.Schemas.GasInjectorCostProfileOverrideDto |
    keyof Components.Schemas.SeismicAcquisitionAndProcessingDto |
    keyof Components.Schemas.CountryOfficeCostDto;

export interface EditInstance {
    uuid: string; // unique identifier for the edit
    timeStamp: number; // the time the edit was made
    newValue: string | number | undefined; // the value after the edit
    previousValue: string | number | undefined; // the value before the edit
    inputLabel: string; // the label of the input field being edited
    projectId: string; // the project id
    resourceName: ResourceName; // the asset being edited
    resourcePropertyKey: ResourcePropertyKey; // the key of the asset being edited
    resourceId?: string; // the id of the asset being edited
    resourceProfileId?: string; // the id of the timeseries profile being edited
    caseId?: string; // the case id
    newDisplayValue?: string | number | undefined; // the displayed new value in case of when the value submitted is not what the user should see
    previousDisplayValue?: string | number | undefined; // the displayed previous value in case of when the value submitted is not what the user should see
    newResourceObject?: ResourceObject; // this is used to replace the whole asset object. used if the edit should change multiple values in the same assets
}

export interface EditEntry {
    caseId: string;
    currentEditId: string;
}

export interface ITimeSeriesData {
    profileName: string
    unit: string,
    profile: ITimeSeries | undefined
    overrideProfile?: ITimeSeries | undefined
    resourceId: string
    resourcePropertyKey: string
    resourceName: ProfileNames
    resourceProfileId?: string
    overridable: boolean
    editable: boolean
}
