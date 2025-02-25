import { ITimeSeriesTableData, ITimeSeriesTableDataWithSet } from "@/Models/ITimeSeries"
import { generateProfile, isExplorationWell } from "@/Utils/common"

interface IAssetWell {
    assetId: string
    wellId: string
    drillingSchedule: {
        id?: string
        startYear: number
        values: number[]
    }
}

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
    assetWells: Components.Schemas.ExplorationWellDto[] | Components.Schemas.DevelopmentWellDto[],
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
