import React, { useEffect, useState } from "react"
import ReactDataSheet from "react-datasheet"
import "react-datasheet/lib/react-datasheet.css"
import "./style.css"
import { AgGridReact } from "ag-grid-react"
import { useAgGridStyles } from "@equinor/fusion-react-ag-grid-addons"

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
    useAgGridStyles()

    const rowData = [
        { make: gridData[0].toString() },
    ]

    const columnsArrayToColDef = () => {
        const col = columns
        const columnToColDef = []
        for (let i = 0; i < col.length; i += 1) {
            columnToColDef.push({ field: col[i] })
        }
        return columnToColDef
    }

    return (
        <div className="ag-theme-alpine" style={{ height: 200 }}>
            <AgGridReact
                rowData={rowData}
                columnDefs={columnsArrayToColDef()}
            />
        </div>
    )
}

export default DataTable
