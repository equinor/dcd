import {
    Dispatch,
    SetStateAction,
    useCallback, useEffect, useMemo, useRef,
} from "react"
import "react-datasheet/lib/react-datasheet.css"
import "./style.css"
import { AgGridReact } from "ag-grid-react"
import { useAgGridStyles } from "@equinor/fusion-react-ag-grid-addons"
import { CellValueChangedEvent, ColDef } from "ag-grid-community"
import { ITimeSeries } from "../../models/ITimeSeries"
import { buildGridData } from "./helpers"
import "ag-grid-enterprise"

export interface CellValue {
    value: number | string
    readOnly?: boolean
}
interface Props {
    columns: string[]
    gridData: any[]
    dG4Year: string
    profileName: string[]
    profileEnum: number
    setHasChanges: Dispatch<SetStateAction<boolean>>,
    setTimeSeries: Dispatch<SetStateAction<ITimeSeries | undefined>>,
    timeSeries: (ITimeSeries | undefined)[]
    profileType: string
}

function DataTable({
    columns, gridData, dG4Year, profileName, profileEnum, setHasChanges, setTimeSeries, timeSeries, profileType,
}: Props) {
    const topGrid = useRef<AgGridReact>(null)
    const bottomGrid = useRef<AgGridReact>(null)

    const combinedTimeseries: any = []

    useAgGridStyles()

    enum CurrencyEnum {
        "MNOK" = 1,
        "MUSD" = 2
    }

    enum GSM3Enum {
        "GSm³/yr" = 0,
        "Bscf/yr" = 1
    }

    enum MSM3Enum {
        "MSm³/yr" = 0,
        "mill bbls/yr" = 1
    }

    const setUnit = (j: number) => {
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
        const objKey: any = []
        const objVal: any = []
        const objValSum: any = []
        const combinedObjArr: any = []

        const value: any = []

        if (gridData.length === 0) {
            for (let j = 0; j < profileName.length; j += 1) {
                const rowPinned = { Profile: profileName[j], Unit: setUnit(j) }
                const rowObj = objKey
                    .reduce((obj: any, element: any, index: any) => ({ ...obj, [element]: objVal[index] }), {})
                combinedObjArr.push(rowObj)

                const totalValueObj = { Total: 0 }
                value.push({ ...combinedObjArr[j], ...totalValueObj, ...rowPinned })
            }
        }

        if (gridData.length >= 1 && col.length !== 0) {
            for (let j = 0; j < gridData.length; j += 1) {
                const rowPinned = { Profile: profileName[j], Unit: setUnit(j) }
                const totalValue: any = []
                if (gridData[j] !== undefined) {
                    for (let i = 0; i < col.length; i += 1) {
                        if (gridData[j][0]) {
                            objKey.push(`${col[i]}`)
                            objVal.push(`${gridData[j][0].map((v: any) => v.value)[i]}`)
                        }
                    }
                    objValSum.push(gridData[j][0]?.map((v: any) => v.value).reduce((x: any, y: any) => x + y))
                    totalValue.push(objValSum[j])
                }
                const objValToNumbers = objVal.map((x: any) => parseFloat(x))
                const rowObj = objKey
                    .reduce((obj: any, element: any, index: any) => ({ ...obj, [element]: objValToNumbers[index] }), {})
                combinedObjArr.push(rowObj)

                const totalValueObj = { Total: Number(totalValue) }
                value.push({ ...combinedObjArr[j], ...totalValueObj, ...rowPinned })
            }
        }
        return value
    }

    const columnsArrayToColDef = () => {
        if (columns.length !== 0) {
            const col = columns
            const columnToColDef = []
            const columnPinned = [
                {
                    field: "Profile", pinned: "left", width: "autoWidth", aggFunc: "",
                },
                {
                    field: "Unit", pinned: "left", width: "autoWidth", aggFunc: "",
                },
                {
                    field: "Total", pinned: "right", aggFunc: "sum", cellStyle: { fontWeight: "bold" },
                }]
            for (let i = 0; i < col.length; i += 1) {
                columnToColDef.push({ field: col[i], aggFunc: "sum" })
            }
            const columnWithProfile = columnToColDef.concat([...columnPinned])
            return columnWithProfile
        }
        return undefined
    }

    const defaultColDef = useMemo<ColDef>(() => ({
        resizable: true,
        sortable: true,
        editable: true,
        flex: 1,
    }), [])

    const footerColDef = useMemo<ColDef>(() => ({
        resizable: true,
        sortable: true,
        editable: false,
        flex: 1,
    }), [])

    useEffect(() => {
    }, [timeSeries, profileName, gridData, dG4Year])

    const onCellValueChanged = useCallback((event: CellValueChangedEvent) => {
        const rowEventData = event.data
        const index = event.node.rowIndex

        if (timeSeries! !== undefined) {
            const convertObj = {
                convertObj:
                    (delete rowEventData.Unit, delete rowEventData.Profile,
                    delete rowEventData.Total),
                rowEventData,
            }
            const changeKeysToValue = Object.keys(rowEventData)
                .reduce((prev: any, curr: any, ind: any) => ({ ...prev, [(ind)]: Number(rowEventData[curr]) }), {})
            const newTimeSeries: ITimeSeries = { ...timeSeries![index!] }
            newTimeSeries.startYear = (Number(Object.keys(rowEventData)[0]) - Number(dG4Year!))
            newTimeSeries.name = profileName![index!]
            newTimeSeries.values = Object.values(changeKeysToValue)
            setTimeSeries(newTimeSeries)
            const newGridData = buildGridData(newTimeSeries)
            combinedTimeseries.push(newGridData)
        }
        setHasChanges(true)
    }, [dG4Year])

    const columnTotalsData = () => {
        const footerGridData = {
            Profile: "Total cost",
            Unit: setUnit(0),
        }
        const totalValueArray: any = []
        const valueArray = []
        if (timeSeries !== undefined && timeSeries.length >= 1 && columns.length !== 0) {
            for (let i = 0; i < columns.length; i += 1) {
                if (timeSeries[i] !== undefined) {
                    valueArray.push(timeSeries[i]?.values)
                }
            }
            for (let k = 0; k < columns.length; k += 1) {
                if (timeSeries !== undefined) {
                    totalValueArray.push(valueArray.reduce((prev, curr) => prev + curr![k], 0))
                }
            }
        }
        const value = columns
            .reduce((obj: any, element: any, index: any) => ({ ...obj, [element]: totalValueArray[index] }), {})

        const totalTotalCostArray = []
        if (timeSeries !== undefined && timeSeries.length >= 1 && columns.length !== 0) {
            for (let j = 0; j < timeSeries.length; j += 1) {
                if (timeSeries[j] !== undefined && gridData[j] !== undefined) {
                    totalTotalCostArray.push(gridData[j][0]?.map((v: any) => v.value).reduce((x: any, y: any) => x + y))
                }
            }
        }
        const sum = totalTotalCostArray.reduce((prev, curr) => prev + curr, 0)
        const totalTotalObj = { Total: Number(sum) }
        const combinedFooterRow = [{ ...value, ...footerGridData, ...totalTotalObj }]
        return combinedFooterRow
    }

    return (
        <div
            style={{ display: "flex", flexDirection: "column", height: 150 }}
            className="ag-theme-alpine"
        >
            <div style={{ flex: "1 1 auto" }}>
                <AgGridReact
                    ref={topGrid}
                    alignedGrids={bottomGrid.current ? [bottomGrid.current] : undefined}
                    rowData={rowDataToColumns()}
                    columnDefs={columnsArrayToColDef()}
                    defaultColDef={defaultColDef}
                    animateRows
                    domLayout="autoHeight"
                    enableCellChangeFlash
                    onCellValueChanged={onCellValueChanged}
                    rowSelection="multiple"
                    enableRangeSelection
                    suppressCopySingleCellRanges
                    suppressMovableColumns
                    suppressHorizontalScroll
                />
            </div>
            { profileType === "Cost"
                        && (
                            <div style={{ flex: "none", height: "60px" }}>
                                <AgGridReact
                                    ref={bottomGrid}
                                    alignedGrids={topGrid.current ? [topGrid.current] : undefined}
                                    rowData={columnTotalsData()}
                                    defaultColDef={footerColDef}
                                    columnDefs={columnsArrayToColDef()}
                                    headerHeight={0}
                                    rowStyle={{ fontWeight: "bold" }}
                                />
                            </div>
                        )}
        </div>
    )
}

export default DataTable
