import isEqual from "lodash/isEqual"
import { Dispatch, SetStateAction } from "react"

import { ITimeSeries, ITimeSeriesTableData, ITimeSeriesTableDataWithSet } from "@/Models/ITimeSeries"
import { EditInstance } from "@/Models/Interfaces"
import { TABLE_VALIDATION_RULES } from "@/Utils/Config/constants"
import { parseDecimalInput, roundToDecimals, sumAndRound } from "@/Utils/FormatingUtils"

/**
 * Checks if a well is an exploration well based on its category
 * @param well - Well to check
 * @returns True if well belongs to categories 4, 5, or 6
 */
export const isExplorationWell = (well: Components.Schemas.WellOverviewDto | undefined) => [4, 5, 6].indexOf(well?.wellCategory ?? -1) > -1

/**
 * Merges multiple time series data arrays with their respective offsets
 * @param dataArrays - Arrays of numerical values to merge
 * @param offsets - Offset positions for each array
 * @returns Combined array with summed values at corresponding positions
 */

/**
 * Merges two time series objects
 * @param t1 - First time series
 * @param t2 - Second time series
 * @returns Combined time series with values summed at corresponding years
 */
export const mergeTimeseries = (t1: ITimeSeries | undefined, t2: ITimeSeries | undefined): ITimeSeries => {
    if (!t1) { return t2 || { startYear: 0, values: [] } }
    if (!t2) { return t1 }

    const startYears = [t1, t2].map((t: ITimeSeries | undefined) => t?.startYear ?? 0)
    const minYear = Math.min(...startYears)

    const arrays = [t1, t2].map((t: ITimeSeries | undefined) => t?.values ?? [])
    const offsets = startYears.map((year: number) => Math.abs(year - minYear))

    const mergeTimeSeriesValues = (): number[] => {
        if (arrays.length !== offsets.length) {
            throw new Error("dataArrays and offsets must have the same length")
        }

        const maxLength = Math.max(...arrays.map((dataArray, index) => dataArray.length + offsets[index]))
        const result = new Array(maxLength).fill(0)

        arrays.forEach((dataArray: number[], index: number) => {
            const offset = offsets[index]

            dataArray.forEach((value: number, i: number) => {
                const adjustedIndex = i + offset

                if (adjustedIndex < maxLength) {
                    result[adjustedIndex] += value
                }
            })
        })

        return result
    }

    return {
        startYear: minYear,
        values: mergeTimeSeriesValues(),
    }
}

/**
 * Merges multiple time series objects into a single series
 * @param timeSeriesList - Array of time series objects to merge
 * @returns Single combined time series
 */
export const mergeTimeseriesList = (timeSeriesList: (ITimeSeries | undefined)[]): ITimeSeries => {
    let mergedTimeSeries: ITimeSeries = { startYear: 0, values: [] }

    timeSeriesList.forEach((currentSeries, index) => {
        if (index === 0) {
            mergedTimeSeries = currentSeries ?? mergedTimeSeries
        } else {
            mergedTimeSeries = mergeTimeseries(mergedTimeSeries, currentSeries)
        }
    })

    return mergedTimeSeries
}

/**
 * Generates a time series profile from table values
 * @param tableTimeSeriesValues - Array of year/value pairs
 * @param profile - Base profile to extend
 * @param startYear - Start year of the profile
 * @param firstYear - First year to include
 * @param lastYear - Last year to include
 * @returns Complete profile with values array
 */
export const generateProfile = (
    tableTimeSeriesValues: { year: number, value: number }[],
    profile: any,
    startYear: number,
    firstYear: number,
    lastYear: number,
) => {
    const values: number[] = []

    if (tableTimeSeriesValues.length === 0) {
        return {
            ...profile,
            startYear,
            values: [],
        }
    }

    for (let year = firstYear; year <= lastYear; year += 1) {
        const tableTimeSeriesValue = tableTimeSeriesValues.find((v) => v.year === year)

        values.push(tableTimeSeriesValue ? tableTimeSeriesValue.value : 0)
    }

    return {
        ...profile,
        startYear,
        values,
    }
}

/**
 * Gets the last year from a time series
 * @param timeSeries - Time series to analyze
 * @returns Last year in the series or undefined if invalid
 */
export const GetTimeSeriesLastYear = (
    timeSeries:
        | {
            id?: string;
            startYear?: number;
            name?: string;
            values?: number[] | null;
            sum?: number | undefined;
        }
        | undefined,
): number | undefined => {
    if (
        timeSeries
        && timeSeries.startYear !== undefined
        && timeSeries.values
        && timeSeries.values.length > 0
    ) {
        return timeSeries.startYear + timeSeries.values.length - 1
    }

    return undefined
}

export interface ITableCellChangeParams {
    data: any
    newValue: any
    oldValue: any
    profileName: string
    profile?: any
    resourceId?: string
    wellId?: string
}

export interface ITableCellChangeConfig {
    dg4Year: number
    caseId?: string
    projectId?: string
    tab?: string
    tableName?: string
    timeSeriesData: any[]
    setSnackBarMessage?: (message: string) => void
}

/**
 * Determines if a table cell is editable
 * @param params - Cell parameters including node and data
 * @param editAllowed - Whether editing is allowed globally
 * @param isSaving - Whether a save operation is in progress
 * @returns True if the cell can be edited
 */
export const tableCellisEditable = (params: any, editAllowed: boolean, isSaving?: boolean): boolean => {
    if (!params || !params.node || !params.data || params.node.footer || isSaving) {
        return false
    }

    if (params.data.overridable) {
        return editAllowed && params.data.override
    }

    return editAllowed && params.data.editable
}

/**
 * Validates input against defined rules for table cells
 * @param params - Cell parameters including value and data
 * @param editAllowed - Whether editing is allowed
 * @param isSaving - Whether a save operation is in progress
 * @returns Error message or null if valid
 */
export const validateInput = (params: any, editAllowed: boolean, isSaving?: boolean) => {
    const { value, data } = params

    // Empty strings are valid input
    if (value === "") {
        return null
    }

    if (tableCellisEditable(params, editAllowed, isSaving) && value) {
        const rule = TABLE_VALIDATION_RULES[data.profileName]

        if (rule && (value < rule.min || value > rule.max)) {
            return `Value must be between ${rule.min} and ${rule.max}.`
        }
    }

    return null
}

/**
 * Parses and validates number input for table cells
 * @param setSnackBarMessage - Function to display error messages
 * @param params - Object with new and old values
 * @returns Parsed and validated number value or empty string
 */
export const numberValueParser = (
    setSnackBarMessage: Dispatch<SetStateAction<string | undefined>>,
    params: { newValue: any, oldValue: any, data: any },
) => {
    const { oldValue, newValue } = params

    // Allow empty string to clear cell value
    if (newValue === "") {
        return ""
    }

    // Check if the newValue has more than one decimal point
    const dotCount = (newValue.toString().match(/\./g) || []).length
                   + (newValue.toString().match(/,/g) || []).length

    if (dotCount > 1) {
        setSnackBarMessage("Only one decimal point is allowed. The entry was reset.")

        return oldValue
    }

    // First check if the input only contains valid characters
    const validInput = /^-?[0-9.,]+$/.test(newValue.toString())

    if (!validInput && newValue.toString() !== "") {
        setSnackBarMessage("Only numbers, commas, dots and minus signs are allowed. Invalid characters have been removed.")
        // Filter out invalid characters and try again
        const filtered = newValue.toString().replace(/[^0-9.,-]/g, "")

        return parseDecimalInput(filtered)
    }

    return parseDecimalInput(newValue)
}

/**
 * Returns style for case rows in tables
 * @param params - Row parameters
 * @returns Style object or undefined
 */
export const getCaseRowStyle = (params: any) => {
    if (params.node.footer) {
        return { fontWeight: "bold" }
    }

    return undefined
}

/**
 * Style object for right-aligned cells
 */
export const cellStyleRightAlign = { textAlign: "right" }

/**
 * Sets a non-negative number in state object
 * @param value - Number value to set
 * @param objectKey - Key in state object
 * @param state - Current state
 * @param setState - State setter function
 */
export const setNonNegativeNumberState = (value: number, objectKey: string, state: any, setState: Dispatch<SetStateAction<any>>): void => {
    const newState = { ...state }

    newState[objectKey] = Math.max(value, 0)
    setState(newState)
}

/**
 * Formats sum of column values with validation
 * @param params - Object containing values array
 * @param decimalPrecision - Number of decimal places to round to
 * @returns Formatted sum as a number, or empty string for zero sums or empty arrays
 */
export function formatColumnSum(params: any, decimalPrecision: number = 2): string | number {
    // Early returns for invalid inputs
    if (!params || !params.values) {
        return ""
    }

    // Process each value in the array
    const validNumbers = params.values
        // Convert strings to numbers if needed
        .map((value: any) => {
            if (typeof value === "string") {
                return parseDecimalInput(value)
            }

            return value
        })
        // Filter out invalid values (NaN, null, undefined, Infinity, empty strings)
        .filter((value: any) => (
            typeof value === "number"
            && !Number.isNaN(value)
            && Number.isFinite(value)
        ))

    // If we have valid numbers, calculate and return the sum
    if (validNumbers.length > 0) {
        const sum = sumAndRound(validNumbers, decimalPrecision)

        // Return empty string for zero sums
        if (sum === 0) {
            return ""
        }

        return sum
    }

    // Return empty string for empty arrays
    return ""
}

/**
 * Extracts year/value pairs from table row data
 * @param tableData - Row data object
 * @returns Array of year/value pairs sorted by year
 */
export const getValuesFromEntireRow = (tableData: any) => {
    const valuePerYear: { year: number, value: number }[] = []
    const isInteger = (value: string): boolean => /^-?\d+$/.test(value)

    Object.keys(tableData).forEach((columnName) => {
        const cellValue = tableData[columnName]
        const parsedValue = parseDecimalInput(cellValue?.toString() ?? "")

        if (
            isInteger(columnName)
            && cellValue !== ""
            && cellValue !== null
            && typeof parsedValue === "number"
            && !Number.isNaN(parsedValue)
        ) {
            valuePerYear.push({
                year: parseInt(columnName, 10),
                value: parsedValue,
            })
        }
    })

    return valuePerYear.sort((a, b) => a.year - b.year)
}

/**
 * Validates a table cell change against defined rules
 * @param params - Cell change parameters
 * @param config - Configuration including rules and notifications
 * @returns True if change is valid
 */
export const validateTableCellChange = (params: ITableCellChangeParams, config: ITableCellChangeConfig) => {
    const { newValue, profileName } = params
    const { setSnackBarMessage } = config

    // Allow empty strings
    if (newValue === "") {
        return true
    }

    const rule = TABLE_VALIDATION_RULES[profileName]

    if (rule && setSnackBarMessage && (newValue < rule.min || newValue > rule.max)) {
        setSnackBarMessage(`Value must be between ${rule.min} and ${rule.max}. Please correct the input to save the input.`)

        return false
    }

    return true
}

/**
 * Generates edit instance for table cell change
 * @param params - Cell change parameters
 * @param config - Configuration including case and project info
 * @returns Edit instance or null if no changes
 */
export const generateTableCellEdit = (params: ITableCellChangeParams, config: ITableCellChangeConfig): EditInstance | null => {
    const { data, profileName } = params
    const {
        dg4Year, caseId, projectId, timeSeriesData,
    } = config

    if (!caseId || !projectId) { return null }

    const rowValues = getValuesFromEntireRow(data)
    const existingProfile = data.profile ? structuredClone(data.profile) : {
        startYear: 0,
        values: [],
        id: data.resourceId,
    }

    let newProfile

    if (rowValues.length > 0) {
        const firstYear = rowValues[0].year
        const lastYear = rowValues[rowValues.length - 1].year
        const startYear = firstYear - dg4Year

        newProfile = generateProfile(rowValues, data.profile, startYear, firstYear, lastYear)
    } else {
        newProfile = structuredClone(existingProfile)
        newProfile.values = []
    }

    if (!newProfile || isEqual(newProfile.values, existingProfile.values)) {
        return null
    }

    const profileInTimeSeriesData = timeSeriesData.find(
        (v) => v.profileName === profileName,
    )

    if (data.override !== undefined) {
        newProfile.overrideProfile = {
            override: !!data.override,
        }
    } else if (data.overrideProfile) {
        newProfile.overrideProfile = data.overrideProfile
    }

    const editInstance: EditInstance = {
        projectId,
        caseId,
        resourceName: profileInTimeSeriesData?.resourceName,
        resourcePropertyKey: profileInTimeSeriesData?.resourcePropertyKey,
        resourceId: profileInTimeSeriesData?.resourceId,
        wellId: params.data.wellId,
        resourceObject: newProfile,
    }

    return editInstance
}

/**
 * Sets table year range based on profiles
 * @param profiles - Array of time series profiles
 * @param dg4Year - Base year for DG4
 * @param setStartYear - Start year setter
 * @param setEndYear - End year setter
 * @param setTableYears - Table years range setter
 */
export const SetTableYearsFromProfiles = (
    profiles: (
        | {
            id?: string;
            startYear?: number;
            name?: string;
            values?: number[] | null;
            sum?: number | undefined;
        }
        | undefined
    )[],
    dg4Year: number,
    setStartYear: Dispatch<SetStateAction<number>>,
    setEndYear: Dispatch<SetStateAction<number>>,
    setTableYears: Dispatch<SetStateAction<[number, number]>>,
) => {
    let firstYear: number | undefined
    let lastYear: number | undefined

    profiles.forEach((profile) => {
        if (profile?.startYear !== undefined) {
            const { startYear } = profile
            const profileStartYear: number = startYear + dg4Year

            if (firstYear === undefined) {
                firstYear = profileStartYear
            } else if (profileStartYear < firstYear) {
                firstYear = profileStartYear
            }
        }

        const profileLastYear = GetTimeSeriesLastYear(profile)

        if (profileLastYear !== undefined) {
            const adjustedProfileLastYear = profileLastYear + dg4Year

            if (lastYear === undefined) {
                lastYear = adjustedProfileLastYear
            } else if (adjustedProfileLastYear > lastYear) {
                lastYear = adjustedProfileLastYear
            }
        }
    })

    if (firstYear !== undefined && lastYear !== undefined) {
        const totalYears = lastYear - firstYear + 1
        const desiredYears = 11

        if (totalYears < desiredYears) {
            const additionalYears = desiredYears - totalYears
            const additionalYearsBefore = Math.floor(additionalYears / 2)
            const additionalYearsAfter = additionalYears - additionalYearsBefore

            firstYear -= additionalYearsBefore
            lastYear += additionalYearsAfter
        }

        setStartYear(firstYear)
        setEndYear(lastYear)
        setTableYears([firstYear, lastYear])
    }
}

/**
 * Sets summary table year range based on profiles
 * @param profiles - Array of time series
 * @param dg4Year - Base year for DG4
 * @param setTableYears - Table years range setter
 */
export const SetSummaryTableYearsFromProfiles = (
    profiles: (ITimeSeries | undefined)[],
    dg4Year: number,
    setTableYears: Dispatch<SetStateAction<[number, number]>>,
) => {
    let firstYear: number | undefined
    let lastYear: number | undefined

    profiles.forEach((profile) => {
        if (profile?.startYear !== undefined) {
            const { startYear } = profile
            const profileStartYear: number = startYear + dg4Year

            if (firstYear === undefined) {
                firstYear = profileStartYear
            } else if (profileStartYear < firstYear) {
                firstYear = profileStartYear
            }
        }

        const profileLastYear = GetTimeSeriesLastYear(profile)

        if (profileLastYear !== undefined) {
            const adjustedProfileLastYear = profileLastYear + dg4Year

            if (lastYear === undefined) {
                lastYear = adjustedProfileLastYear
            } else if (adjustedProfileLastYear > lastYear) {
                lastYear = adjustedProfileLastYear
            }
        }
    })

    if (firstYear !== undefined && lastYear !== undefined) {
        setTableYears([firstYear, lastYear])
    }
}

/**
 * Creates bar profile objects for charts
 * @param barProfiles - Profile identifiers
 * @param barNames - Display names for profiles
 * @param xKey - X-axis key
 * @returns Array of configured bar profile objects
 */
export const separateProfileObjects = (barProfiles: string[], barNames: string[], xKey: string) => {
    const barProfileObjects = barProfiles.map((bp, i) => ({
        type: "bar",
        xKey,
        yKey: bp,
        yName: barNames[i],
        grouped: true,
        highlightStyle: {
            item: {
                fill: undefined,
                stroke: undefined,
                strokeWidth: 1,
            },
            series: {
                enabled: true,
                dimOpacity: 0.2,
                strokeWidth: 2,
            },
        },
    }))

    return barProfileObjects
}

/**
 * Conditionally inserts elements based on condition
 * @param condition - Whether to insert elements
 * @param addAxes - Whether to add axes data
 * @param axesData - Axes configuration data
 * @param elements - Elements to conditionally insert
 * @returns Inserted elements or empty array
 */
export const insertIf = (condition: boolean, addAxes: boolean, axesData: any, ...elements: any) => {
    if (addAxes) {
        return condition ? { axes: axesData } : []
    }

    return condition ? elements : []
}

/**
 * Converts grid reference array to aligned grid
 * @param alignedGridsRef - Array of grid references
 * @returns Aligned grid array or undefined
 */
export const gridRefArrayToAlignedGrid = (alignedGridsRef: any[]) => {
    if (alignedGridsRef && alignedGridsRef.length > 0) {
        const refArray: any[] = []

        alignedGridsRef.forEach((agr: any) => {
            if (agr && agr.current) {
                refArray.push(agr.current)
            }
        })
        if (refArray.length > 0) {
            return refArray
        }
    }

    return undefined
}

interface IAssetWell {
    assetId: string
    wellId: string
    drillingSchedule: {
        id?: string
        startYear: number
        values: number[]
    }
}

/**
 * Creates missing asset wells from available wells
 * @param existingAssetWells - Currently defined asset wells
 * @param availableWells - All available wells
 * @param resourceId - Current resource identifier
 * @param dg4Year - Base year for DG4
 * @param isExplorationTable - Whether table is for exploration wells
 * @returns Complete array with existing and missing wells
 */
export const createMissingAssetWellsFromWells = (
    existingAssetWells: any[],
    availableWells: Components.Schemas.WellOverviewDto[] | undefined,
    resourceId: string,
    dg4Year: number,
    isExplorationTable: boolean,
): IAssetWell[] => {
    // Convert existing wells to the expected format
    const formattedAssetWells: IAssetWell[] = existingAssetWells.map((well) => ({
        assetId: well.explorationId || well.wellProjectId,
        wellId: well.wellId,
        drillingSchedule: well.drillingSchedule,
    }))

    const relevantWells = availableWells?.filter((well) => isExplorationTable === isExplorationWell(well)) ?? []

    const missingWells = relevantWells
        .filter((well) => !formattedAssetWells.some((existing) => existing.wellId === well.id))
        .map((well) => ({
            assetId: resourceId,
            wellId: well.id,
            drillingSchedule: {
                startYear: dg4Year,
                values: [],
            },
        }))

    return [...formattedAssetWells, ...missingWells]
}

/**
 * Calculates year key from base year and index
 * @param baseYear - Base year (DG4)
 * @param startYear - Start year of series
 * @param index - Index in values array
 * @returns Year as string key
 */
const calculateYearKey = (baseYear: number, startYear: number, index: number): string => (baseYear + startYear + index).toString()

/**
 * Sums array of numerical values
 * @param values - Array of numbers to sum
 * @returns Total sum
 */
const sumValues = (values: number[]): number => values.reduce((acc, val) => acc + val, 0)

/**
 * Creates yearly values object from drilling schedule
 * @param drillingSchedule - Drilling schedule with values
 * @param dg4Year - Base year for DG4
 * @returns Object with year keys and values plus total
 */
const createYearlyValues = (
    drillingSchedule: IAssetWell["drillingSchedule"],
    dg4Year: number,
): { [key: string]: number, total: number } => {
    const result: { [key: string]: number, total: number } = { total: 0 }

    if (drillingSchedule.values?.length > 0 && drillingSchedule.startYear !== undefined) {
        drillingSchedule.values.forEach((value, index) => {
            const yearKey = calculateYearKey(dg4Year, drillingSchedule.startYear, index)

            result[yearKey] = value
        })
        result.total = sumValues(drillingSchedule.values)
    }

    return result
}

/**
 * Converts wells to table row data
 * @param assetWells - Wells associated with asset
 * @param wells - All available wells
 * @param dg4Year - Base year for DG4
 * @param editAllowed - Whether editing is allowed
 * @param resourceId - Current resource identifier
 * @param isExplorationTable - Whether table is for exploration wells
 * @returns Array of formatted row data objects
 */
export const wellsToRowData = (
    assetWells: Components.Schemas.CampaignWellDto[],
    wells: Components.Schemas.WellOverviewDto[] | undefined,
    dg4Year: number,
    editAllowed: boolean,
    resourceId: string,
    isExplorationTable: boolean,
) => {
    const allAssetWells = createMissingAssetWellsFromWells(assetWells, wells, resourceId, dg4Year, isExplorationTable)

    const tableWells = allAssetWells.map((assetWell) => {
        const wellName = wells?.find((well) => well.id === assetWell.wellId)?.name ?? ""
        const { drillingSchedule } = assetWell

        const yearlyValues = createYearlyValues(drillingSchedule, dg4Year)

        const tableWell = {
            name: wellName,
            ...yearlyValues,
            assetWell,
            assetWells: allAssetWells,
            drillingSchedule,
        }

        if (editAllowed || tableWell.total > 0) {
            return tableWell
        }

        return undefined
    })

    return tableWells.filter((tableWell) => tableWell !== undefined)
}

/**
 * Converts time series data to table row data
 * @param timeSeriesData - Array of time series data
 * @param dg4Year - Base year for DG4
 * @param tableName - Name of table for precision settings
 * @param editAllowed - Whether editing is allowed
 * @param decimalPrecision - Number of decimal places to round to (default: 2)
 * @returns Array of formatted row data
 */
export const profilesToRowData = (
    timeSeriesData: ITimeSeriesTableData[],
    dg4Year: number,
    editAllowed: boolean,
    decimalPrecision: number = 2,
) => {
    const tableRows: ITimeSeriesTableData[] = []

    timeSeriesData?.forEach((ts: ITimeSeriesTableDataWithSet) => {
        const isOverridden = ts.overrideProfile?.override === true
        const profile = isOverridden ? ts.overrideProfile : ts.profile
        const profileSet = isOverridden ? ts.overrideProfileSet : ts.set

        const rowObject: any = {
            profileName: ts.profileName,
            unit: ts.unit,
            total: ts.total ?? 0,
            set: profileSet,
            profile,
            override: isOverridden,
            resourceId: ts.resourceId,
            resourceName: ts.resourceName,
            overridable: ts.overridable,
            editable: ts.editable,
            hideIfEmpty: ts.hideIfEmpty,
            overrideProfileSet: ts.overrideProfileSet,
            overrideProfile: ts.overrideProfile ?? { startYear: 0, values: [], override: false },
            wellId: ts.wellId,
        }

        if (profile && profile.values && profile.values.length > 0) {
            let j = 0

            for (let i = profile.startYear; i < profile.startYear + profile.values.length; i += 1) {
                const year = dg4Year + i
                const value = profile.values[j]

                // In edit mode, keep full precision, otherwise round the values
                rowObject[year.toString()] = editAllowed
                    ? value // Keep full precision in edit mode
                    : roundToDecimals(value, decimalPrecision) // Round in view mode
                j += 1
            }

            // Calculate total with full precision as well
            const total = profile.values.reduce((acc, value) => acc + value, 0)

            rowObject.total = total

            if (ts.total !== undefined) {
                rowObject.total = Number(ts.total)
            }
        }

        const isNotHidden = !rowObject.hideIfEmpty
        const hasProfileValues = rowObject.hideIfEmpty && rowObject.profile?.values.length > 0

        if (editAllowed || isNotHidden || hasProfileValues) {
            tableRows.push(rowObject)
        }
    })

    return tableRows
}

/**
 * Gets existing profile from table data or creates empty one
 * @param tableData - Current table data
 * @param resourceId - Resource identifier
 * @returns Existing profile or new empty profile
 */
export const getExistingProfile = (tableData: any, resourceId: string) => (tableData.profile ? structuredClone(tableData.profile) : {
    startYear: 0,
    values: [],
    id: resourceId,
})

/**
 * Generates new profile from row values
 * @param rowValues - Array of year/value pairs
 * @param existingProfile - Existing profile to extend
 * @param dg4Year - Base year for DG4
 * @param tableData - Current table data
 * @returns New or updated profile
 */
export const generateNewProfile = (rowValues: any[], existingProfile: any, dg4Year: number, tableData: any) => {
    if (rowValues.length === 0) {
        const emptyProfile = structuredClone(existingProfile)

        emptyProfile.values = []

        return emptyProfile
    }

    const firstYear = rowValues[0].year
    const lastYear = rowValues[rowValues.length - 1].year
    const startYear = firstYear - dg4Year

    return generateProfile(rowValues, tableData.profile, startYear, firstYear, lastYear)
}

/**
 * Finds profile data by name in time series data array
 * @param timeSeriesData - Array of time series data
 * @param profileName - Name of profile to find
 * @returns Matching profile data or undefined
 */
export const getProfileDataFromTimeSeriesData = (timeSeriesData: ITimeSeriesTableData[], profileName: string) => timeSeriesData.find((v) => v.profileName === profileName)
