import { Button, Table } from "@equinor/eds-core-react"
import {
    Dispatch, SetStateAction, useEffect, useMemo, useRef, useState,
} from "react"
import { AgGridReact } from "ag-grid-react"
import { useAgGridStyles } from "@equinor/fusion-react-ag-grid-addons"
import { Project } from "../../models/Project"
import { Well } from "../../models/Well"
import { GetWellService } from "../../Services/WellService"
import { IsExplorationWell } from "../../Utils/common"
import WellTableRowEditTechnicalInput from "./WellTableRowEditTechnicalInput"
import "ag-grid-enterprise"

interface Props {
    project: Project
    setProject: Dispatch<SetStateAction<Project | undefined>>
    wells: Well[]
    setWells: Dispatch<SetStateAction<Well[]>>
    explorationWells: boolean
}

interface TableWell {
    id: string,
    name: string,
    wellCategory: Components.Schemas.WellCategory,
    drillingDays: number,
    wellCost: number,
}

function WellListEditTechnicalInputNew({
    project, setProject, explorationWells, wells, setWells,
}: Props) {
    const gridRef = useRef(null)

    const onGridReady = (params: any) => {
        gridRef.current = params.api
    }

    const [rowData, setRowData] = useState<TableWell[]>()

    const wellsToRowData = () => {
        if (wells) {
            const tableWells: TableWell[] = []
            wells.forEach((c) => {
                const tableWell: TableWell = {
                    id: c.id!,
                    name: c.name ?? "",
                    wellCategory: c.wellCategory ?? 0,
                    drillingDays: c.drillingDays ?? 0,
                    wellCost: c.wellCost ?? 0,

                }
                tableWells.push(tableWell)
            })
            setRowData(tableWells)
        }
    }

    useEffect(() => {
        console.log("useEffect", wells)
        wellsToRowData()
    }, [wells])

    type SortOrder = "desc" | "asc" | null
    const order: SortOrder = "asc"

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
    }), [])

    const [columnDefs] = useState([
        { field: "name", sort: order },
        {
            field: "wellCategory", headerName: "Well type", width: 90,
        },
        {
            field: "drillingDays", width: 110,
        },
        {
            field: "wellCost", headerName: "Cost", width: 90,
        },
    ])

    const CreateWell = async () => {
        console.log("Wells in createWell: ", wells)
        const newWell = new Well()
        newWell.wellCategory = !explorationWells ? 0 : 4
        newWell.name = "New well"
        newWell.projectId = project.projectId
        const newWells = [...wells, newWell]
        setWells(newWells)
    }

    return (
        <>
            <div
                style={{
                    display: "flex", flexDirection: "column", width: "45%",
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
                    onGridReady={onGridReady}
                />
            </div>
            <Button onClick={CreateWell} variant="outlined">
                {explorationWells
                    ? "Add new exploration well type" : "Add new development/drilling well type"}
            </Button>
        </>
    )
}

export default WellListEditTechnicalInputNew
