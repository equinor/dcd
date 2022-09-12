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
    profileName: string[]
    profileEnum: number
    setHasChanges: Dispatch<SetStateAction<boolean>>,
    setTimeSeries: Dispatch<SetStateAction<ITimeSeries | undefined>>,
    timeSeries: ITimeSeries[] | undefined
}

function DataTable({
    // eslint-disable-next-line max-len
    columns, gridData, onCellsChanged, dG4Year, profileName, profileEnum, setHasChanges, setTimeSeries, timeSeries,
}: Props) {
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

    const rowDataToColumns = () => {
        const col = columns
        const objKey:any = []
        const objVal: any = []
        const objValSum:any = []
        const combinedObjArr: any = []

        const rowTotalCost:any = []

        if (gridData.length === 0) {
            for (let j = 0; j < profileName.length; j += 1) {
                const rowPinned = { Profile: profileName[j], Unit: setUnit(j) }
                // eslint-disable-next-line max-len
                const rowObj = objKey.reduce((obj:any, element:any, index:any) => ({ ...obj, [element]: objVal[index] }), {})
                combinedObjArr.push(rowObj)

                const totalCostObj = { "Total cost": 0 }
                rowTotalCost.push({ ...combinedObjArr[j], ...totalCostObj, ...rowPinned })
            }
        }

        if (gridData.length >= 1 && col.length !== 0) {
            for (let j = 0; j < gridData.length; j += 1) {
                const rowPinned = { Profile: profileName[j], Unit: setUnit(j) }
                const totalCost:any = []
                if (gridData[j] !== undefined) {
                    for (let i = 0; i < col.length; i += 1) {
                        if (gridData[j][0]) {
                            objKey.push(`${col[i]}`)
                            objVal.push(`${gridData[j][0].map((v:any) => v.value)[i]}`)
                        }
                    }
                    objValSum.push(gridData[j][0]?.map((v:any) => v.value).reduce((x:any, y:any) => x + y))
                    totalCost.push(objValSum[j])
                }
                // eslint-disable-next-line max-len
                const rowObj = objKey.reduce((obj:any, element:any, index:any) => ({ ...obj, [element]: objVal[index] }), {})
                combinedObjArr.push(rowObj)

                const totalCostObj = { "Total cost": totalCost }
                rowTotalCost.push({ ...combinedObjArr[j], ...totalCostObj, ...rowPinned })
            }
        }
        return rowTotalCost
    }

    const columnsArrayToColDef = () => {
        if (columns.length !== 0) {
            const col = columns
            const columnToColDef = []
            const columnPinned = [
                { field: "Profile", pinned: "left", width: "autoWidth" },
                { field: "Unit", pinned: "left", width: "autoWidth" },
                { field: "Total cost", pinned: "right" }]
            for (let i = 0; i < col.length; i += 1) {
                columnToColDef.push({ field: col[i] })
            }
            const columnWithProfile = columnToColDef.concat([...columnPinned])
            return columnWithProfile
        }
        return undefined
    }

    const defaultColDef = useMemo(() => ({
        resizable: true,
        sortable: true,
        // initialWidth: 120,
        editable: true,
        // flex: 1,
    }), [])

    useEffect(() => {
    }, [timeSeries, profileName, gridData, dG4Year])

    const onCellValueChanged = useCallback((event: CellValueChangedEvent) => {
        const rowEventData = event.data
        const index = event.node.rowIndex

        // timeseries[0] doesn't update griddata. total cost is 0 and isnt saved
        if (timeSeries! !== undefined) {
            // eslint-disable-next-line max-len
            const convertObj = { convertObj: (delete rowEventData.Unit, delete rowEventData.Profile, delete rowEventData["Total cost"]), rowEventData }
            // eslint-disable-next-line max-len
            const changeKeysToValue = Object.keys(rowEventData).reduce((prev:any, curr:any, index:any) => ({ ...prev, [(index)]: Number(rowEventData[curr]) }), {})
            const newTimeSeries: ITimeSeries = { ...timeSeries![index!] } // find rowNumber
            // eslint-disable-next-line max-len
            newTimeSeries.startYear = (Number(Object.keys(rowEventData)[0]) - Number(dG4Year!))
            newTimeSeries.name = profileName![index!] // need to add profileName!!!
            newTimeSeries.values = Object.values(changeKeysToValue)
            setTimeSeries(newTimeSeries)
            const newGridData = buildGridData(newTimeSeries)
            combinedTimeseries.push(newGridData)
        }
        setHasChanges(true)
    }, [dG4Year])

    return (
        <div className="ag-theme-alpine">
            <AgGridReact
                ref={gridRef}
                rowData={rowDataToColumns()}
                columnDefs={columnsArrayToColDef()}
                defaultColDef={defaultColDef}
                animateRows
                domLayout="autoHeight"
                enableCellChangeFlash
                onCellValueChanged={onCellValueChanged}
            />
        </div>
    )
}

export default DataTable
