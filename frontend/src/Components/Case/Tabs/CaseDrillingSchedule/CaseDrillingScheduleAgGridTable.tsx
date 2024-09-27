import {
    useMemo,
    useState,
    useEffect,
    useRef,
    useCallback,
} from "react"

import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { CellKeyDownEvent, ColDef } from "@ag-grid-community/core"
import { useParams } from "react-router"
import isEqual from "lodash/isEqual"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import {
    isExplorationWell,
    cellStyleRightAlign,
    extractTableTimeSeriesValues,
    generateProfile,
    numberValueParser,
} from "../../../../Utils/common"
import { useAppContext } from "../../../../Context/AppContext"
import { useProjectContext } from "../../../../Context/ProjectContext"

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
    addEdit: any
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
    addEdit,
}: Props) => {
    const { editMode, setSnackBarMessage } = useAppContext()
    const { caseId, tab } = useParams()
    const { projectId } = useProjectContext()
    const styles = useStyles()

    const [rowData, setRowData] = useState<any[]>([])
    const [stagedEdit, setStagedEdit] = useState<any>()

    const firstTriggerRef = useRef<boolean>(true)
    const timerRef = useRef<NodeJS.Timeout | null>(null)

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
                return undefined
            })
            setRowData(tableWells.filter((tw) => tw !== undefined))
        }
    }

    const generateTableYearColDefs = () => {
        const columnPinned: any[] = [
            {
                field: "name",
                headerName: tableName,
                width: 250,
                editable: false,
                pinned: "left",
            },
            {
                field: "total",
                flex: 2,
                editable: false,
                pinned: "right",
                width: 100,
                cellStyle: { fontWeight: "bold", textAlign: "right" },
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
                valueParser: (params: any) => numberValueParser(setSnackBarMessage, params),
            })
        }
        return columnPinned.concat([...yearDefs])
    }

    const [columnDefs, setColumnDefs] = useState<ColDef[]>(generateTableYearColDefs())

    const stageEdit = (params: any) => {
        const tableTimeSeriesValues = extractTableTimeSeriesValues(params.data)
        const existingProfile = params.data.drillingSchedule ? structuredClone(params.data.drillingSchedule) : {
            startYear: 0,
            values: [],
            id: resourceId,
        }

        let newProfile
        if (tableTimeSeriesValues.length > 0) {
            const firstYear = tableTimeSeriesValues[0].year
            const lastYear = tableTimeSeriesValues[tableTimeSeriesValues.length - 1].year
            const startYear = firstYear - dg4Year
            newProfile = generateProfile(tableTimeSeriesValues, params.data.drillingSchedule, startYear, firstYear, lastYear)
        } else {
            newProfile = structuredClone(existingProfile)
            newProfile.values = []
        }

        if (!caseId || projectId === "" || !newProfile) { return }

        const rowWells = params.data.assetWells
        if (rowWells) {
            const index = rowWells.findIndex((w: any) => w === params.data.assetWell)
            if (index > -1) {
                const well = rowWells[index]
                const updatedWell = { ...well, drillingSchedule: newProfile }
                const updatedWells = [...rowWells]
                updatedWells[index] = updatedWell
                const resourceName = isExplorationTable ? "explorationWellDrillingSchedule" : "wellProjectWellDrillingSchedule"

                if (!isEqual(newProfile.values, existingProfile.values)) {
                    setStagedEdit({
                        newDisplayValue: newProfile.values.map((value: string) => Math.floor(Number(value) * 10000) / 10000).join(" - "),
                        previousDisplayValue: existingProfile.values.map((value: string) => Math.floor(Number(value) * 10000) / 10000).join(" - "),
                        inputLabel: params.data.name,
                        projectId,
                        resourceName,
                        resourcePropertyKey: "drillingSchedule",
                        caseId,
                        resourceId,
                        newResourceObject: newProfile,
                        previousResourceObject: existingProfile,
                        wellId: updatedWell.wellId,
                        drillingScheduleId: newProfile.id,
                        tabName: tab,
                        tableName,
                    })
                }
            }
        }
    }

    const handleCellValueChange = useCallback((params: any) => {
        if (firstTriggerRef.current) {
            firstTriggerRef.current = false
            stageEdit(params)

            if (timerRef.current) {
                clearTimeout(timerRef.current)
            }

            timerRef.current = setTimeout(() => {
                firstTriggerRef.current = true
            }, 0) // TODO: Can this be removed?
        }
    }, [stageEdit, dg4Year, projectId, isExplorationTable])

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

    const clearCellsInRange = (start: number, end: number, columns: any[]) => {
        for (let rowIndex = start; rowIndex <= end; rowIndex += 1) {
            const rowNode = gridRef.current?.api.getRowNode(rowIndex)
            if (rowNode) {
                columns.forEach((col: any) => {
                    rowNode.setDataValue(col, "")
                })
            }
        }
    }

    const handleDeleteOnRange = useCallback(
        (e: CellKeyDownEvent) => {
            const keyboardEvent = e.event as unknown as KeyboardEvent
            const { key } = keyboardEvent

            const cellRanges = e.api.getCellRanges()

            if (key === "Backspace") {
                if (!cellRanges || cellRanges.length === 0) {
                    return
                }

                cellRanges.forEach((range: any) => {
                    const { startRow, endRow, columns } = range
                    const startRowIndex = Math.min(startRow.rowIndex, endRow.rowIndex)
                    const endRowIndex = Math.max(startRow.rowIndex, endRow.rowIndex)

                    const colIds = columns.map((col: any) => col.getColId())

                    if (startRowIndex === endRowIndex && colIds.length === 1) {
                        const colId = colIds[0]
                        const cellValue = e.api.getValue(colId, e.node)
                        if (cellValue !== undefined && cellValue !== null && typeof cellValue === "string") {
                            const newValue = cellValue.slice(0, -1)
                            e.node.setDataValue(colId, newValue)
                        }
                    } else {
                        clearCellsInRange(startRowIndex, endRowIndex, colIds)
                    }
                })
            }
        },
        [clearCellsInRange],
    )

    return (
        <div className={styles.root}>
            <div
                id={tableName}
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
                    suppressLastEmptyLineOnPaste
                    onCellKeyDown={editMode ? handleDeleteOnRange : undefined}
                />
            </div>
        </div>
    )
}

export default CaseDrillingScheduleTabTable
