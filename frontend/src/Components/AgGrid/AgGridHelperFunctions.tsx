import { ITimeSeriesTableData, ITimeSeriesTableDataWithSet } from "@/Models/ITimeSeries"
import { isExplorationWell } from "@/Utils/common"

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
    assetWell: any[],
    wells: Components.Schemas.WellOverviewDto[] | undefined,
    resourceId: string,
    dg4Year: number,
    isExplorationTable: boolean,
) => {
    const newAssetWells: (IAssetWell)[] = assetWell.map((w) => ({
        assetId: w.explorationId || w.wellProjectId,
        wellId: w.wellId,
        drillingSchedule: w.drillingSchedule,
    }))
    if (isExplorationTable) {
        wells?.filter((w) => isExplorationWell(w)).forEach((w) => {
            const explorationWell = assetWell.find((ew) => ew.wellId === w.id)
            if (!explorationWell) {
                const newExplorationWell = {
                    assetId: resourceId,
                    wellId: w.id,
                    drillingSchedule: {
                        startYear: dg4Year,
                        values: [],
                    },
                }
                newAssetWells.push(newExplorationWell)
            }
        })
    } else {
        wells?.filter((w) => !isExplorationWell(w)).forEach((w) => {
            const wellProjectWell = assetWell.find((wpw) => wpw.wellId === w.id)
            if (!wellProjectWell) {
                const newWellProjectWell = {
                    assetId: resourceId,
                    wellId: w.id,
                    drillingSchedule: {
                        startYear: dg4Year,
                        values: [],
                    },
                }
                newAssetWells.push(newWellProjectWell)
            }
        })
    }

    return newAssetWells
}

export const wellsToRowData = (
    assetWells: Components.Schemas.ExplorationWellDto[] | Components.Schemas.WellProjectWellDto[],
    wells: Components.Schemas.WellOverviewDto[] | undefined,
    dg4Year: number,
    editMode: boolean,
    resourceId: string,
    isExplorationTable: boolean,
) => {
    const existingAndNewAssetWells = createMissingAssetWellsFromWells(assetWells, wells, resourceId, dg4Year, isExplorationTable)
    if (existingAndNewAssetWells) {
        const tableWells = existingAndNewAssetWells.map((w) => {
            const name = wells?.find((well) => well.id === w.wellId)?.name ?? ""
            const tableWell: any = {
                name,
                total: 0,
                assetWell: w,
                assetWells: existingAndNewAssetWells,
                drillingSchedule: w.drillingSchedule,
            }

            if (tableWell.drillingSchedule.values && tableWell.drillingSchedule.values.length > 0
                    && tableWell.drillingSchedule.startYear !== undefined) {
                tableWell.drillingSchedule.values.forEach((value: any, index: any) => {
                    const yearKey = (dg4Year + tableWell.drillingSchedule.startYear + index).toString()
                    tableWell[yearKey] = value
                })
                tableWell.total = tableWell.drillingSchedule.values.reduce((acc: any, val: any) => acc + val, 0)
            }
            if ((!editMode && tableWell.total > 0) || editMode) {
                return tableWell
            }
            return undefined
        })
        const tableRows = tableWells.filter((tw) => tw !== undefined)
        return tableRows
    }
    return []
}

export const profilesToRowData = (
    timeSeriesData: ITimeSeriesTableData[],
    dg4Year: number,
    tableName: string,
    editMode: boolean,
) => {
    console.log("timeSeriesData", timeSeriesData)
    const tableRows: ITimeSeriesTableData[] = []
    timeSeriesData.forEach((ts: ITimeSeriesTableDataWithSet) => {
        const isOverridden = ts.overrideProfile?.override === true
        const rowObject: any = {}
        const { profileName, unit } = ts
        rowObject.profileName = profileName
        rowObject.unit = unit
        rowObject.total = ts.total ?? 0
        rowObject.set = isOverridden ? ts.overrideProfileSet : ts.set
        rowObject.profile = isOverridden ? ts.overrideProfile : ts.profile
        rowObject.override = ts.overrideProfile?.override === true
        rowObject.resourceId = ts.resourceId
        rowObject.resourceName = ts.resourceName
        rowObject.overridable = ts.overridable
        rowObject.editable = ts.editable
        rowObject.hideIfEmpty = ts.hideIfEmpty

        rowObject.overrideProfileSet = ts.overrideProfileSet
        rowObject.overrideProfile = ts.overrideProfile ?? {
            startYear: 0, values: [], override: false,
        }

        if (rowObject.profile && rowObject.profile.values?.length > 0) {
            let j = 0
            if (tableName === "Production profiles" || tableName === "CO2 emissions") {
                for (let i = rowObject.profile.startYear;
                    i < rowObject.profile.startYear + rowObject.profile.values.length;
                    i += 1) {
                    rowObject[(dg4Year + i).toString()] = rowObject.profile.values.map(
                        (v: number) => Math.round((v + Number.EPSILON) * 10000) / 10000,
                    )[j]
                    j += 1
                    rowObject.total = Math.round(rowObject.profile.values.map(
                        (v: number) => (v + Number.EPSILON),
                    ).reduce((x: number, y: number) => x + y) * 10000) / 10000
                    if (ts.total !== undefined) {
                        rowObject.total = Math.round(Number(ts.total) * 1000) / 1000
                    }
                }
            } else {
                for (let i = rowObject.profile.startYear;
                    i < rowObject.profile.startYear + rowObject.profile.values.length;
                    i += 1) {
                    rowObject[(dg4Year + i).toString()] = rowObject.profile.values.map(
                        (v: number) => Math.round((v + Number.EPSILON) * 10) / 10,
                    )[j]
                    j += 1
                    rowObject.total = Math.round(rowObject.profile.values.map(
                        (v: number) => (v + Number.EPSILON),
                    ).reduce((x: number, y: number) => x + y) * 10) / 10
                }
            }
        }

        const isNotHidden = !rowObject.hideIfEmpty
        const hasProfileValues = rowObject.hideIfEmpty && rowObject.profile?.values.length > 0

        if (editMode || isNotHidden || hasProfileValues) {
            tableRows.push(rowObject)
        }
    })

    return tableRows
}
