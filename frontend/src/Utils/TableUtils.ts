import isEqual from "lodash/isEqual"
import { Dispatch, SetStateAction } from "react"

import { ITimeSeries, ITimeSeriesTableData, ITimeSeriesTableDataWithSet } from "@/Models/ITimeSeries"
import { EditInstance } from "@/Models/Interfaces"
import { TABLE_VALIDATION_RULES } from "@/Utils/Config/constants"

export const isExplorationWell = (well: Components.Schemas.WellOverviewDto | undefined) => [4, 5, 6].indexOf(well?.wellCategory ?? -1) > -1

// Core time series operations
const mergeTimeSeriesValues = (dataArrays: number[][], offsets: number[]): number[] => {
    if (dataArrays.length !== offsets.length) {
        throw new Error("dataArrays and offsets must have the same length")
    }

    const maxLength = Math.max(...dataArrays.map((dataArray, index) => dataArray.length + offsets[index]))
    const result = new Array(maxLength).fill(0)

    dataArrays.forEach((dataArray: number[], index: number) => {
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

export const mergeTimeseries = (t1: ITimeSeries | undefined, t2: ITimeSeries | undefined): ITimeSeries => {
    if (!t1) { return t2 || { startYear: 0, values: [] } }
    if (!t2) { return t1 }

    const startYears = [t1, t2].map((t: ITimeSeries | undefined) => t?.startYear ?? 0)
    const minYear = Math.min(...startYears)

    const arrays = [t1, t2].map((t: ITimeSeries | undefined) => t?.values ?? [])
    const offsets = startYears.map((year: number) => Math.abs(year - minYear))

    const mergedValues = mergeTimeSeriesValues(arrays, offsets)

    return {
        startYear: minYear,
        values: mergedValues,
    }
}

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

// Time series data manipulation
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

// Table interfaces
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

// Table cell operations
export const tableCellisEditable = (params: any, editAllowed: boolean, isSaving?: boolean): boolean => {
    if (!params || !params.node || !params.data || params.node.footer || isSaving) {
        return false
    }

    if (params.data.overridable) {
        return editAllowed && params.data.override
    }

    return editAllowed && params.data.editable
}

export const validateInput = (params: any, editAllowed: boolean, isSaving?: boolean) => {
    const { value, data } = params

    if (tableCellisEditable(params, editAllowed, isSaving) && value) {
        const rule = TABLE_VALIDATION_RULES[data.profileName]

        if (rule && (value < rule.min || value > rule.max)) {
            return `Value must be between ${rule.min} and ${rule.max}.`
        }
    }

    return null
}

export const numberValueParser = (
    setSnackBarMessage: Dispatch<SetStateAction<string | undefined>>,
    params: { newValue: any, oldValue: any, data: any },
) => {
    const { oldValue, newValue } = params
    const valueWithOnlyValidChars = newValue.toString().replace(/[^0-9.,-]/g, "")
    const allCommasTurnedToDots = valueWithOnlyValidChars.replace(/,/g, ".")

    if ((allCommasTurnedToDots.match(/\./g) || []).length > 1) {
        setSnackBarMessage("Only one decimal point is allowed. The entry was reset.")

        return oldValue
    }

    if (valueWithOnlyValidChars.toString() !== newValue.toString()) {
        setSnackBarMessage("Only numbers, commas, dots and minus signs are allowed. Invalid characters have been removed.")
    }

    return allCommasTurnedToDots
}

// Table data formatting
export const getCaseRowStyle = (params: any) => {
    if (params.node.footer) {
        return { fontWeight: "bold" }
    }

    return undefined
}

export const cellStyleRightAlign = { textAlign: "right" }

export const setNonNegativeNumberState = (value: number, objectKey: string, state: any, setState: Dispatch<SetStateAction<any>>): void => {
    const newState = { ...state }

    newState[objectKey] = Math.max(value, 0)
    setState(newState)
}

export const formatColumnSum = (params: { values: any[] }) => {
    let sum = 0

    params.values.forEach((value: any) => {
        if (!Number.isNaN(parseFloat(value)) && Number.isFinite(value)) {
            sum += Number(value)
        }
    })

    return sum > 0 ? parseFloat(sum.toFixed(10)) : ""
}

export const getValuesFromEntireRow = (tableData: any) => {
    const valuePerYear: { year: number, value: number }[] = []
    const isInteger = (value: string): boolean => /^-?\d+$/.test(value)

    Object.keys(tableData).forEach((columnName) => {
        const cellValue = tableData[columnName]

        if (
            isInteger(columnName)
            && cellValue !== ""
            && cellValue !== null
            && !Number.isNaN(Number(cellValue.toString().replace(/,/g, ".")))
        ) {
            valuePerYear.push({
                year: parseInt(columnName, 10),
                value: Number(cellValue.toString().replace(/,/g, ".")),
            })
        }
    })

    return valuePerYear.sort((a, b) => a.year - b.year)
}

// Table validation and edit generation
export const validateTableCellChange = (params: ITableCellChangeParams, config: ITableCellChangeConfig) => {
    const { newValue, profileName } = params
    const { setSnackBarMessage } = config

    const rule = TABLE_VALIDATION_RULES[profileName]

    if (rule && setSnackBarMessage && (newValue < rule.min || newValue > rule.max)) {
        setSnackBarMessage(`Value must be between ${rule.min} and ${rule.max}. Please correct the input to save the input.`)

        return false
    }

    return true
}

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

// Table year management
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

    if (firstYear !== undefined) {
        setStartYear(firstYear)
    }
    if (lastYear !== undefined) {
        setEndYear(lastYear)
    }

    const totalYears = (lastYear !== undefined && firstYear !== undefined) ? lastYear - firstYear + 1 : 0
    const desiredYears = 11

    if (totalYears < desiredYears) {
        const additionalYears = desiredYears - totalYears
        const additionalYearsBefore = Math.floor(additionalYears / 2)
        const additionalYearsAfter = additionalYears - additionalYearsBefore

        if (firstYear !== undefined && lastYear !== undefined) {
            firstYear -= additionalYearsBefore
            lastYear += additionalYearsAfter
            setStartYear(firstYear)
            setEndYear(lastYear)
            setTableYears([firstYear, lastYear])
        }
    }
    if (firstYear !== undefined && lastYear !== undefined) {
        setTableYears([firstYear, lastYear])
    }
}

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

// Table UI components
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

export const insertIf = (condition: boolean, addAxes: boolean, axesData: any, ...elements: any) => {
    if (addAxes) {
        return condition ? { axes: axesData } : []
    }

    return condition ? elements : []
}

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

const calculateYearKey = (baseYear: number, startYear: number, index: number): string => (baseYear + startYear + index).toString()

const sumValues = (values: number[]): number => values.reduce((acc, val) => acc + val, 0)

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

const roundValues = (values: number[], precision: number): number[] => values.map((v) => Math.round((v + Number.EPSILON) * precision) / precision)

const calculateTotal = (values: number[], precision: number): number => Math.round(values.reduce((x, y) => x + y) * precision) / precision

export const profilesToRowData = (
    timeSeriesData: ITimeSeriesTableData[],
    dg4Year: number,
    tableName: string,
    editAllowed: boolean,
) => {
    const tableRows: ITimeSeriesTableData[] = []

    timeSeriesData.forEach((ts: ITimeSeriesTableDataWithSet) => {
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
            const precision = (tableName === "Production profiles" || tableName === "CO2 emissions") ? 10000 : 10
            const roundedValues = roundValues(profile.values, precision)
            let j = 0

            for (let i = profile.startYear; i < profile.startYear + profile.values.length; i += 1) {
                const yearKey = calculateYearKey(dg4Year, i, 0)

                rowObject[yearKey] = roundedValues[j]
                j += 1
            }

            rowObject.total = calculateTotal(profile.values, precision)
            if (ts.total !== undefined) {
                rowObject.total = Math.round(Number(ts.total) * 1000) / 1000
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

export const getExistingProfile = (tableData: any, resourceId: string) => (tableData.profile ? structuredClone(tableData.profile) : {
    startYear: 0,
    values: [],
    id: resourceId,
})

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

export const getProfileDataFromTimeSeriesData = (timeSeriesData: ITimeSeriesTableData[], profileName: string) => timeSeriesData.find((v) => v.profileName === profileName)
