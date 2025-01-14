/* eslint-disable import/no-extraneous-dependencies */
/* eslint-disable no-restricted-syntax */
import { describe, it, expect } from "vitest"
import {
    dateFromTimestamp,
    dateStringToDateUtc,
    getYearFromDateString,
    sortUtcDateStrings,
} from "../DateUtils"

describe("dateStringToDateUtc", () => {
    it("should convert \"0001-01-01\" to \"0001-01-01T00:00:00Z\"", () => {
        const dateString = "0001-01-01"
        const expectedDate = new Date("0001-01-01T00:00:00Z")
        const result = dateStringToDateUtc(dateString)
        expect(result.toISOString()).toBe(expectedDate.toISOString())
    })

    it("should convert \"0001-01-01T00:00\" to \"0001-01-01T00:00:00Z\"", () => {
        const dateString = "0001-01-01T00:00"
        const expectedDate = new Date("0001-01-01T00:00:00Z")
        const result = dateStringToDateUtc(dateString)
        expect(result.toISOString()).toBe(expectedDate.toISOString())
    })

    it("should convert \"0001-01-01T00:00:00\" to \"0001-01-01T00:00:00Z\"", () => {
        const dateString = "0001-01-01T00:00:00"
        const expectedDate = new Date("0001-01-01T00:00:00Z")
        const result = dateStringToDateUtc(dateString)
        expect(result.toISOString()).toBe(expectedDate.toISOString())
    })

    it("should convert \"0001-01-01T00:00:00Z\" to \"0001-01-01T00:00:00Z\"", () => {
        const dateString = "0001-01-01T00:00:00Z"
        const expectedDate = new Date("0001-01-01T00:00:00Z")
        const result = dateStringToDateUtc(dateString)
        expect(result.toISOString()).toBe(expectedDate.toISOString())
    })

    it("should convert \"0001-01-01T00:00:00+00:00\" to \"0001-01-01T00:00:00Z\"", () => {
        const dateString = "0001-01-01T00:00:00+00:00"
        const expectedDate = new Date("0001-01-01T00:00:00Z")
        const result = dateStringToDateUtc(dateString)
        expect(result.toISOString()).toBe(expectedDate.toISOString())
    })

    it("should convert \"0001-01-01T00:00:00.0000000\" to \"0001-01-01T00:00:00Z\"", () => {
        const dateString = "0001-01-01T00:00:00.0000000"
        const expectedDate = new Date("0001-01-01T00:00:00Z")
        const result = dateStringToDateUtc(dateString)
        expect(result.toISOString()).toBe(expectedDate.toISOString())
    })
})

describe("getYearFromDateString", () => {
    it("should return the year from a date string", () => {
        const dateString = "2025-01-01T00:00:00Z"
        const expectedYear = 2025
        const result = getYearFromDateString(dateString)
        expect(result).toBe(expectedYear)
    })

    it("should return the year from a date string without time", () => {
        const dateString = "2025-01-01"
        const expectedYear = 2025
        const result = getYearFromDateString(dateString)
        expect(result).toBe(expectedYear)
    })
})

describe("dateFromTimestamp", () => {
    it("should return a Date object from a timestamp", () => {
        const timestamp = 1735689600000 // Equivalent to 2025-01-01T00:00:00Z
        const expectedDate = new Date("2025-01-01T00:00:00Z")
        const result = dateFromTimestamp(timestamp)
        expect(result.toISOString()).toBe(expectedDate.toISOString())
    })
})

describe("sortUtcDateStrings", () => {
    it("should sort two UTC date strings correctly", () => {
        const dateString1 = "2025-01-01T00:00:00Z"
        const dateString2 = "2023-01-01T00:00:00Z"
        const result = sortUtcDateStrings(dateString1, dateString2)
        expect(result).toBeGreaterThan(0) // dateString1 is after dateString2

        const result2 = sortUtcDateStrings(dateString2, dateString1)
        expect(result2).toBeLessThan(0) // dateString2 is before dateString1
    })

    it("should return 0 for two identical UTC date strings", () => {
        const dateString = "2025-01-01T00:00:00Z"
        const result = sortUtcDateStrings(dateString, dateString)
        expect(result).toBe(0)
    })
})
