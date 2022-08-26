import React, { useEffect, useMemo, useState } from "react"
import ReactDataSheet from "react-datasheet"
import "react-datasheet/lib/react-datasheet.css"
import "./style.css"
import { AgGridReact } from "ag-grid-react"
import { useAgGridStyles } from "@equinor/fusion-react-ag-grid-addons"
import { objCreate } from "@microsoft/applicationinsights-core-js"
import { EMPTY_GUID } from "../../Utils/constants"

export interface CellValue {
    value: number | string
    readOnly?: boolean
}

class Table extends ReactDataSheet<CellValue> {}

interface Props {
    columns: string[]
    gridData: any
    onCellsChanged: any
    dG4Year: string
}

/* eslint-disable react/no-unstable-nested-components */
function DataTable({
    columns, gridData, onCellsChanged, dG4Year,
}: Props) {
    // const [rowData, setRowData] = useState([
    //     { rowDataToColumns() },
    // ])

    useAgGridStyles()

    // const rowData = [
    //     { 2025: 1, 2026: 1, 2027: 1 },
    // ]

    const rowDataToColumns = () => {
        const col = columns
        const gridDataToRowData = []
        for (let i = 0; i < col.length; i += 1) {
            gridDataToRowData.push(`${col[i]}`)
        }
        console.log(gridData)
        const obj = gridDataToRowData.reduce((acc, item) => ({ ...acc, [item]: 1 }), {})
        // setRowData(obj)
        console.log([obj])
        return [obj]
    }

    const defaultColDef = useMemo(() => ({
        resizable: true,
        sortable: true,
    }), [])

    const columnsArrayToColDef = () => {
        const col = columns
        const columnToColDef = []
        for (let i = 0; i < col.length; i += 1) {
            columnToColDef.push({ field: col[i] })
        }
        console.log(columnToColDef)
        return columnToColDef
    }

    console.log(gridData)

    return (
        <div className="ag-theme-alpine" style={{ height: 200 }}>
            <AgGridReact
                rowData={rowDataToColumns()}
                columnDefs={columnsArrayToColDef()}
                defaultColDef={defaultColDef}
                animateRows
            />
        </div>
    )
}

export default DataTable
