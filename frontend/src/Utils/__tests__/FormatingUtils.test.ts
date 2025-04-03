import {
    describe, it, expect, vi,
} from "vitest"

import {
    formatCurrencyUnit,
    formatGasVolumeUnit,
    formatNumberWithDecimals,
    formatOilVolumeUnit,
    formatProfileName,
    formatWaterVolumeUnit,
    getUnitByProfileName,
    parseDecimalInput,
    roundToDecimals,
    sumAndRound,
    truncateText,
    formatNumberForView,
} from "../FormatingUtils"

// Mock enums to avoid @/Models/enums dependency
const Currency = {
    Usd: 0,
    Nok: 1,
}

const PhysUnit = {
    OilField: 0,
    Si: 1,
}

// Mock the imports
vi.mock("@/Models/enums", () => ({
    Currency: {
        Usd: 0,
        Nok: 1,
    },
    PhysUnit: {
        OilField: 0,
        Si: 1,
    },
}))

describe("truncateText", () => {
    it("should truncate text when length exceeds maxLength", () => {
        const text = "This is a long text"
        const maxLength = 10
        const result = truncateText(text, maxLength)

        expect(result).toBe("This is a ...")
    })

    it("should not truncate text when length is less than maxLength", () => {
        const text = "Short"
        const maxLength = 10
        const result = truncateText(text, maxLength)

        expect(result).toBe("Short")
    })
})

describe("parseDecimalInput", () => {
    it("should convert comma decimal separator to dot", () => {
        expect(parseDecimalInput("123,45")).toBe(123.45)
    })

    it("should handle dot decimal separator", () => {
        expect(parseDecimalInput("123.45")).toBe(123.45)
    })

    it("should handle null input", () => {
        expect(parseDecimalInput(null)).toBe(0)
    })

    it("should handle number input", () => {
        expect(parseDecimalInput(123.45)).toBe(123.45)
    })
})

describe("formatCurrencyUnit", () => {
    it("should return MNOK for NOK currency", () => {
        expect(formatCurrencyUnit(Currency.Nok)).toBe("MNOK")
    })

    it("should return MUSD for USD currency", () => {
        expect(formatCurrencyUnit(Currency.Usd)).toBe("MUSD")
    })
})

describe("formatOilVolumeUnit", () => {
    it("should return MSm³/yr for SI units", () => {
        expect(formatOilVolumeUnit(PhysUnit.Si)).toBe("MSm³/yr")
    })

    it("should return mill bbls/yr for oilfield units", () => {
        expect(formatOilVolumeUnit(PhysUnit.OilField)).toBe("mill bbls/yr")
    })
})

describe("formatGasVolumeUnit", () => {
    it("should return GSm³/yr for SI units", () => {
        expect(formatGasVolumeUnit(PhysUnit.Si)).toBe("GSm³/yr")
    })

    it("should return Bscf/yr for oilfield units", () => {
        expect(formatGasVolumeUnit(PhysUnit.OilField)).toBe("Bscf/yr")
    })
})

describe("formatWaterVolumeUnit", () => {
    it("should return MSm³/yr for SI units", () => {
        expect(formatWaterVolumeUnit(PhysUnit.Si)).toBe("MSm³/yr")
    })

    it("should return mill bbls/yr for oilfield units", () => {
        expect(formatWaterVolumeUnit(PhysUnit.OilField)).toBe("mill bbls/yr")
    })
})

describe("formatNumberWithDecimals", () => {
    it("should format number with 2 decimal places", () => {
        expect(formatNumberWithDecimals(1234.5678)).toBe("1,234.57")
    })

    it("should add thousands separator", () => {
        expect(formatNumberWithDecimals(1234567.89)).toBe("1,234,567.89")
    })
})

describe("roundToDecimals", () => {
    it("should round to 2 decimal places by default", () => {
        expect(roundToDecimals(1.2345)).toBe(1.23)
        expect(roundToDecimals(1.235)).toBe(1.24)
    })

    it("should round to specified decimal places", () => {
        expect(roundToDecimals(1.2345, 3)).toBe(1.235)
        expect(roundToDecimals(1.2345, 1)).toBe(1.2)
    })
})

describe("sumAndRound", () => {
    it("should sum array of numbers and round to 2 decimals by default", () => {
        expect(sumAndRound([1.111, 2.222, 3.333])).toBe(6.67)
    })

    it("should sum and round to specified decimal places", () => {
        expect(sumAndRound([1.111, 2.222, 3.333], 3)).toBe(6.666)
    })

    it("should handle empty array", () => {
        expect(sumAndRound([])).toBe(0)
    })
})

describe("formatProfileName", () => {
    it("should add spaces before capital letters", () => {
        expect(formatProfileName("OilProduction")).toBe("Oil Production")
    })

    it("should remove Profiles suffix", () => {
        expect(formatProfileName("OilProductionProfiles")).toBe("Oil Production")
    })
})

describe("getUnitByProfileName", () => {
    it("should return oil volume unit for oil profiles", () => {
        expect(getUnitByProfileName("Oil production", PhysUnit.Si, Currency.Usd)).toBe("MSm³/yr")
        expect(getUnitByProfileName("Water injection", PhysUnit.OilField, Currency.Usd)).toBe("mill bbls/yr")
    })

    it("should return gas volume unit for gas profiles", () => {
        expect(getUnitByProfileName("Rich gas production", PhysUnit.Si, Currency.Usd)).toBe("GSm³/yr")
        expect(getUnitByProfileName("Net sales gas", PhysUnit.OilField, Currency.Usd)).toBe("Bscf/yr")
    })

    it("should return fixed unit for specific profiles", () => {
        expect(getUnitByProfileName("NGL Production", PhysUnit.Si, Currency.Usd)).toBe("MTPA")
        expect(getUnitByProfileName("Imported electricity", PhysUnit.Si, Currency.Usd)).toBe("GWh")
    })

    it("should default to currency unit for other profiles", () => {
        expect(getUnitByProfileName("Rig upgrade", PhysUnit.Si, Currency.Nok)).toBe("MNOK")
        expect(getUnitByProfileName("OPEX", PhysUnit.Si, Currency.Usd)).toBe("MUSD")
    })
})

describe("formatChartNumber", () => {
    // Edge cases and invalid inputs
    it("should handle null, undefined and empty values", () => {
        expect(formatNumberForView(null as any)).toBe("0")
        expect(formatNumberForView(undefined as any)).toBe("0")
        expect(formatNumberForView("")).toBe("0")
    })

    it("should handle NaN and zero", () => {
        expect(formatNumberForView(NaN)).toBe("0")
        expect(formatNumberForView(0)).toBe("0")
        expect(formatNumberForView("0")).toBe("0")
    })

    // Numbers less than 1
    it("should preserve full precision for very small numbers", () => {
        expect(formatNumberForView(0.12345)).toBe("0,12345") // Preserved with Norwegian format
        expect(formatNumberForView(0.0001)).toBe("0,0001") // Preserved with Norwegian format
        expect(formatNumberForView(0.00001)).toBe("0,00001") // Preserved with Norwegian format
        expect(formatNumberForView(0.9999)).toBe("0,9999") // Preserved with Norwegian format
    })

    // Single digit integers
    it("should format small integers (1-9) without changes", () => {
        expect(formatNumberForView(1)).toBe("1")
        expect(formatNumberForView(5)).toBe("5")
        expect(formatNumberForView(9)).toBe("9")
    })

    // Regular numbers without 3 trailing zeros (not abbreviated)
    it("should format regular numbers using Norwegian locale", () => {
        expect(formatNumberForView(123)).toBe("123")
        expect(formatNumberForView(1234)).toEqual(expect.stringMatching(/1.234/)) // Match pattern to avoid encoding issues
        expect(formatNumberForView(12345)).toEqual(expect.stringMatching(/12.345/))
        expect(formatNumberForView(123456)).toEqual(expect.stringMatching(/123.456/))
        expect(formatNumberForView(1234567)).toEqual(expect.stringMatching(/1.234.567/))
        expect(formatNumberForView(12345678)).toEqual(expect.stringMatching(/12.345.678/))
        expect(formatNumberForView(123456789)).toEqual(expect.stringMatching(/123.456.789/))
    })

    // Decimal numbers
    it("should format decimal numbers using Norwegian locale", () => {
        expect(formatNumberForView(12.34)).toEqual(expect.stringMatching(/12,34/)) // Match pattern to avoid encoding issues
        expect(formatNumberForView(1234.56)).toEqual(expect.stringMatching(/1.234,56/))
        expect(formatNumberForView(1234567.89)).toEqual(expect.stringMatching(/1.234.567,89/))
    })

    // Numbers with 3 trailing zeros - should be abbreviated
    it("should abbreviate thousands with 3+ trailing zeros", () => {
        expect(formatNumberForView(1000)).toBe("1K")
        expect(formatNumberForView(5000)).toBe("5K")
        expect(formatNumberForView(10000)).toBe("10K")
        expect(formatNumberForView(100000)).toBe("100K")
        expect(formatNumberForView(999000)).toBe("999K")
    })

    it("should abbreviate millions with 3+ trailing zeros", () => {
        expect(formatNumberForView(1000000)).toBe("1M")
        expect(formatNumberForView(5000000)).toBe("5M")
        expect(formatNumberForView(10000000)).toBe("10M")
        expect(formatNumberForView(100000000)).toBe("100M")
        expect(formatNumberForView(999000000)).toBe("999M")
    })

    it("should abbreviate billions with 3+ trailing zeros", () => {
        expect(formatNumberForView(1000000000)).toBe("1B")
        expect(formatNumberForView(5000000000)).toBe("5B")
        expect(formatNumberForView(10000000000)).toBe("10B")
        expect(formatNumberForView(100000000000)).toBe("100B")
    })

    // Edge cases for abbreviation
    it("should not abbreviate numbers without exactly 3+ trailing zeros", () => {
        expect(formatNumberForView(1001)).toEqual(expect.stringMatching(/1.001/)) // Match pattern to avoid encoding issues
        expect(formatNumberForView(1000001)).toEqual(expect.stringMatching(/1.000.001/))
        expect(formatNumberForView(1000000001)).toEqual(expect.stringMatching(/1.000.000.001/))
    })

    // Negative numbers
    it("should handle negative numbers correctly", () => {
        expect(formatNumberForView(-5)).toBe("-5") // Single digits don't use toLocaleString
        expect(formatNumberForView(-1234)).toEqual(expect.stringMatching(/[−-]1.?234/)) // Match any minus sign and thousands separator
        expect(formatNumberForView(-1000)).toBe("-1K") // K/M/B abbreviations use hyphen
        expect(formatNumberForView(-1000000)).toBe("-1M")
        expect(formatNumberForView(-1000000000)).toBe("-1B")
    })

    // String inputs
    it("should handle string inputs correctly", () => {
        expect(formatNumberForView("123")).toBe("123")
        expect(formatNumberForView("1000")).toBe("1K")
        expect(formatNumberForView("1000000")).toBe("1M")
        expect(formatNumberForView("0.1234")).toBe("0,1234")
    })

    // Specific values from the requirements
    it("should match specific examples from the requirements", () => {
        // Format small whole numbers (1-9) without decimal places
        expect(formatNumberForView(5)).toBe("5")

        // Display very small values (<1) with full precision
        expect(formatNumberForView(0.1234)).toEqual(expect.stringMatching(/0,1234/))

        // Abbreviate numbers with trailing zeros
        expect(formatNumberForView(1000)).toBe("1K")
        expect(formatNumberForView(10000)).toBe("10K")
        expect(formatNumberForView(1000000)).toBe("1M")
        expect(formatNumberForView(1000000000)).toBe("1B")

        // Preserve full notation with separators for non-round numbers
        expect(formatNumberForView(1234)).toEqual(expect.stringMatching(/1.234/)) // Norwegian locale uses spaces or dots
        expect(formatNumberForView(10123)).toEqual(expect.stringMatching(/10.123/))
        expect(formatNumberForView(1000123)).toEqual(expect.stringMatching(/1.000.123/))

        // The example for 15000000 should be correctly abbreviated
        expect(formatNumberForView(15000000)).toBe("15M")
    })

    // More detailed edge cases
    it("should handle complex number patterns correctly", () => {
        // Numbers with non-zero values after initial zeros
        expect(formatNumberForView(130001)).toEqual(expect.stringMatching(/130.001/)) // No abbreviation - not divisible by 1000

        // Numbers with zeros but not exactly divisible by 1000
        expect(formatNumberForView(54000230)).toEqual(expect.stringMatching(/54.000.230/)) // No abbreviation

        // Numbers exactly divisible by 1000 but without trailing zeros
        expect(formatNumberForView(10001000)).toBe("10.001M") // Abbreviated because divisible by 1000

        // Numbers at the edge of abbreviation thresholds
        expect(formatNumberForView(120000012)).toEqual(expect.stringMatching(/120.000.012/)) // No abbreviation

        // Verify numbers with exact 1000 multiples are abbreviated
        expect(formatNumberForView(130000)).toBe("130K") // Should be abbreviated (exact multiple of 1000)
        expect(formatNumberForView(54000000)).toBe("54M") // Should be abbreviated (exact multiple of 1000000)

        // Numbers that look like they should be abbreviated but aren't
        expect(formatNumberForView(1234000)).toBe("1.234M") // Divisible by 1000, so abbreviated
        expect(formatNumberForView(999000000)).toBe("999M") // Divisible by 1000, so abbreviated

        // Numbers divisible by 1000 with non-trailing zeros
        expect(formatNumberForView(1001000)).toBe("1.001M") // Divisible by 1000, so abbreviated
        expect(formatNumberForView(3400500000)).toBe("3.4005B") // Divisible by 1000, in billions range

        // Numbers at transition thresholds
        expect(formatNumberForView(999999000)).toBe("999.999M") // Abbreviated as M because it's in millions range
        expect(formatNumberForView(1000000000)).toBe("1B") // Abbreviated as B because it's 1×1000000000

        // Numbers with decimal parts and exact thousands
        expect(formatNumberForView(5000.123)).toEqual(expect.stringMatching(/5.000,123/)) // Not abbreviated due to decimal part
        expect(formatNumberForView(1000000.5)).toEqual(expect.stringMatching(/1.000.000,5/)) // Not abbreviated due to decimal part
    })

    // Edge cases with various divisibility patterns
    it("should handle divisibility edge cases correctly", () => {
        // Divisible by 1000 but not 1000000
        expect(formatNumberForView(123000)).toBe("123K") // Abbreviated as K
        expect(formatNumberForView(123000000)).toBe("123M") // Abbreviated as M

        // Numbers with complex internal zeros
        expect(formatNumberForView(10100100)).toEqual(expect.stringMatching(/10.100.100/)) // Not divisible by 1000
        expect(formatNumberForView(10100000)).toBe("10.1M") // Divisible by 1000, so abbreviated

        // Near-threshold numbers
        expect(formatNumberForView(999999)).toEqual(expect.stringMatching(/999.999/)) // Not abbreviated
        expect(formatNumberForView(999999000)).toBe("999.999M") // Abbreviated as M because it's in millions range

        // Very large numbers
        expect(formatNumberForView(1234567890000)).toBe("1234.56789B") // Abbreviated as B with proper formatting
        expect(formatNumberForView(1230000000000)).toBe("1230B") // Abbreviated more concisely

        // Numbers with multiple internal zero patterns
        expect(formatNumberForView(10010010000)).toBe("10.01001B") // Abbreviated because divisible by 1000 and in billions range
    })
})
