import {
    useMemo,
    useState,
    useEffect,
} from "react"

import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { ColDef } from "@ag-grid-community/core"
import { useParams } from "react-router"
import { isExplorationWell, isInteger } from "../../../../Utils/common"
import { EMPTY_GUID } from "../../../../Utils/constants"
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
    setAssetWells: (assetWell: any[]) => void
    wells: Components.Schemas.WellDto[] | undefined
    resourceId: string
    isExplorationTable: boolean
}

const CaseDrillingScheduleTabTable = ({
    dg4Year,
    tableYears, tableName,
    alignedGridsRef, gridRef,
    assetWells, setAssetWells,
    wells, resourceId, isExplorationTable,
}: Props) => {
    const styles = useStyles()
    const [rowData, setRowData] = useState<any[]>([])
    const { editMode } = useAppContext()
    const { project } = useProjectContext()
    const { addEdit } = useDataEdits()
    const { caseId } = useParams()

    const createMissingAssetWellsFromWells = (assetWell: any[]) => {
        const newAssetWells: (Components.Schemas.ExplorationWellDto | Components.Schemas.WellProjectWellDto)[] = [...assetWells]
        if (isExplorationTable) {
            wells?.filter((w) => isExplorationWell(w)).forEach((w) => {
                const explorationWell = assetWell.find((ew) => ew.wellId === w.id)
                if (!explorationWell) {
                    const newExplorationWell = {
                        explorationId: resourceId,
                        wellId: w.id,
                        drillingSchedule: {
                            id: EMPTY_GUID,
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
                        wellProjectId: resourceId,
                        wellId: w.id,
                        drillingSchedule: {
                            id: EMPTY_GUID,
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
            const tableWells: any[] = []
            existingAndNewAssetWells.forEach((w) => {
                const name = wells?.find((well) => well.id === w.wellId)?.name
                const tableWell: any = {
                    name: name ?? "",
                    total: 0,
                    assetWell: w,
                    assetWells: existingAndNewAssetWells,
                    drillingSchedule: w.drillingSchedule,
                }
                if (tableWell.drillingSchedule.values && tableWell.drillingSchedule.values.length > 0
                    && tableWell.drillingSchedule.startYear !== undefined) {
                    let j = 0
                    for (let i = tableWell.drillingSchedule.startYear;
                        i < tableWell.drillingSchedule.startYear + tableWell.drillingSchedule.values.length; i += 1) {
                        tableWell[(dg4Year + i).toString()] = tableWell.drillingSchedule.values[j]
                        j += 1
                        tableWell.total = tableWell.drillingSchedule.values.reduce((x: number, y: number) => x + y)
                    }
                }
                tableWells.push(tableWell)
            })
            setRowData(tableWells)
        }
    }

    const generateTableYearColDefs = () => {
        const columnPinned: any[] = [
            {
                field: "name", headerName: tableName, width: 250, editable: false, pinned: "left",
            },
            {
                field: "total", flex: 2, editable: false, pinned: "right", width: 100,
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
            })
        }
        return columnPinned.concat([...yearDefs])
    }

    const [columnDefs, setColumnDefs] = useState<ColDef[]>(generateTableYearColDefs())

    const handleCellValueChange = (p: any) => {
        if (!caseId || !project) { return }
        const properties = Object.keys(p.data)
        const tableTimeSeriesValues: any[] = []
        properties.forEach((prop) => {
            if (isInteger(prop)
                && p.data[prop] !== ""
                && p.data[prop] !== null
                && !Number.isNaN(Number(p.data[prop].toString().replace(/,/g, ".")))) {
                tableTimeSeriesValues.push({
                    year: parseInt(prop, 10),
                    value: Number(p.data[prop].toString().replace(/,/g, ".")),
                })
            }
        })
        tableTimeSeriesValues.sort((a, b) => a.year - b.year)
        if (tableTimeSeriesValues.length > 0) {
            const tableTimeSeriesFirstYear = tableTimeSeriesValues[0].year
            const tableTimeSerieslastYear = tableTimeSeriesValues.at(-1).year
            const timeSeriesStartYear = tableTimeSeriesFirstYear - dg4Year
            const values: number[] = []
            for (let i = tableTimeSeriesFirstYear; i <= tableTimeSerieslastYear; i += 1) {
                const tableTimeSeriesValue = tableTimeSeriesValues.find((v) => v.year === i)
                if (tableTimeSeriesValue) {
                    values.push(tableTimeSeriesValue.value)
                } else {
                    values.push(0)
                }
            }
            const newProfile = { ...p.data.drillingSchedule }
            newProfile.startYear = timeSeriesStartYear
            newProfile.values = values
            const rowWells: Components.Schemas.ExplorationWellDto[] | Components.Schemas.WellProjectWellDto[] = p.data.assetWells

            if (rowWells) {
                const index = rowWells.findIndex((w) => w === p.data.assetWell)
                if (index > -1) {
                    const well = rowWells[index]
                    const updatedWell = well
                    updatedWell.drillingSchedule = newProfile
                    const updatedWells: any[] = [...rowWells]
                    updatedWells[index] = updatedWell
                    setAssetWells(updatedWells) // remove after addEdit works

                    console.log(updatedWell)
                    console.log(updatedWells)
                    console.log(p)
                    console.log(p.data.assetWell.explorationId)

                    const { explorationId } = p.data.assetWell
                    const { wellProjectId } = p.data.assetWell

                    console.log(updatedWell.wellId)

                    const resourceName = explorationId ? "explorationWellDrillingSchedule" : "wellProjectWellDrillingSchedule"

                    addEdit({
                        newValue: p.newValue,
                        previousValue: p.oldValue,
                        inputLabel: p.data.profileName,
                        projectId: project.id,
                        resourceName,
                        resourcePropertyKey: "drillingSchedule",
                        caseId,
                        resourceId: explorationId || wellProjectId,
                        newResourceObject: newProfile,
                        wellId: updatedWell.wellId,
                        drillingScheduleId: newProfile.id,
                    })
                }
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
    }), [])

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
                    singleClickEdit={editMode}
                    stopEditingWhenCellsLoseFocus
                />
            </div>
        </div>
    )
}

export default CaseDrillingScheduleTabTable
