import { Currency, PhysUnit } from "@/Models/enums"

export function truncateText(text: string, maxLength: number): string {
    return (text.length + 3) > maxLength ? `${text.slice(0, maxLength)}...` : text
}

/**
 * Converts a string that may use commas as decimal separators to a number
 * This is useful for parsing user input from regions that use comma as the decimal separator
 *
 * @param value The string value to convert
 * @returns The converted number, or 0 if the input is not valid
 */
export function parseDecimalInput(value: string | number | null): number | "" {
    if (value === null || value === undefined) {
        return 0
    }

    const stringValue = typeof value === "number" ? value.toString() : value

    // Return empty string if the input is empty
    if (stringValue === "") {
        return ""
    }

    return Number(stringValue.replace(/,/g, "."))
}

/**
 * Formats currency units based on the project currency setting
 * @param currency The currency enum value from project data
 * @returns Formatted currency unit string (MNOK or MUSD)
 */
export function formatCurrencyUnit(currency: Currency | undefined): string {
    return currency === Currency.Nok ? "MNOK" : "MUSD"
}

/**
 * Formats oil volume units based on the physical unit setting
 * @param physUnit The physical unit enum value from project data
 * @returns Formatted oil volume unit string
 */
export function formatOilVolumeUnit(physUnit: PhysUnit | undefined): string {
    return physUnit === PhysUnit.Si ? "MSm³/yr" : "mill bbls/yr"
}

/**
 * Formats gas volume units based on the physical unit setting
 * @param physUnit The physical unit enum value from project data
 * @returns Formatted gas volume unit string
 */
export function formatGasVolumeUnit(physUnit: PhysUnit | undefined): string {
    return physUnit === PhysUnit.Si ? "GSm³/yr" : "Bscf/yr"
}

/**
 * Formats water volume units based on the physical unit setting
 * @param physUnit The physical unit enum value from project data
 * @returns Formatted water volume unit string
 */
export function formatWaterVolumeUnit(physUnit: PhysUnit | undefined): string {
    return physUnit === PhysUnit.Si ? "MSm³/yr" : "mill bbls/yr"
}

/**
 * Formats a numeric value with two decimal places and thousands separators
 * @param value The numeric value to format
 * @returns Formatted string with two decimal places
 */
export function formatNumberWithDecimals(value: number): string {
    return value.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",")
}

/**
 * Rounds a number to a specified number of decimal places
 * @param value The number to round
 * @param decimals The number of decimal places to round to (default: 2)
 * @returns The rounded number
 */
export function roundToDecimals(value: number, decimals: number = 2): number {
    // Check if value is NaN
    if (Number.isNaN(value)) {
        return NaN
    }

    const factor = 10 ** decimals

    return Math.round((value + Number.EPSILON) * factor) / factor
}

/**
 * Calculates the sum of an array of numbers and rounds the result
 * @param values Array of numbers to sum
 * @param decimals The number of decimal places to round to (default: 2)
 * @returns The rounded sum
 */
export function sumAndRound(values: number[], decimals: number = 2): number {
    const sum = values.reduce((acc, value) => acc + value, 0)

    return roundToDecimals(sum, decimals)
}

/**
 * Transforms camelCase or PascalCase profile names into human-readable format
 * Removes "Profiles" suffix and adds spaces before capital letters
 *
 * @param profileName The original profile name in camelCase or PascalCase
 * @returns Formatted human-readable profile name
 */
export function formatProfileName(profileName: string): string {
    return profileName
        .replace(/Profiles$/, "") // Remove Profiles suffix
        .replace(/([A-Z])/g, " $1") // Add space before capital letters
        .trim() // Remove any leading/trailing whitespace
}

/**
 * Gets the appropriate unit for a profile based on the profile name
 * This is a convenient way to get the right unit in one call when you know the profile name
 *
 * @param profileName The display name of the profile
 * @param physUnit The physical unit setting from project data
 * @param currency The currency setting from project data
 * @returns The appropriate unit string for the profile
 */
export function getUnitByProfileName(
    profileName: string,
    physUnit: PhysUnit | undefined,
    currency: Currency | undefined,
): string {
    // Profiles that use oil volume units
    const oilProfiles = [
        "Oil production",
        "Additional Oil production",
        "Condensate production",
        "Water production",
        "Water injection",
        "Deferred oil production",
    ]

    // Profiles that use gas volume units
    const gasProfiles = [
        "Rich gas production",
        "Additional rich gas production",
        "Fuel, flaring and losses",
        "Net sales gas",
        "Deferred gas production",
    ]

    // Profiles with fixed units
    const fixedUnitProfiles: Record<string, string> = {
        "NGL Production": "MTPA",
        "Imported electricity": "GWh",
        "Production & sales volumes": "MBoE/yr",
    }

    if (oilProfiles.includes(profileName)) {
        return formatOilVolumeUnit(physUnit)
    }

    if (gasProfiles.includes(profileName)) {
        return formatGasVolumeUnit(physUnit)
    }

    if (fixedUnitProfiles[profileName]) {
        return fixedUnitProfiles[profileName]
    }

    // Default to currency for cost profiles
    return formatCurrencyUnit(currency)
}

/**
 * Formats a number for display in charts, adding thousands separators
 * Abbreviates numbers with trailing zeros (100000 -> 100K, 2000000 -> 2M)
 * But keeps precise numbers with separators (100123 -> 100,123)
 * Handles small decimal values for CO2 intensity metrics
 * @param value The number to format
 * @returns Formatted string with appropriate precision
 */
export function formatChartNumber(value: number | string): string {
    if (value === null || value === undefined || value === "") {
        return "0"
    }

    const numericValue = typeof value === "number" ? value : parseFloat(value)

    if (Number.isNaN(numericValue)) {
        return "0"
    }

    if (numericValue === 0) {
        return "0"
    }

    // For very small decimal values (like fractional CO2 intensity values)
    if (Math.abs(numericValue) < 1) {
        // Use fixed 4 decimal places for very small values
        return numericValue.toFixed(4)
    }

    // For small whole numbers (1-9)
    if (Math.abs(numericValue) < 10 && Number.isInteger(numericValue)) {
        return numericValue.toString()
    }

    // For small decimal numbers between 1 and 10
    if (Math.abs(numericValue) < 10) {
        return numericValue.toFixed(2)
    }

    // Check if number has only trailing zeros in thousands
    const absValue = Math.abs(numericValue)

    // For thousands with trailing zeros (1000, 2000, 10000, etc.)
    if (absValue >= 1000 && absValue < 1000000 && absValue % 1000 === 0) {
        return `${(numericValue / 1000)}K`
    }

    // For millions with trailing zeros (1000000, 2000000, etc.)
    if (absValue >= 1000000 && absValue < 1000000000 && absValue % 1000000 === 0) {
        return `${(numericValue / 1000000)}M`
    }

    // For billions with trailing zeros (1000000000, 2000000000, etc.)
    if (absValue >= 1000000000 && absValue % 1000000000 === 0) {
        return `${(numericValue / 1000000000)}B`
    }

    // For numbers that are not round, use localeString with commas
    return numericValue.toLocaleString("en-US", {
        maximumFractionDigits: 1,
        minimumFractionDigits: 0,
    })
}
