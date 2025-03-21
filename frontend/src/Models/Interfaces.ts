import { ProfileTypes } from "@/Models/enums"

export type ResourceName =
    "rigUpgradingCost" |
    "rigMobDemobCost" |
    `${ProfileTypes}`

export type ProfileObject =
    Components.Schemas.SaveTimeSeriesDto |
    Components.Schemas.SaveTimeSeriesOverrideDto

export type ResourcePropertyKey =
    keyof Components.Schemas.CampaignDto |
    keyof Components.Schemas.TimeSeriesDto |
    keyof Components.Schemas.TimeSeriesOverrideDto

export interface EditInstance {
    projectId: string;
    resourceName: ResourceName;
    resourcePropertyKey: ResourcePropertyKey;
    resourceId?: string;
    wellId?: string;
    caseId?: string;
    resourceObject: ProfileObject;
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

export interface TableWell {
    id: string,
    name: string,
    wellCategory: Components.Schemas.WellCategory,
    drillingDays: number,
    wellCost: number,
    well: Components.Schemas.WellOverviewDto
    wells: Components.Schemas.WellOverviewDto[]
}
