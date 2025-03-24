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
