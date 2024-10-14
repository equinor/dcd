import { Dispatch, SetStateAction } from "react"

import { ITimeSeries } from "@/Models/ITimeSeries"
import { TABLE_VALIDATION_RULES } from "@/Utils/constants"
import { EditEntry } from "@/Models/Interfaces"

export const loginAccessTokenKey = "loginAccessToken"
export const FusionAccessTokenKey = "fusionAccessToken"

export const projectPath = (projectId: string) => `/${projectId}`

export const casePath = (projectId: string, caseId: string) => `${projectPath(projectId)}/case/${caseId}`

export const storeAppId = (appId: string) => {
    window.sessionStorage.setItem("appId", appId)
}

export const storeAppScope = (appScope: string) => {
    window.sessionStorage.setItem("appScope", appScope)
}

export const getToken = (keyName: string) => {
    const scopes = [[window.sessionStorage.getItem("appScope") || ""][0]]
    return window.Fusion.modules.auth.acquireAccessToken({ scopes })
}

export const unwrapProjectId = (projectId?: string | undefined | null): string => {
    if (projectId === undefined || projectId === null) {
        throw new Error("Attempted to use a Project ID which does not exist")
    }
    return projectId
}

export const getProjectCategoryName = (key?: Components.Schemas.ProjectCategory): string => {
    if (key === undefined) {
        return ""
    }
    return {
        0: "Unknown",
        1: "Brownfield",
        2: "Cessation",
        3: "Drilling upgrade",
        4: "Onshore",
        5: "Pipeline",
        6: "Platform FPSO",
        7: "Subsea",
        8: "Solar",
        9: "CO2 storage",
        10: "Efuel",
        11: "Nuclear",
        12: "CO2 Capture",
        13: "FPSO",
        14: "Hydrogen",
        15: "Hse",
        16: "Offshore wind",
        17: "Platform",
        18: "Power from shore",
        19: "Tie-in",
        20: "Renewable other",
        21: "CCS",
    }[key]
}

export const getProjectPhaseName = (key?: Components.Schemas.ProjectPhase): string => {
    if (key === undefined) {
        return ""
    }
    return {
        0: "Unknown",
        1: "Bid preparations",
        2: "Business identification",
        3: "Business planning",
        4: "Concept planning",
        5: "Concessions / Negotiations",
        6: "Defintion",
        7: "Execution",
        8: "Operation",
        9: "Screening business opportunities",
    }[key]
}

export const getInternalProjectPhaseName = (key?: Components.Schemas.InternalProjectPhase): string => {
    if (key === undefined) {
        return ""
    }
    return {
        0: "Approval Point Business Oppertunity",
        1: "Business Opportunity Reconfirmation",
        2: "Valid Point Business Opportunity",
    }[key]
}

export const toMonthDate = (date?: Date | null): string | undefined => {
    if (Number.isNaN(date?.getTime())) {
        return undefined
    }

    return date?.toISOString().substring(0, 7)
}

export const isDefaultDate = (date?: Date | null): boolean => {
    if (date && (toMonthDate(date) === "0001-01" || date.toLocaleDateString("en-CA") === "1-01-01")) {
        return true
    }
    return false
}

export const isDefaultDateString = (dateString?: string | null): boolean => {
    const date = new Date(dateString ?? "")
    if (date && (toMonthDate(date) === "0001-01" || date.toLocaleDateString("en-CA") === "1-01-01")) {
        return true
    }
    return false
}

export const dateFromString = (dateString?: string | null): Date => new Date(dateString ?? "")

export const defaultDate = () => new Date("0001-01-01")

export const isInteger = (value: string) => /^-?\d+$/.test(value)

export const productionStrategyOverviewToString = (value?: Components.Schemas.ProductionStrategyOverview): string => {
    if (value === undefined) { return "" }
    return {
        0: "Depletion",
        1: "Water injection",
        2: "Gas injection",
        3: "WAG",
        4: "Mixed",
    }[value]
}

export const isExplorationWell = (well: Components.Schemas.WellDto | undefined) => [4, 5, 6].indexOf(well?.wellCategory ?? -1) > -1

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
    if (!t1) { return t2 || { id: "", startYear: 0, values: [] } }
    if (!t2) { return t1 }

    const startYears = [t1, t2].map((t: ITimeSeries | undefined) => t?.startYear ?? 0)
    const minYear = Math.min(...startYears)

    const arrays = [t1, t2].map((t: ITimeSeries | undefined) => t?.values ?? [])
    const offsets = startYears.map((year: number) => Math.abs(year - minYear))

    const mergedValues = mergeTimeSeriesValues(arrays, offsets)

    return {
        id: t1.id || t2.id || "",
        startYear: minYear,
        values: mergedValues,
    }
}

export const mergeTimeseriesList = (timeSeriesList: (ITimeSeries | undefined)[]): ITimeSeries => {
    let mergedTimeSeries: ITimeSeries = { id: "", startYear: 0, values: [] }

    timeSeriesList.forEach((currentSeries, index) => {
        if (index === 0) {
            mergedTimeSeries = currentSeries ?? mergedTimeSeries
        } else {
            mergedTimeSeries = mergeTimeseries(mergedTimeSeries, currentSeries)
        }
    })

    return mergedTimeSeries
}

export function formatDate(isoDateString: string): string {
    if (isoDateString === "0001-01-01T00:00:00+00:00" || isoDateString === "0001-01-01T00:00:00.000Z") {
        return "_"
    }
    const date = new Date(isoDateString)
    const options: Intl.DateTimeFormatOptions = {
        month: "long",
        year: "numeric",
    }
    return new Intl.DateTimeFormat("no-NO", options).format(date)
}

export const formatDateAndTime = (dateString: string | undefined | null) => {
    if (!dateString) { return "" }
    const date = new Date(dateString)
    const options: Intl.DateTimeFormatOptions = {
        day: "2-digit",
        month: "short",
        year: "numeric",
        hour: "2-digit",
        minute: "2-digit",
        hour12: false,
    }
    return new Intl.DateTimeFormat("en-GB", options)
        .format(date)
        .replace(",", "")
}

export const formatFullDate = (dateString: string | undefined | null) => {
    if (!dateString) { return "" }
    const date = new Date(dateString)
    const options: Intl.DateTimeFormatOptions = {
        day: "2-digit",
        month: "short",
        year: "numeric",
    }
    return new Intl.DateTimeFormat("en-GB", options)
        .format(date)
        .replace(",", "")
}

export const isWithinRange = (number: number, max: number, min: number) => number >= max && number <= min

export const preventNonDigitInput = (e: React.KeyboardEvent<HTMLInputElement>): void => {
    if (!/\d/.test(e.key)) { e.preventDefault() }
}

/**
 * Updates a specified property of an object and sets the updated object using a React state setter function.
 * If the property is already set to the provided value or if the object/value is not defined, logs an error and returns without updating.
*/
export function updateObject<T>(object: T | undefined, setObject: Dispatch<SetStateAction<T | undefined>>, key: keyof T, value: any): void {
    if (!object || !value) {
        console.error("Object or value is undefined")
        return
    }
    if (object[key] === value) {
        console.error("Object key is already set to value")
        return
    }
    const newObject: T = { ...object }
    newObject[key] = value
    setObject(newObject)
}
/**
 * Determines if a table cell is editable based on the provided parameters and edit mode.
 *
 * @param params The parameters of the table cell.
 * @param editMode A boolean indicating if the table is in edit mode.
 * @returns A boolean indicating if the cell is editable.
 * */
export const tableCellisEditable = (params: any, editMode: boolean): boolean => {
    if (!params || !params.node || !params.data) {
        return false
    }

    if (params.node.footer) {
        return false
    }

    if (params.data.overridable) {
        return editMode && params.data.override
    }

    return editMode && params.data.editable
}

export const validateInput = (params: any, editMode: boolean) => {
    const { value, data } = params
    if (tableCellisEditable(params, editMode) && editMode && value) {
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
    const valueWithOnlyNumbersCommasAndDots = newValue.toString().replace(/[^0-9.,]/g, "")
    const allCommasTurnedToDots = valueWithOnlyNumbersCommasAndDots.replace(/,/g, ".")

    if ((allCommasTurnedToDots.match(/\./g) || []).length > 1) {
        setSnackBarMessage("Only one decimal point is allowed. The entry was reset.")
        return oldValue
    }

    if (valueWithOnlyNumbersCommasAndDots !== newValue) {
        setSnackBarMessage("Only numbers, commas and dots are allowed. Invalid characters have been removed.")
    }

    return allCommasTurnedToDots
}

export const getCaseRowStyle = (params: any) => {
    if (params.node.footer) {
        return { fontWeight: "bold" }
    }
    return undefined
}

export const cellStyleRightAlign = { textAlign: "right" }

/**
 * Updates a state object with a non-negative number value.
 * If the provided value is negative, sets the object key to 0.
 * @param value The number value to set.
 * @param objectKey The key of the object to update.
 * @param state The state object to update.
 * @param setState The state setter function.
 * @returns void
 */
export const setNonNegativeNumberState = (value: number, objectKey: string, state: any, setState: Dispatch<SetStateAction<any>>): void => {
    const newState = { ...state }
    newState[objectKey] = Math.max(value, 0)
    setState(newState)
}

export const formatTime = (timestamp: number): string => {
    const date = new Date(timestamp)
    const hours = date.getHours()
    const minutes = date.getMinutes()

    // Pads single digits with a leading zero
    const formattedHours = hours.toString().padStart(2, "0")
    const formattedMinutes = minutes.toString().padStart(2, "0")

    return `${formattedHours}:${formattedMinutes}`
}

export const getCurrentEditId = (editIndexes: EditEntry[], caseId: string | undefined): string | undefined => {
    const currentCaseEditId = editIndexes.find((entry: EditEntry) => entry.caseId === caseId && entry.currentEditId)
    return (currentCaseEditId as unknown as EditEntry)?.currentEditId
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

export const extractTableTimeSeriesValues = (data: any) => {
    const tableTimeSeriesValues: { year: number, value: number }[] = []

    Object.keys(data).forEach((prop) => {
        const value = data[prop]

        if (
            isInteger(prop)
            && value !== ""
            && value !== null
            && !Number.isNaN(Number(value.toString().replace(/,/g, ".")))
        ) {
            tableTimeSeriesValues.push({
                year: parseInt(prop, 10),
                value: Number(value.toString().replace(/,/g, ".")),
            })
        }
    })

    return tableTimeSeriesValues.sort((a, b) => a.year - b.year)
}

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

export function truncateText(text: string, maxLength: number): string {
    return text.length > maxLength ? `${text.slice(0, maxLength)}...` : text
}
