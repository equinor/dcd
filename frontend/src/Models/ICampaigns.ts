export interface CampaignProps {
    campaign: Components.Schemas.CampaignDto
    title: string
    tableYears: [number, number]
}

export interface TableRow {
    name: string
    subheader: string
    [key: number]: number | string
    total?: number
}

export interface Profile {
    startYear: number
    values: number[]
}

export interface Well {
    wellName: string
    startYear: number
    values: number[]
    wellCategory: number
    wellId: string
}

export interface Campaign {
    rigMobDemobProfile: Profile
    rigUpgradingProfile: Profile
    campaignWells: Well[]
}
