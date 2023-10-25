import {
    Dispatch,
    SetStateAction,
    useMemo,
    useState,
    useEffect,
} from "react"

import { AgGridReact } from "@ag-grid-community/react"
import { Project } from "../../models/Project"
import { Case } from "../../models/case/Case"
import { IsExplorationWell, isInteger } from "../../Utils/common"
import { DrillingSchedule } from "../../models/assets/wellproject/DrillingSchedule"
import { WellProjectWell } from "../../models/WellProjectWell"
import { ExplorationWell } from "../../models/ExplorationWell"
import { Well } from "../../models/Well"

interface Props {
    project: Project,
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    caseItem: Case,
    setCase: Dispatch<SetStateAction<Case | undefined>>,
    dg4Year: number
    tableYears: [number, number]
    tableName: string
    alignedGridsRef?: any[]
    gridRef?: any
    assetWells: ExplorationWell[] | WellProjectWell[]
    setAssetWells: Dispatch<SetStateAction<ExplorationWell[] | WellProjectWell[] | undefined>>
    wells: Well[] | undefined
    assetId: string
    isExplorationTable: boolean
}

function CaseDrillingScheduleTabTable({
    project, setProject,
    caseItem, setCase,
    dg4Year,
    tableYears, tableName,
    alignedGridsRef, gridRef,
    assetWells, setAssetWells,
    wells, assetId, isExplorationTable,
}: Props) {
    const [rowData, setRowData] = useState<any[]>([])

    const createMissingAssetWellsFromWells = (assetWell: any[]) => {
        const newAssetWells: ExplorationWell[] | WellProjectWell[] = [...assetWells]
        if (isExplorationTable) {
            wells?.filter((w) => IsExplorationWell(w)).forEach((w) => {
                const explorationWell = assetWell.find((ew) => ew.wellId === w.id)
                if (!explorationWell) {
                    const newExplorationWell = new ExplorationWell()
                    newExplorationWell.explorationId = assetId
                    newExplorationWell.wellId = w.id
                    newAssetWells.push(newExplorationWell)
                }
            })
        } else {
            wells?.filter((w) => !IsExplorationWell(w)).forEach((w) => {
                const wellProjectWell = assetWell.find((wpw) => wpw.wellId === w.id)
                if (!wellProjectWell) {
                    const newWellProjectWell = new WellProjectWell()
                    newWellProjectWell.wellProjectId = assetId
                    newWellProjectWell.wellId = w.id
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
                    drillingSchedule: w.drillingSchedule ?? new DrillingSchedule(),
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
            })
        }
        return columnPinned.concat([...yearDefs])
    }

    const [columnDefs, setColumnDefs] = useState(generateTableYearColDefs())

    useEffect(() => {
        wellsToRowData()
        const newColDefs = generateTableYearColDefs()
        setColumnDefs(newColDefs)
    }, [assetWells, tableYears, wells])

    const handleCellValueChange = (p: any) => {
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
            const rowWells: ExplorationWell[] | WellProjectWell[] = p.data.assetWells
            if (rowWells) {
                const { field } = p.colDef
                const index = rowWells.findIndex((w) => w === p.data.assetWell)
                if (index > -1) {
                    const well = rowWells[index]
                    const updatedWell = well
                    updatedWell.drillingSchedule = newProfile
                    const updatedWells = [...rowWells]
                    updatedWells[index] = updatedWell
                    setAssetWells(updatedWells)
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

    return (
        <div
            style={{
                display: "flex", flexDirection: "column", width: "100%",
            }}
            className="ag-theme-alpine"
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
            />
        </div>
    )
}

export default CaseDrillingScheduleTabTable
