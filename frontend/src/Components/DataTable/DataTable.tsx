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
}

function DataTable({
    columns, gridData, dG4Year, profileName, profileEnum, setHasChanges, setTimeSeries, timeSeries,
}: Props) {
    const gridRef = useRef<AgGridReact | null>(null)

    const combinedTimeseries: any = []

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
            const columnToColDef2 = []
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
                columnToColDef.push({ field: col[i] })
                columnToColDef2.push({ field: col[i], aggFunc: "sum" })
            }
            //            const columnWithProfile2 = [...columnToColDef2, ...columnPinned]

            const columnWithProfile2 = columnToColDef2.concat([...columnPinned])
            // console.log(columnWithProfile2)

            const columnWithProfile = columnToColDef.concat([...columnPinned])
            // console.log(columnWithProfile)
            return columnWithProfile2
        }
        return undefined
    }

    const defaultColDef = useMemo<ColDef>(() => ({
        resizable: true,
        sortable: true,
        // initialWidth: 120,
        editable: true,
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

    const autoGroupColumnDef = {
        cellRendererParams: {
            footerValueGetter: (params: any) => {
                const isRootLevel = params.node.level === -1
                if (isRootLevel) {
                    return "Total"
                }
                return `Sub Total (${params.value})`
            },
        },
    }

    const footerPins = [
        { Profile: "Total" },
    ]

    const gridOptions = {
        getRowStyle: (params: any) => {
            if (params.node.footer) {
                return { fontWeight: "bold" }
            }
            return { fontWeight: "normal" }
        },
    }

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
                rowSelection="multiple"
                enableRangeSelection
                suppressCopySingleCellRanges
                autoGroupColumnDef={autoGroupColumnDef}
                groupIncludeFooter
                groupIncludeTotalFooter
                gridOptions={gridOptions}
                suppressMovableColumns
            />
        </div>
    )
}

export default DataTable
