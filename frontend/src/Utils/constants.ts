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
