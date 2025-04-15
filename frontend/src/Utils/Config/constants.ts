import { visibility, lock, type IconData } from "@equinor/eds-icons"

import { ProfileTypes, WellCategory } from "@/Models/enums"

export const TABLE_VALIDATION_RULES: { [key: string]: { min: number, max: number } } = {
    // production profiles
    "Oil production": { min: 0, max: 1_000_000 },
    "Rich gas production": { min: 0, max: 1_000_000 },
    "Water production": { min: 0, max: 1_000_000 },
    "Water injection": { min: 0, max: 1_000_000 },
    "Fuel, flaring and losses": { min: 0, max: 1_000_000 },
    "Net sales gas": { min: 0, max: 1_000_000 },
    "Imported electricity": { min: 0, max: 1_000_000 },

    // CO2 emissions
    "Annual CO2 emissions": { min: 0, max: 1_000_000 },
    "Year-by-year CO2 intensity": { min: 0, max: 1_000_000 },
}

// Gantt chart configuration
export const GANTT_CHART_CONFIG = {
    MAX_VISIBLE_RANGE_YEARS: 20,
    MIN_ALLOWED_YEAR: 2000,
    MAX_ALLOWED_YEAR: 2100,
}

interface ProjectClassification {
    label: string,
    description: string,
    icon: IconData,
    color: "default" | "active" | "error" | undefined,
    warn: boolean
}

export const PROJECT_CLASSIFICATION: { [key: number]: ProjectClassification } = {
    0: {
        label: "Open",
        description: "This project is open.\nThere are no consequences if information is made available for unauthorized persons.",
        icon: visibility,
        color: "active",
        warn: false,
    },
    1: {
        label: "Internal",
        description: "This project is internal.\nInformation here is restricted to Equinor personnel. External sharing requires the project leader's approval.",
        icon: visibility,
        color: "active",
        warn: false,
    },
    2: {
        label: "Restricted",
        // eslint-disable-next-line max-len
        description: "This project is restricted.\nInformation here is not allowed to be shared with unauthorized persons and screenshots should be stored safely and handled according to this classification.",
        icon: lock,
        color: "error",
        warn: true,
    },
    3: {
        label: "Confidential",
        // eslint-disable-next-line max-len
        description: "This project is confidential.\nInformation here is not allowed to be shared with unauthorized persons and screenshots should be stored safely and handled according to this classification.",
        icon: lock,
        color: "error",
        warn: true,
    },
}

export const caseTabNames = [
    "Description",
    "Production Profiles",
    "Schedule",
    "Drilling Schedule",
    "Facilities",
    "Cost",
    "CO2 Emissions",
    "Summary",
]

export const projectTabNames = [
    "Overview",
    "Compare cases",
    "Technical Input",
    "Access Management",
    "Project change log",
    "Settings",
]

export const productionOverrideResources = [
    ProfileTypes.ProductionProfileOil,
    ProfileTypes.AdditionalProductionProfileOil,
    ProfileTypes.ProductionProfileGas,
    ProfileTypes.AdditionalProductionProfileGas,
    ProfileTypes.ProductionProfileWater,
    ProfileTypes.ProductionProfileWaterInjection,
]

export const totalStudyCostOverrideResources = [
    ProfileTypes.SurfCostProfileOverride,
    ProfileTypes.TopsideCostProfileOverride,
    ProfileTypes.TransportCostProfileOverride,
    ProfileTypes.SubstructureCostProfileOverride,
    ProfileTypes.OnshorePowerSupplyCostProfileOverride,
    "case",
]

export const INTERNAL_PROJECT_PHASE: { [key: number]: ProjectClassification } = {
    0: {
        label: "APbo",
        description: "Approval Point Business Oppertunity",
        icon: visibility,
        color: "active",
        warn: false,
    },
    1: {
        label: "BOR",
        description: "Business Opportunity Reconfirmation",
        icon: visibility,
        color: "active",
        warn: false,
    },
    2: {
        label: "VPbo",
        description: "Valid Point Business Opportunity",
        icon: lock,
        color: "error",
        warn: true,
    },
}

export const PROJECT_CATEGORY: { [key: number]: string } = {
    0: "Unknown",
    1: "Brownfield",
    2: "Cessation",
    3: "Drilling upgrade",
    4: "Onshore",
    5: "Pipeline",
    6: "Platform FPSO",
    7: "Subsea",
    8: "Solar",
    9: "CO2 storage",
    10: "Efuel",
    11: "Nuclear",
    12: "CO2 Capture",
    13: "FPSO",
    14: "Hydrogen",
    15: "Hse",
    16: "Offshore wind",
    17: "Platform",
    18: "Power from shore",
    19: "Tie-in",
    20: "Renewable other",
    21: "CCS",
}

export const PROJECT_PHASE: { [key: number]: string } = {
    0: "Unknown",
    1: "Bid preparations",
    2: "Business identification",
    3: "Business planning",
    4: "Concept planning",
    5: "Concessions / Negotiations",
    6: "Defintion",
    7: "Execution",
    8: "Operation",
    9: "Screening business opportunities",
}

export const PRODUCTION_STRATEGY: { [key: number]: string } = {
    0: "Depletion",
    1: "Water injection",
    2: "Gas injection",
    3: "WAG",
    4: "Mixed",
}

export const loginAccessTokenKey = "loginAccessToken"
export const FusionAccessTokenKey = "fusionAccessToken"

export const developmentWellOptions = [
    { key: "0", value: WellCategory.OilProducer, label: "Oil producer" },
    { key: "1", value: WellCategory.GasProducer, label: "Gas producer" },
    { key: "2", value: WellCategory.WaterInjector, label: "Water injector" },
    { key: "3", value: WellCategory.GasInjector, label: "Gas injector" },
]

export const explorationWellOptions = [
    { key: "4", value: WellCategory.ExplorationWell, label: "Exploration well" },
    { key: "5", value: WellCategory.AppraisalWell, label: "Appraisal well" },
    { key: "6", value: WellCategory.Sidetrack, label: "Sidetrack" },
]
