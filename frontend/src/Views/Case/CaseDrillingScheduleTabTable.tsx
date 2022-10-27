import {
    Dispatch,
    SetStateAction,
    useMemo,
    useState,
    useRef,
    useEffect,
} from "react"

import { AgGridReact } from "ag-grid-react"
import { useAgGridStyles } from "@equinor/fusion-react-ag-grid-addons"
import { Project } from "../../models/Project"
import { Case } from "../../models/case/Case"
import "ag-grid-enterprise"
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
    // timeSeriesData: any[]
    dg4Year: number
    tableYears: [number, number]
    tableName: string
    alignedGridsRef?: any[]
    gridRef?: any
    assetWells: ExplorationWell[] | WellProjectWell[]
    setAssetWell: any
    wells: Well[] | undefined
    assetId: string
}

interface TableAssetWell {
    // id: string,
    name: string,
    assetWell: ExplorationWell | WellProjectWell
    assetWells: ExplorationWell[] | WellProjectWell[]
    drillingSchedule: DrillingSchedule
}

function CaseDrillingScheduleTabTable({
    project, setProject,
    caseItem, setCase,
    // timeSeriesData,
    dg4Year,
    tableYears, tableName,
    alignedGridsRef, gridRef,
    assetWells, setAssetWell,
    wells, assetId,
}: Props) {
    useAgGridStyles()
    const [rowData, setRowData] = useState<any[]>([{ name: "as" }])

    // 1. funksjon som lager tomme ExplorationWells fra wells av kategori exploration
    const createMissingExplorationWellsFromWells = (expWell: ExplorationWell[]) => {
        const newExplorationWells: ExplorationWell[] = [...assetWells]
        wells?.filter((w) => IsExplorationWell(w)).forEach((w) => {
            const explorationWell = expWell.find((ew) => ew.wellId === w.id)
            if (!explorationWell) {
                const newExplorationWell = new ExplorationWell()
                newExplorationWell.explorationId = assetId
                newExplorationWell.wellId = w.id
                newExplorationWells.push(newExplorationWell)
            }
        })

        return newExplorationWells
    }

    const wellsToRowData = () => {
        const assetWellsWithEmpty = createMissingExplorationWellsFromWells(assetWells)
        if (assetWellsWithEmpty) {
            const tableWells: any[] = []
            assetWellsWithEmpty.forEach((w) => {
                const name = wells?.find((well) => well.id === w.wellId)?.name
                const tableWell: any = {
                    name: name ?? "",
                    assetWell: w,
                    assetWells: assetWellsWithEmpty,
                    drillingSchedule: w.drillingSchedule ?? new DrillingSchedule(),
                }
                if (tableWell.drillingSchedule.values && tableWell.drillingSchedule.values.length > 0
                    && tableWell.drillingSchedule.startYear !== undefined) {
                    let j = 0
                    for (let i = tableWell.drillingSchedule.startYear;
                        i < tableWell.drillingSchedule.startYear + tableWell.drillingSchedule.values.length; i += 1) {
                            tableWell[(dg4Year + i).toString()] = tableWell.drillingSchedule.values[j]
                        j += 1
                    }
                }
                tableWells.push(tableWell)
            })
            setRowData(tableWells)
        }
    }

    const generateTableYearColDefs = () => {
        const profileNameDef = {
            field: "name", width: 250, editable: false,
        }
        const yearDefs = []
        for (let index = tableYears[0]; index <= tableYears[1]; index += 1) {
            yearDefs.push({
                field: index.toString(),
                flex: 1,
                minWidth: 100,
            })
        }
        const totalDef = { field: "total", flex: 2, editable: false }
        return [profileNameDef, ...yearDefs, totalDef]
    }

    const [columnDefs, setColumnDefs] = useState(generateTableYearColDefs())

    useEffect(() => {
        wellsToRowData()
        const newColDefs = generateTableYearColDefs()
        setColumnDefs(newColDefs)
    }, [assetWells, tableYears])

    const handleCellValueChange = (p: any) => {
        debugger
        const properties = Object.keys(p.data)
        const tableTimeSeriesValues: any[] = []
        properties.forEach((prop) => {
            if (isInteger(prop)
                && p.data[prop] !== ""
                && p.data[prop] !== null
                && !Number.isNaN(Number(p.data[prop]))) {
                tableTimeSeriesValues.push({ year: parseInt(prop, 10), value: Number(p.data[prop]) })
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
            // p.data.set(newProfile)
            const rowWells: ExplorationWell[] | WellProjectWell[] = p.data.assetWells
            if (rowWells) {
                const { field } = p.colDef
                const index = rowWells.findIndex((w) => w === p.data.assetWell)
                if (index > -1) {
                    const well = rowWells[index]
                    const updatedWell = well
                    // eslint-disable-next-line max-len
                    // updatedWell[field as keyof typeof updatedWell] = field === "name" ? p.newValue : Number(p.newValue)
                    updatedWell.drillingSchedule = newProfile
                    const updatedWells = [...rowWells]
                    updatedWells[index] = updatedWell
                    setAssetWell(updatedWells)
                }
            }
        }
    }

    const updateWells = (p: any) => {
        const rowWells: ExplorationWell[] | WellProjectWell[] = p.data.assetWells
        if (rowWells) {
            const { field } = p.colDef
            const index = rowWells.findIndex((w) => w === p.data.assetWell)
            if (index > -1) {
                const well = rowWells[index]
                const updatedWell = well
                updatedWell[field as keyof typeof updatedWell] = field === "name" ? p.newValue : Number(p.newValue)
                const updatedWells = [...rowWells]
                updatedWells[index] = updatedWell
                setAssetWell(updatedWells)
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
