import {
    describe, it, expect, vi, beforeEach,
} from "vitest"

import {
    formatColumnSum,
    getValuesFromEntireRow,
    numberValueParser,
} from "../TableUtils"

describe("numberValueParser", () => {
    const mockSetSnackBarMessage = vi.fn()

    beforeEach(() => {
        mockSetSnackBarMessage.mockReset()
    })

    it("should convert string with dot decimal to number", () => {
        const params = {
            newValue: "123.45",
            oldValue: "",
            data: {},
        }

        const result = numberValueParser(mockSetSnackBarMessage, params)

        expect(result).toBe(123.45)
        expect(mockSetSnackBarMessage).not.toHaveBeenCalled()
    })

    it("should convert string with comma decimal to number", () => {
        const params = {
            newValue: "123,45",
            oldValue: "",
            data: {},
        }

        const result = numberValueParser(mockSetSnackBarMessage, params)

        expect(result).toBe(123.45)
        expect(mockSetSnackBarMessage).not.toHaveBeenCalled()
    })

    it("should handle multiple decimal points and return oldValue", () => {
        const params = {
            newValue: "123.45.67",
            oldValue: "100",
            data: {},
        }

        const result = numberValueParser(mockSetSnackBarMessage, params)

        expect(result).toBe("100")
        expect(mockSetSnackBarMessage).toHaveBeenCalledWith("Only one decimal point is allowed. The entry was reset.")
    })

    it("should handle invalid characters and filter them out", () => {
        mockSetSnackBarMessage.mockClear()
        const params = {
            newValue: "123abc45",
            oldValue: "",
            data: {},
        }

        const result = numberValueParser(mockSetSnackBarMessage, params)

        expect(result).toBe(12345)
        expect(mockSetSnackBarMessage).toHaveBeenCalledWith("Only numbers, commas, dots and minus signs are allowed. Invalid characters have been removed.")
    })

    it("should handle empty string input", () => {
        const params = {
            newValue: "",
            oldValue: "",
            data: {},
        }

        const result = numberValueParser(mockSetSnackBarMessage, params)

        expect(result).toBe("")
        expect(mockSetSnackBarMessage).not.toHaveBeenCalled()
    })

    it("should handle negative numbers", () => {
        const params = {
            newValue: "-123.45",
            oldValue: "",
            data: {},
        }

        const result = numberValueParser(mockSetSnackBarMessage, params)

        expect(result).toBe(-123.45)
        expect(mockSetSnackBarMessage).not.toHaveBeenCalled()
    })
})

describe("formatColumnSum", () => {
    it("should sum valid numbers and round to 2 decimal places", () => {
        const params = {
            values: [1.111, 2.222, 3.333],
        }
        const expected = 6.67
        const result = formatColumnSum(params, 2)

        expect(result).toBe(expected)
    })

    it("should respect the specified precision parameter", () => {
        const params = {
            values: [1.111, 2.222, 3.333],
        }

        // Test with precision 4
        expect(formatColumnSum(params, 4)).toBe(6.666)
        // Test with precision 1
        expect(formatColumnSum(params, 1)).toBe(6.7)
        // Test with precision 0
        expect(formatColumnSum(params, 0)).toBe(7)
    })

    it("should filter out NaN and non-finite values", () => {
        const params = {
            values: [1.5, NaN, 2.5, Infinity],
        }
        const expected = 4
        const result = formatColumnSum(params, 2)

        expect(result).toBe(expected)
    })

    it("should return empty string for zero sum", () => {
        const params = {
            values: [0, 0, 0],
        }
        const expected = ""
        const result = formatColumnSum(params, 2)

        expect(result).toBe(expected)
    })

    it("should handle negative numbers and return the negative sum", () => {
        const params = {
            values: [-5, 2],
        }
        const expected = -3
        const result = formatColumnSum(params, 2)

        expect(result).toBe(expected)
    })

    it("should return empty string for empty array", () => {
        const params = {
            values: [],
        }
        const expected = ""
        const result = formatColumnSum(params, 2)

        expect(result).toBe(expected)
    })

    it("should handle string values by converting them to numbers", () => {
        const params = {
            values: ["1.5", "2.5", "3.5"],
        }
        const expected = 7.5
        const result = formatColumnSum(params, 2)

        expect(result).toBe(expected)
    })

    it("should handle comma-separated string values", () => {
        const params = {
            values: ["1,5", "2,5", "3,5"],
        }
        const expected = 7.5
        const result = formatColumnSum(params, 2)

        expect(result).toBe(expected)
    })
})

describe("getValuesFromEntireRow", () => {
    it("should convert table data to year-value pairs", () => {
        const tableData = {
            name: "Test Profile",
            2020: "10.5",
            2021: "20.5",
            2022: "30.5",
            total: "61.5",
        }
        const expected = [
            { year: 2020, value: 10.5 },
            { year: 2021, value: 20.5 },
            { year: 2022, value: 30.5 },
        ]
        const result = getValuesFromEntireRow(tableData)

        expect(result).toEqual(expected)
    })

    it("should sort by year", () => {
        const tableData = {
            2022: "30.5",
            2020: "10.5",
            2021: "20.5",
            total: "61.5",
        }
        const expected = [
            { year: 2020, value: 10.5 },
            { year: 2021, value: 20.5 },
            { year: 2022, value: 30.5 },
        ]
        const result = getValuesFromEntireRow(tableData)

        expect(result).toEqual(expected)
    })

    it("should filter out non-year properties", () => {
        const tableData = {
            name: "Test Profile",
            2020: "10.5",
            unit: "units",
            2021: "20.5",
            profile: {},
            total: "61.5",
        }
        const expected = [
            { year: 2020, value: 10.5 },
            { year: 2021, value: 20.5 },
        ]
        const result = getValuesFromEntireRow(tableData)

        expect(result).toEqual(expected)
    })

    it("should filter out empty string and null values", () => {
        const tableData = {
            2020: "10.5",
            2021: "",
            2022: null,
            2023: "30.5",
        }
        const expected = [
            { year: 2020, value: 10.5 },
            { year: 2023, value: 30.5 },
        ]
        const result = getValuesFromEntireRow(tableData)

        expect(result).toEqual(expected)
    })

    it("should handle comma-separated decimal values", () => {
        const tableData = {
            2020: "10,5",
            2021: "20,5",
        }
        const expected = [
            { year: 2020, value: 10.5 },
            { year: 2021, value: 20.5 },
        ]
        const result = getValuesFromEntireRow(tableData)

        expect(result).toEqual(expected)
    })
})
