import { useMemo } from "react"
import "react-datasheet/lib/react-datasheet.css"
import "./style.css"
import { AgGridReact } from "ag-grid-react"
import { useAgGridStyles } from "@equinor/fusion-react-ag-grid-addons"

export interface CellValue {
    value: number | string
    readOnly?: boolean
}
interface Props {
    columns: string[]
    gridData: any
    onCellsChanged: any
    dG4Year: string
}

function DataTableOld({
    columns, gridData, onCellsChanged, dG4Year,
}: Props) {
    // const [rowData, setRowData] = useState<any[]>(rowDataToColumns())
    // const [columnDefs, setColumnDefs] = useState<ColDef[]>()

    useAgGridStyles()

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
        const rowObj = objKey.reduce((obj:any, element:any, index:any) => ({ ...obj, [element]: objVal[index] }), {})
        return [rowObj]
    }

    const columnsArrayToColDef = () => {
        const col = columns
        const columnToColDef = []
        const columnPinned = { field: "Profile", initialPinned: "left" }
        for (let i = 0; i < col.length; i += 1) {
            columnToColDef.push({ field: col[i] })
        }
        const columnWithProfile = columnToColDef.concat([...[columnPinned]])
        return columnWithProfile
    }

    const defaultColDef = useMemo(() => ({
        resizable: true,
        sortable: true,
        initialWidth: 120,
    }), [])

    return (
        <div className="ag-theme-alpine" style={{ height: 100 }}>
            <AgGridReact
                rowData={rowDataToColumns()}
                columnDefs={columnsArrayToColDef()}
                defaultColDef={defaultColDef}
                animateRows
            />
        </div>
    )
}

export default DataTableOld
