import React, { useEffect, useMemo, useState } from "react"
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
    // const [rowData, setRowData] = useState([
    //     { rowDataToColumns() },
    // ])

    useAgGridStyles()

    // const rowData = [
    //     { 2025: 1, 2026: 1, 2027: 1 },
    // ]

    const rowDataToColumns = () => {
        const col = columns
        const objKey:any = []
        const objVal: any = []
        for (let i = 0; i < col.length; i += 1) {
            if (gridData[0][i]) {
                objKey.push(`${col[i]}`)
                objVal.push(`${gridData[0].map((v:any) => v.value)[i]}`)
            }
        }
        const obj1 = objKey.reduce((obj:any, element:any, index:any) => ({ ...obj, [element]: objVal[index] }), {})
        console.log(obj1)
        return [obj1]
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
