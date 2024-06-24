import { visibility, lock, type IconData } from "@equinor/eds-icons"

export const EMPTY_GUID = "00000000-0000-0000-0000-000000000000"

export const TABLE_VALIDATION_RULES: { [key: string]: { min: number, max: number } } = {
    // production profiles
    "Oil production": { min: 0, max: 1000000 },
    "Gas production": { min: 0, max: 1000000 },
    "Water production": { min: 0, max: 1000000 },
    "Water injection": { min: 0, max: 1000000 },
    "Fuel, flaring and losses": { min: 0, max: 1000000 },
    "Net sales gas": { min: 0, max: 1000000 },
    "Imported electricity": { min: 0, max: 1000000 },

    // CO2 emissions
    "Annual CO2 emissions": { min: 0, max: 1000000 },
    "Year-by-year CO2 intensity": { min: 0, max: 1000000 },
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
