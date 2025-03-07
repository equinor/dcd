import { AxiosError } from "axios"
import { Dispatch, SetStateAction } from "react"
import isEqual from "lodash/isEqual"
import { v4 as uuidv4 } from "uuid"

import { ITimeSeries } from "@/Models/ITimeSeries"
import { TABLE_VALIDATION_RULES } from "@/Utils/constants"
import { EditEntry, EditInstance } from "@/Models/Interfaces"
import { dateFromTimestamp } from "@/Utils/DateUtils"
import { WellCategory } from "@/Models/enums"

export const loginAccessTokenKey = "loginAccessToken"
export const FusionAccessTokenKey = "fusionAccessToken"

export const projectPath = (projectId: string) => `/${projectId}`

export const casePath = (projectId: string, caseId: string) => `${projectPath(projectId)}/case/${caseId}`

export const caseRevisionPath = (projectId: string, caseId: string, isRevision: boolean, revisionId?: string) => {
    if (isRevision && revisionId) {
        return `${projectPath(projectId)}/revision/${revisionId}/case/${caseId}`
    }
    return `${projectPath(projectId)}/case/${caseId}`
}
export const storeAppId = (appId: string) => {
    window.sessionStorage.setItem("appId", appId)
}

export const storeAppScope = (appScope: string) => {
    window.sessionStorage.setItem("appScope", appScope)
}

export const getToken = () => {
    const scopes = [[window.sessionStorage.getItem("appScope") || ""][0]]
    return window.Fusion.modules.auth.acquireAccessToken({ scopes })
}

export const unwrapProjectId = (projectId?: string | undefined | null): string => {
    if (projectId === undefined || projectId === null) {
        throw new Error("Attempted to use a Project ID which does not exist")
    }
    return projectId
}

export const isExplorationWell = (well: Components.Schemas.WellOverviewDto | undefined) => [4, 5, 6].indexOf(well?.wellCategory ?? -1) > -1

export const developmentWellOptions = [
    { key: "0", value: WellCategory.OilProducer, label: "Oil producer" },
    { key: "1", value: WellCategory.GasProducer, label: "Gas producer" },
    { key: "2", value: WellCategory.WaterInjector, label: "Water injector" },
    { key: "3", value: WellCategory.GasInjector, label: "Gas injector" },
]

export const explorationWellOptions = [
    { key: "4", value: WellCategory.ExplorationWell, label: "Exploration well" },
    { key: "5", value: WellCategory.AppraisalWell, label: "Appraisal well" },
    { key: "6", value: WellCategory.Sidetrack, label: "Sidetrack" },
]

export const filterWells = (wells: Components.Schemas.WellOverviewDto[]) => {
    if (!wells) {
        return {
            explorationWells: [],
            developmentWells: [],
            developmentWellOptions,
            explorationWellOptions,
        }
    }
    return {
        explorationWells: wells.filter((well) => isExplorationWell(well)),
        developmentWells: wells.filter((well) => !isExplorationWell(well)),
        developmentWellOptions,
        explorationWellOptions,
    }
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
 * @param editAllowed A boolean indicating if the table can be eddited by user.
 * @returns A boolean indicating if the cell is editable.
 * */
export const tableCellisEditable = (params: any, editAllowed: boolean): boolean => {
    if (!params || !params.node || !params.data) {
        return false
    }

    if (params.node.footer) {
        return false
    }

    if (params.data.overridable) {
        return editAllowed && params.data.override
    }

    return editAllowed && params.data.editable
}

export const validateInput = (params: any, editAllowed: boolean) => {
    const { value, data } = params
    if (tableCellisEditable(params, editAllowed) && value) {
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
    const date = dateFromTimestamp(timestamp)
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

export const getValuesFromEntireRow = (tableData: any) => {
    const valuePerYear: { year: number, value: number }[] = []

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
    return (text.length + 3) > maxLength ? `${text.slice(0, maxLength)}...` : text
}

export function isAxiosError(error: unknown): error is AxiosError {
    return (error as AxiosError).isAxiosError !== undefined
}

export const defaultAxesData = [
    {
        type: "category",
        position: "bottom",
        gridLine: {
            style: [
                {
                    stroke: "rgba(0, 0, 0, 0.2)",
                    lineDash: [3, 2],
                },
                {
                    stroke: "rgba(0, 0, 0, 0.2)",
                    lineDash: [3, 2],
                },
            ],
        },
        label: {
            formatter: (label: any) => Math.floor(Number(label.value)),
        },
    },
    {
        type: "number",
        position: "left",
    },
]

export const roundToFourDecimalsAndJoin = (values: string[]): string => values.map((value) => Math.floor(Number(value) * 10000) / 10000).join(" - ")

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
    const editInstance: EditInstance = {
        uuid: uuidv4(),
        projectId,
        caseId,
        resourceName: profileInTimeSeriesData?.resourceName,
        resourcePropertyKey: profileInTimeSeriesData?.resourcePropertyKey,
        resourceId: profileInTimeSeriesData?.resourceId,
        wellId: params.data.wellId,
        resourceObject: newProfile,
    }

    /*
    return {
        inputLabel: profileName,
        projectId,
        resourceName: profileInTimeSeriesData?.resourceName,
        resourcePropertyKey: profileInTimeSeriesData?.resourcePropertyKey,
        caseId,
        resourceId: profileInTimeSeriesData?.resourceId,
        newResourceObject: newProfile,
        previousResourceObject: existingProfile,
        tabName: tab,
        tableName,
    }
        */

    return editInstance
}

export const sortVersions = (versions: string[]): string[] => versions.sort((a, b) => {
    const [aMajor, aMinor, aPatch] = a.split(".").map(Number)
    const [bMajor, bMinor, bPatch] = b.split(".").map(Number)

    if (aMajor !== bMajor) { return bMajor - aMajor }
    if (aMinor !== bMinor) { return bMinor - aMinor }
    return bPatch - aPatch
})

export const getLastForcedReloadDate = () => window.localStorage.getItem("forcedReloadDate")

export const setLastForcedReloadDate = (date: string) => window.localStorage.setItem("forcedReloadDate", date)
