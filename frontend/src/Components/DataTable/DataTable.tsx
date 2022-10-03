/* eslint-disable camelcase */
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
import { Icon } from "@equinor/eds-core-react"
import { lock, lock_off } from "@equinor/eds-icons"
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
    readOnlyTimeSeries: (ITimeSeries | undefined)[]
    readOnlyName: string[]
}

function DataTable({
    columns,
    gridData,
    dG4Year,
    profileName,
    profileEnum,
    setHasChanges,
    setTimeSeries,
    timeSeries,
    profileType,
    readOnlyTimeSeries,
    readOnlyName,
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

    const lockIcon = (params: any) => {
        if (params.data.ReadOnly === true) {
            return <Icon data={lock} color="#007079" />
        }
        if (params.data.Profile === "Total cost") {
            return ""
        }
        return <Icon data={lock_off} color="grey" />
    }

    const generateTimeSeriesYears = (index: number, dg4: string) => {
        const years = []
        if (dg4) {
            const profileStartYear: number = Number(readOnlyTimeSeries[index]?.startYear) + Number(dG4Year)
            const maxYear: number = Number(readOnlyTimeSeries[index]?.values?.length) + profileStartYear

            for (let i = profileStartYear; i < maxYear; i += 1) {
                years.push(i.toString())
            }
        }
        return years
    }

    const rowDataToColumns = () => {
        const col = columns
        const objKey: string[] = []
        const objVal: string[] = []
        const objValSum: number[] = []
        const combinedObjArr: object[] = []
        const readOnlyCombinedObjArr: object[] = []
        const readOnlyObjValSum: number[] = []

        const value: object[] = []

        if (readOnlyName.length >= 1 && readOnlyTimeSeries !== undefined && col.length !== 0 && dG4Year) {
            for (let i = 0; i < readOnlyName.length; i += 1) {
                const totalValue: number[] = []
                const readOnly = {
                    Profile: readOnlyName[i], Unit: setUnit(i), Total: totalValue, ReadOnly: true,
                }
                if (readOnlyTimeSeries[i] !== undefined && dG4Year && readOnlyTimeSeries[i]?.values?.length !== 0) {
                    readOnlyObjValSum.push((readOnlyTimeSeries[i]?.values?.map(
                        (v) => Math.round((v + Number.EPSILON) * 10) / 10,
                    ) ?? [])
                        .reduce((x: number, y: number) => x + y))
                    totalValue.push(readOnlyObjValSum[i])
                }

                const objValToNumbers: number[] = readOnlyTimeSeries[i]?.values?.map(
                    (v) => Math.round((v + Number.EPSILON) * 10) / 10,
                ) ?? []
                const rowObj = generateTimeSeriesYears(i, dG4Year)
                    .reduce((obj: object, element: string, index: number) => (
                        { ...obj, [element]: objValToNumbers[index] }), {})
                readOnlyCombinedObjArr.push(rowObj)
                value.push({ ...readOnlyCombinedObjArr[i], ...readOnly })
            }
        }

        if (gridData.length === 0) {
            for (let j = 0; j < profileName.length; j += 1) {
                const rowPinned = { Profile: profileName[j], Unit: setUnit(j), ReadOnly: false }
                const rowObj = objKey
                    .reduce((obj: object, element: string, index: number) => ({ ...obj, [element]: objVal[index] }), {})
                combinedObjArr.push(rowObj)

                const totalValueObj = { Total: 0 }
                value.push({ ...combinedObjArr[j], ...totalValueObj, ...rowPinned })
            }
        }

        if (gridData.length >= 1 && col.length !== 0) {
            for (let j = 0; j < gridData.length; j += 1) {
                const rowPinned = { Profile: profileName[j], Unit: setUnit(j), ReadOnly: false }
                const totalValue: number[] = []
                if (gridData[j] !== undefined) {
                    for (let i = 0; i < col.length; i += 1) {
                        if (gridData[j][0]) {
                            objKey.push(`${col[i]}`)
                            objVal.push(`${gridData[j][0].map((v: any) => v.value)[i]}`)
                        }
                    }
                    objValSum.push(gridData[j][0]?.map((v: any) => v.value).reduce((x: number, y: number) => x + y))
                    totalValue.push(objValSum[j])
                }
                const objValToNumbers = objVal.map((x: string) => parseFloat(x))
                const rowObj = objKey
                    .reduce((obj: object, element: string, index: number) => (
                        { ...obj, [element]: objValToNumbers[index] }), {})
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
                    field: "Profile",
                    pinned: "left",
                    width: "autoWidth",
                    aggFunc: "",
                    editable: false,
                },
                {
                    field: "Unit", pinned: "left", width: "120", aggFunc: "", editable: false,
                },
                {
                    field: "Total",
                    width: 100,
                    pinned: "right",
                    aggFunc: "sum",
                    cellStyle: { fontWeight: "bold" },
                    editable: false,
                },
                {
                    headerName: "",
                    width: 60,
                    field: "ReadOnly",
                    pinned: "right",
                    aggFunc: "",
                    cellStyle: { fontWeight: "normal" },
                    editable: false,
                    hide: readOnlyTimeSeries.length === 0,
                    cellRenderer: lockIcon,
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
        editable: (params) => (params.data.ReadOnly !== true) === true,
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
        const convertObj = {
            convertObj:
                (delete rowEventData.Unit, delete rowEventData.Profile,
                delete rowEventData.Total, delete rowEventData.ReadOnly),
            rowEventData,
        }
        const changeKeysToValue = Object.keys(rowEventData)
            .reduce((prev: object, curr: string, ind: number) => (
                { ...prev, [(ind)]: Number(rowEventData[curr]) }), {})

        if (readOnlyTimeSeries.length !== 0) {
            const newTimeSeries: ITimeSeries = { ...timeSeries[Number(index! - readOnlyTimeSeries.length)] }
            newTimeSeries.startYear = (Number(Object.keys(rowEventData)[0]) - Number(dG4Year))
            newTimeSeries.name = profileName[Number(index! - readOnlyTimeSeries.length)]
            newTimeSeries.values = Object.values(changeKeysToValue)
            setTimeSeries(newTimeSeries)
            const newGridData = buildGridData(newTimeSeries)
            combinedTimeseries.push(newGridData)
            setHasChanges(true)
        }
        if (readOnlyTimeSeries.length === 0) {
            const newTimeSeries: ITimeSeries = { ...timeSeries[index!] }
            newTimeSeries.startYear = (Number(Object.keys(rowEventData)[0]) - Number(dG4Year))
            newTimeSeries.name = profileName[index!]
            newTimeSeries.values = Object.values(changeKeysToValue)
            setTimeSeries(newTimeSeries)
            const newGridData = buildGridData(newTimeSeries)
            combinedTimeseries.push(newGridData)
            setHasChanges(true)
        }
    }, [dG4Year])

    const columnTotalsData = () => {
        const footerGridData = {
            Profile: "Total cost",
            Unit: setUnit(0),
        }
        const totalValueArray: number[] = []
        const valueArray: number[][] = []
        const readOnlyValueArray: number[][] = []
        const readOnlyTotalValueArray: number[] = []
        if (readOnlyTimeSeries.length >= 1 && columns.length > 1) {
            for (let i = 0; i < columns.length; i += 1) {
                if (readOnlyTimeSeries[i] !== undefined) {
                    const zeroesAtStart: number[] = Array.from({
                        length: Number(readOnlyTimeSeries[i]?.startYear!)
                            + Number(dG4Year) - Number(columns[0]),
                    }, (() => 0))

                    const zeroesAtEnd: number[] = Array.from({
                        length: Number(columns.slice(-1)[0]) + 1
                            - (Number(readOnlyTimeSeries[i]?.startYear!)
                                + Number(dG4Year)
                                + Number(readOnlyTimeSeries[i]?.values!.length!)),
                    }, (() => 0))

                    const alignedAssetGridData: number[] = zeroesAtStart
                        .concat(readOnlyTimeSeries[i]?.values!, zeroesAtEnd)
                    readOnlyValueArray.push(alignedAssetGridData.map((v) => Math.round((v + Number.EPSILON) * 10) / 10))
                }
            }
            for (let k = 0; k < columns.length; k += 1) {
                readOnlyTotalValueArray.push(readOnlyValueArray.reduce((prev, curr) => prev + curr[k], 0))
            }
        }
        if (timeSeries.length >= 1 && columns.length !== 0) {
            for (let i = 0; i < columns.length; i += 1) {
                if (timeSeries[i] !== undefined && timeSeries[i]?.values?.length === columns.length) {
                    valueArray.push(timeSeries[i]?.values ?? [])
                }
                if (timeSeries[i] !== undefined && timeSeries[i]?.values?.length !== columns.length) {
                    const zeroesAtStart: number[] = Array.from({
                        length: Number(timeSeries[i]?.startYear!)
                            + Number(dG4Year) - Number(columns[0]),
                    }, (() => 0))

                    const zeroesAtEnd: number[] = Array.from({
                        length: Number(columns.slice(-1)[0]) + 1
                            - (Number(timeSeries[i]?.startYear!)
                                + Number(dG4Year)
                                + Number(timeSeries[i]?.values!.length!)),
                    }, (() => 0))

                    const alignedAssetGridData: number[] = zeroesAtStart.concat(timeSeries[i]?.values!, zeroesAtEnd)
                    valueArray.push(alignedAssetGridData)
                }
            }
            for (let k = 0; k < columns.length; k += 1) {
                totalValueArray.push(valueArray.reduce((prev, curr) => prev + curr[k], 0))
            }
        }
        const yearTotals = () => {
            if (readOnlyTimeSeries.length >= 1) {
                const mergedTimeSeries = totalValueArray.map((a: number, i: number) => a + readOnlyTotalValueArray[i])
                return mergedTimeSeries
            }
            return totalValueArray
        }
        const value = columns
            .reduce((obj: object, element: string, index: number) => (
                { ...obj, [element]: yearTotals()[index] }), {})
        const totalTotalCostArray = []
        if (readOnlyTimeSeries.length >= 1 && columns.length !== 0) {
            for (let j = 0; j < readOnlyTimeSeries.length; j += 1) {
                if (readOnlyTimeSeries[j] !== undefined && readOnlyTimeSeries[j]?.values?.length !== 0) {
                    totalTotalCostArray.push((readOnlyTimeSeries[j]?.values ?? [])
                        .reduce((x: number, y: number) => x + y))
                }
            }
        }
        if (timeSeries.length >= 1 && columns.length !== 0) {
            for (let j = 0; j < timeSeries.length; j += 1) {
                if (timeSeries[j] !== undefined && gridData[j] !== undefined) {
                    totalTotalCostArray.push(gridData[j][0]?.map((v: any) => v.value).reduce((x: any, y: any) => x + y))
                }
            }
        }
        const sum: number = totalTotalCostArray.map((v) => Math.round(
            (v + Number.EPSILON) * 10,
        ) / 10).reduce((prev, curr) => prev + curr, 0)
        const totalTotalObj = { Total: Number(sum) }
        const combinedFooterRow = [{
            ...value, ...footerGridData, ...totalTotalObj,
        }]
        return combinedFooterRow
    }

    return (
        <div
            style={{
                display: "flex", flexDirection: "column", height: 150, marginBottom: 150,
            }}
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
                    enableCharts
                />
            </div>
            {profileType === "Cost"
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
