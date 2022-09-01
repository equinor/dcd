import { useMemo } from "react"
import "react-datasheet/lib/react-datasheet.css"
import "./style.css"
import { AgGridReact } from "ag-grid-react"
import { useAgGridStyles } from "@equinor/fusion-react-ag-grid-addons"
import { ITimeSeries } from "../../models/ITimeSeries"

export interface CellValue {
    value: number | string
    readOnly?: boolean
}
interface Props {
    columns: string[]
    gridData: any[]
    onCellsChanged: any
    dG4Year: string
    timeSeriesArray: ITimeSeries[] | undefined
}

function DataTable({
    columns, gridData, onCellsChanged, dG4Year, timeSeriesArray,
}: Props) {
    // const [rowData, setRowData] = useState<any[]>(rowDataToColumns())
    // const [columnDefs, setColumnDefs] = useState<ColDef[]>()

    useAgGridStyles()

    const rowDataToColumns = () => {
        const col = columns
        const objKey:any = []
        const objVal: any = []
        const objValSum:any = []
        const combinedObjArr: any = []

        // create a name array props in Timeseries
        // iterate over names for rows in a for loop in dataTable

        // or add name to model to retrieve

        const rowPinned = { Profile: "Temp name", Unit: "MNOK" }

        const rowTotalCost:any = []

        for (let j = 0; j < gridData.length; j += 1) {
            const totalCost:any = []
            if (gridData[j] !== undefined) {
                for (let i = 0; i < col.length; i += 1) {
                    if (gridData[j][0]) {
                        objKey.push(`${col[i]}`)
                        objVal.push(`${gridData[j][0].map((v:any) => v.value)[i]}`)
                    }
                    console.log(`${gridData[j]}`)
                }
                objValSum.push(gridData[j][0]?.map((v:any) => v.value).reduce((x:any, y:any) => x + y))
                console.log(objValSum)
                totalCost.push(objValSum[j])
                console.log(totalCost)
            }
            // eslint-disable-next-line max-len
            const rowObj = objKey.reduce((obj:any, element:any, index:any) => ({ ...obj, [element]: objVal[index] }), {})
            combinedObjArr.push(rowObj)

            const totalCostObj = { "Total cost": totalCost }
            rowTotalCost.push({ ...combinedObjArr[j], ...totalCostObj, ...rowPinned })
            console.log(rowTotalCost)
            console.log(combinedObjArr)
        }
        // const rowWithPins = combinedObjArr.concat([...rowPinned])
        return rowTotalCost
    }

    const columnsArrayToColDef = () => {
        const col = columns
        const columnToColDef = []
        const columnPinned = [
            { field: "Profile", pinned: "left" },
            { field: "Unit", pinned: "left" },
            { field: "Total cost", pinned: "right" }]
        for (let i = 0; i < col.length; i += 1) {
            columnToColDef.push({ field: col[i] })
        }
        const columnWithProfile = columnToColDef.concat([...columnPinned])
        return columnWithProfile
    }

    const defaultColDef = useMemo(() => ({
        resizable: true,
        sortable: true,
        initialWidth: 120,
    }), [])

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
