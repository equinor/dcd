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
export function parseDecimalInput(value: string | number | null): number {
    if (value === null || value === undefined) { return 0 }

    const stringValue = typeof value === "number" ? value.toString() : value

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
    return Math.round((value + Number.EPSILON) * 10 ** decimals) / 10 ** decimals
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
        "Total exported volumes": "MBoE/yr",
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
