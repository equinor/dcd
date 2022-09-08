import {
    Dispatch,
    SetStateAction,
    useCallback, useEffect, useMemo, useRef, useState,
} from "react"
import "react-datasheet/lib/react-datasheet.css"
import "./style.css"
import { AgGridReact } from "ag-grid-react"
import { useAgGridStyles } from "@equinor/fusion-react-ag-grid-addons"
import { CellValueChangedEvent } from "ag-grid-community"
import { objKeys } from "@microsoft/applicationinsights-core-js"
import { ITimeSeries } from "../../models/ITimeSeries"
import { buildGridData } from "./helpers"

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
    profileName: string[]
    profileEnum: number
    setHasChanges: Dispatch<SetStateAction<boolean>>,
    setTimeSeries: Dispatch<SetStateAction<ITimeSeries | undefined>>,
    timeSeries: ITimeSeries[] | undefined
}

function DataTable({
    // eslint-disable-next-line max-len
    columns, gridData, onCellsChanged, dG4Year, timeSeriesArray, profileName, profileEnum, setHasChanges, setTimeSeries, timeSeries,
}: Props) {
    const [rowData, setRowData] = useState<any>([{ 2026: 1, 2027: 5 }])
    // const gridRef = useRef(HTMLDivElement)
    // const [columnDefs, setColumnDefs] = useState<ColDef[]>()
    const gridRef = useRef<AgGridReact | null>(null)

    const combinedTimeseries:any = []

    useAgGridStyles()

    enum CurrencyEnum {
        "MUSD" = 0,
        "MNOK" = 1
    }

    enum GSM3Enum {
        "GSm³/yr" = 0,
        "Bscf/yr" = 1
    }

    enum MSM3Enum {
        "MSm³/yr" = 0,
        "mill bbls/yr" = 1
    }

    const setUnit = (j:number) => {
        if (["CO2 emissions", "Production profile NGL"].includes(profileName[j])) {
            return "MTPA"
        }
        if (["Net sales gas", "Fuel flaring and losses", "Production profile gas"].includes(profileName[j])) {
            return GSM3Enum[profileEnum]
        }
        if (["Production profile oil", "Production profile water",
            "Production profile water injection"].includes(profileName[j])) {
            return MSM3Enum[profileEnum]
        }
        return CurrencyEnum[profileEnum]
    }

    // console.log(gridData)

    const rowDataToColumns = () => {
        const col = columns
        const objKey:any = []
        const objVal: any = []
        const objValSum:any = []
        const combinedObjArr: any = []

        // create a name array props in Timeseries
        // iterate over names for rows in a for loop in dataTable

        // or add name to model to retrieve

        // pass currency as a prop
        // then one can retrieve the actual currency enum from each view
        // but units will be worse as production profiles have different units...

        // const rowPinned = { Profile: "Temp name", Unit: "MNOK 2022" }

        const rowTotalCost:any = []

        // CHECK IF griddata only has 0 values? after griddata gets 0 values,
        // it uses griddata.length >= 1
        if (gridData.length === 0) {
            for (let j = 0; j < profileName.length; j += 1) {
                // console.log(setUnit(j))
                const rowPinned = { Profile: profileName[j], Unit: setUnit(j) }
                // if (gridData[j] !== undefined) {
                //     for (let i = 0; i < col.length; i += 1) {
                //         if (gridData[j][0]) {
                //             objKey.push(`${col[i]}`)
                //             objVal.push(`${0}`)
                //         }
                //         // console.log(`${gridData[j]}`)
                //     }
                //     // objValSum.push(gridData[j][0]?.map((v:any) => v.value).reduce((x:any, y:any) => x + y))
                //     // // console.log(objValSum)
                //     // totalCost.push(objValSum[j])
                //     // console.log(totalCost)
                // }
                // eslint-disable-next-line max-len
                const rowObj = objKey.reduce((obj:any, element:any, index:any) => ({ ...obj, [element]: objVal[index] }), {})
                combinedObjArr.push(rowObj)

                const totalCostObj = { "Total cost": 0 }
                rowTotalCost.push({ ...combinedObjArr[j], ...totalCostObj, ...rowPinned })
                // console.log(rowTotalCost)
                // console.log(combinedObjArr)
            }
        }

        if (gridData.length >= 1) {
            for (let j = 0; j < gridData.length; j += 1) {
                // console.log(setUnit(j))
                const rowPinned = { Profile: profileName[j], Unit: setUnit(j) }
                const totalCost:any = []
                if (gridData[j] !== undefined) {
                    for (let i = 0; i < col.length; i += 1) {
                        if (gridData[j][0]) {
                            objKey.push(`${col[i]}`)
                            objVal.push(`${gridData[j][0].map((v:any) => v.value)[i]}`)
                        }
                        // console.log(`${gridData[j]}`)
                    }
                    objValSum.push(gridData[j][0]?.map((v:any) => v.value).reduce((x:any, y:any) => x + y))
                    // console.log(objValSum)
                    totalCost.push(objValSum[j])
                    // console.log(totalCost)
                }
                // eslint-disable-next-line max-len
                const rowObj = objKey.reduce((obj:any, element:any, index:any) => ({ ...obj, [element]: objVal[index] }), {})
                combinedObjArr.push(rowObj)

                const totalCostObj = { "Total cost": totalCost }
                rowTotalCost.push({ ...combinedObjArr[j], ...totalCostObj, ...rowPinned })
                // console.log(rowTotalCost)
                // console.log(combinedObjArr)
            }
        }
        // const rowWithPins = combinedObjArr.concat([...rowPinned])
        // console.log(rowTotalCost)
        return rowTotalCost
    }

    // const [rowData, setRowData] = useState(rowDataToColumns())

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
        editable: true,
        flex: 1,
    }), [])

    // const buildEditedGrid = (updatedTimeSeries: ITimeSeries) => {
    //     // if (timeSeries![0] !== undefined && updatedTimeSeries !== undefined) {
    //     //     setGridData(combinedTimeseries)
    //     // }
    //     setGridData(combinedTimeseries)
    // }

    useEffect(() => {
        // buildEditedGrid(combinedTimeseries!)

        console.log(gridData)
        console.log(timeSeries)
        console.log(profileName)
        console.log(timeSeries?.length)
    }, [timeSeries, profileName, gridData])

    const onCellValueChanged = useCallback((event: CellValueChangedEvent) => {
        // reverse engineer row to gridData
        // set profile start year to first year
        // send entire row with 0s
        // setGridData

        console.log(gridData)
        console.log(timeSeries)

        const rowEventData = event.data
        const index = event.node.rowIndex

        console.log(event.data.name)
        console.log(event.node.rowIndex)

        if (timeSeries! !== undefined) {
            // console.log(timeSeries![i])
            // eslint-disable-next-line max-len
            const convertObj = { convertObj: (delete rowEventData.Unit, delete rowEventData.Profile, delete rowEventData["Total cost"]), rowEventData }
            // console.log(convertObj)
            // eslint-disable-next-line max-len
            const changeKeysToValue = Object.keys(rowEventData).reduce((prev:any, curr:any, index:any) => ({ ...prev, [(index)]: Number(rowEventData[curr]) }), {})
            // console.log(Object.values(changeKeysToValue))

            const newTimeSeries: ITimeSeries = { ...timeSeries![index!] } // find rowNumber
            // eslint-disable-next-line max-len
            newTimeSeries.startYear = Number(Object.keys(rowEventData)[0]) - Number(Object.keys(rowEventData).slice(-1)[0])
            newTimeSeries.name = profileName![index!] // need to add profileName!!!
            newTimeSeries.values = Object.values(changeKeysToValue)
            setTimeSeries(newTimeSeries)
            const newGridData = buildGridData(newTimeSeries)
            // console.log(newTimeSeries)
            combinedTimeseries.push(newGridData)
            // combinedTimeseries.push(newTimeSeries)
        }
        // buildEditedGrid(combinedTimeseries)

        // console.log(combinedTimeseries)
        setHasChanges(true)
    }, [])

    return (
        <div className="ag-theme-alpine" style={{ height: 500 }}>
            <AgGridReact
                ref={gridRef}
                // onGridReady={onGridReady}
                rowData={rowDataToColumns()}
                columnDefs={columnsArrayToColDef()}
                defaultColDef={defaultColDef}
                animateRows
                enableCellChangeFlash
                onCellValueChanged={onCellValueChanged}
            />
        </div>
    )
}

export default DataTable
