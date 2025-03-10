import { ProfileTypes } from "@/Models/enums"

export type ResourceName =
    "case" |
    "topside" |
    "surf" |
    "transport" |
    "onshorePowerSupply" |
    "substructure" |
    "drainageStrategy" |
    "fuelFlaringAndLossesOverride" |
    "wellProject" |
    "exploration" |
    "explorationWellDrillingSchedule" |
    "developmentWellDrillingSchedule" |
    "rigUpgradingCost" |
    "rigMobDemobCost" |
    `${ProfileTypes}`

export type ResourceObject =
    Components.Schemas.TopsideDto |
    Components.Schemas.SurfDto |
    Components.Schemas.SubstructureDto |
    Components.Schemas.TransportDto |
    Components.Schemas.CaseOverviewDto |
    Components.Schemas.CaseWithAssetsDto |
    Components.Schemas.DrainageStrategyDto |
    Components.Schemas.OnshorePowerSupplyDto |
    Components.Schemas.UpdateCampaignDto |
    Components.Schemas.CampaignDto |
    Components.Schemas.UpdateRigMobDemobCostDto |
    Components.Schemas.UpdateRigUpgradingCostDto |
    ProfileObject

export type ProfileObject =
    Components.Schemas.TimeSeriesOverrideDto |
    Components.Schemas.TimeSeriesDto |
    Components.Schemas.SaveTimeSeriesDto |
    Components.Schemas.SaveTimeSeriesOverrideDto

export type ResourcePropertyKey =
    keyof Components.Schemas.TopsideDto |
    keyof Components.Schemas.SurfDto |
    keyof Components.Schemas.SubstructureDto |
    keyof Components.Schemas.TransportDto |
    keyof Components.Schemas.OnshorePowerSupplyDto |
    keyof Components.Schemas.CaseOverviewDto |
    keyof Components.Schemas.DrainageStrategyDto |
    keyof Components.Schemas.UpdateCampaignDto |
    keyof Components.Schemas.CampaignDto |
    keyof Components.Schemas.CampaignWellDto |
    keyof Components.Schemas.TimeSeriesOverrideDto |
    keyof Components.Schemas.TimeSeriesDto

export interface EditInstance {
    uuid: string;
    projectId: string;
    resourceName: ResourceName;
    resourcePropertyKey: ResourcePropertyKey;
    resourceId?: string;
    wellId?: string;
    caseId?: string;
    resourceObject: ResourceObject;
    campaignId?: string;
}

export interface TableCase {
    id: string
    name: string
    description: string
    productionStrategyOverview: Components.Schemas.ProductionStrategyOverview | undefined
    producerCount: number
    waterInjectorCount: number
    gasInjectorCount: number
    createdAt?: string
    referenceCaseId?: string
}

export type Version = `${number}.${number}.${number}`;
export type Category = "New Functionalities" | "UI Improvements" | "Bugfixes" | "Other";
export type UpdateEntry = {
    description: string;
};

export type VersionUpdates = {
    date: string;
    updates: {
        [key in Category]?: UpdateEntry[];
    };
};

export type WhatsNewUpdates = {
    [key in Version]: VersionUpdates;
};
