import {
    useMemo,
    useState,
    useEffect,
} from "react"

import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { ColDef } from "@ag-grid-community/core"
import { useParams } from "react-router"
import {
    isExplorationWell,
    cellStyleRightAlign,
    extractTableTimeSeriesValues,
    generateProfile,
} from "../../../../Utils/common"
import { useAppContext } from "../../../../Context/AppContext"
import { useProjectContext } from "../../../../Context/ProjectContext"
import useDataEdits from "../../../../Hooks/useDataEdits"

interface Props {
    dg4Year: number
    tableYears: [number, number]
    tableName: string
    alignedGridsRef?: any[]
    gridRef?: any
    assetWells: Components.Schemas.ExplorationWellDto[] | Components.Schemas.WellProjectWellDto[]
    wells: Components.Schemas.WellDto[] | undefined
    resourceId: string
    isExplorationTable: boolean
}

interface IAssetWell {
    assetId: string
    wellId: string
    drillingSchedule: {
        id?: string
        startYear: number
        values: number[] | null
    }
}

const CaseDrillingScheduleTabTable = ({
    dg4Year,
    tableYears,
    tableName,
    alignedGridsRef,
    gridRef,
    assetWells,
    wells,
    resourceId,
    isExplorationTable,
}: Props) => {
    const styles = useStyles()
    const [rowData, setRowData] = useState<any[]>([])
    const [stagedEdit, setStagedEdit] = useState<any>()

    const { editMode } = useAppContext()
    const { project } = useProjectContext()
    const { addEdit } = useDataEdits()
    const { caseId } = useParams()

    const createMissingAssetWellsFromWells = (assetWell: any[]) => {
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
                            values: null,
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
                            values: null,
                        },
                    }
                    newAssetWells.push(newWellProjectWell)
                }
            })
        }

        return newAssetWells
    }

    const wellsToRowData = () => {
        const existingAndNewAssetWells = createMissingAssetWellsFromWells(assetWells)
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
            })
            setRowData(tableWells.filter((tw) => tw !== undefined))
        }
    }

    const generateTableYearColDefs = () => {
        const columnPinned: any[] = [
            {
                field: "name", headerName: tableName, width: 250, editable: false, pinned: "left",
            },
            {
                field: "total", flex: 2, editable: false, pinned: "right", width: 100, cellStyle: { fontWeight: "bold", textAlign: "right" },
            },
        ]
        const yearDefs: any[] = []
        for (let index = tableYears[0]; index <= tableYears[1]; index += 1) {
            yearDefs.push({
                field: index.toString(),
                flex: 1,
                minWidth: 100,
                editable: editMode,
                cellClass: editMode ? "editableCell" : undefined,
                cellStyle: cellStyleRightAlign,
            })
        }
        return columnPinned.concat([...yearDefs])
    }

    const [columnDefs, setColumnDefs] = useState<ColDef[]>(generateTableYearColDefs())

    const handleCellValueChange = (p: any) => {
        if (!caseId || !project) { return }

        const tableTimeSeriesValues = extractTableTimeSeriesValues(p.data)
        const newProfile = generateProfile(tableTimeSeriesValues, p.data.drillingSchedule, dg4Year)

        if (!newProfile) { return }

        const rowWells = p.data.assetWells
        if (rowWells) {
            const index = rowWells.findIndex((w: any) => w === p.data.assetWell)
            if (index > -1) {
                const well = rowWells[index]
                const updatedWell = { ...well, drillingSchedule: newProfile }
                const updatedWells = [...rowWells]
                updatedWells[index] = updatedWell

                const resourceName = isExplorationTable ? "explorationWellDrillingSchedule" : "wellProjectWellDrillingSchedule"

                setStagedEdit({
                    newValue: p.newValue,
                    previousValue: p.oldValue,
                    inputLabel: p.data.name,
                    projectId: project.id,
                    resourceName,
                    resourcePropertyKey: "drillingSchedule",
                    caseId,
                    resourceId,
                    newResourceObject: newProfile,
                    wellId: updatedWell.wellId,
                    drillingScheduleId: newProfile.id,
                })
            }
        }
    }

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        editable: true,
        onCellValueChanged: handleCellValueChange,
        suppressHeaderMenuButton: true,
    }), [assetWells])

    const gridRefArrayToAlignedGrid = () => {
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

    useEffect(() => {
        wellsToRowData()
        const newColDefs = generateTableYearColDefs()
        setColumnDefs(newColDefs)
    }, [assetWells, tableYears, wells, editMode])

    useEffect(() => {
        if (stagedEdit) {
            addEdit(stagedEdit)
        }
    }, [stagedEdit])

    return (
        <div className={styles.root}>
            <div
                style={{
                    display: "flex", flexDirection: "column", width: "100%",
                }}
            >
                <AgGridReact
                    ref={gridRef}
                    rowData={rowData}
                    columnDefs={columnDefs}
                    defaultColDef={defaultColDef}
                    animateRows
                    domLayout="autoHeight"
                    enableCellChangeFlash
                    rowSelection="multiple"
                    enableRangeSelection
                    suppressCopySingleCellRanges
                    suppressMovableColumns
                    enableCharts
                    alignedGrids={gridRefArrayToAlignedGrid()}
                    stopEditingWhenCellsLoseFocus
                />
            </div>
        </div>
    )
}

export default CaseDrillingScheduleTabTable
