import {
    useEffect, useMemo, useRef,
} from "react"
import "react-datasheet/lib/react-datasheet.css"
import "./style.css"
import { AgGridReact } from "ag-grid-react"
import { useAgGridStyles } from "@equinor/fusion-react-ag-grid-addons"
import { ColDef } from "ag-grid-community"
import { Icon } from "@equinor/eds-core-react"
import { lock } from "@equinor/eds-icons"
import { ITimeSeries } from "../../models/ITimeSeries"
import "ag-grid-enterprise"
import { DrillingSchedule } from "../../models/assets/wellproject/DrillingSchedule"

export interface CellValue {
    value: number | string
    readOnly?: boolean
}
interface Props {
    columns: string[]
    dG4Year: string
    timeSeriesData: any[]
    expDrillingSchedules: (DrillingSchedule | undefined)[]
    devDrillingSchedules: (DrillingSchedule | undefined)[]
}

function DataTableWell({
    columns,
    dG4Year,
    timeSeriesData,
    expDrillingSchedules,
    devDrillingSchedules,
}: Props) {
    const topGrid = useRef<AgGridReact>(null)
    const bottomGrid = useRef<AgGridReact>(null)

    useAgGridStyles()

    const generateExpTimeSeriesYears = (index: number, dg4: string) => {
        const years = []
        if (dg4) {
            const profileStartYear: number = Number(expDrillingSchedules[index]?.startYear) + Number(dG4Year)
            const maxYear: number = Number(expDrillingSchedules[index]?.values?.length) + profileStartYear
            for (let i = profileStartYear; i < maxYear; i += 1) {
                years.push(i.toString())
            }
        }
        return years
    }

    const generateDevTimeSeriesYears = (index: number, dg4: string) => {
        const years = []
        if (dg4) {
            const profileStartYear: number = Number(devDrillingSchedules[index]?.startYear) + Number(dG4Year)
            const maxYear: number = Number(devDrillingSchedules[index]?.values?.length) + profileStartYear
            for (let i = profileStartYear; i < maxYear; i += 1) {
                years.push(i.toString())
            }
        }
        return years
    }

    const rowDataToColumnsExpWell = () => {
        const combinedObjArr: object[] = []
        const objValSum: number[] = []
        const value: object[] = []
        if (timeSeriesData[0] !== undefined) {
            for (let i = 0; i < timeSeriesData.length; i += 1) {
                const totalValue: number[] = []
                if (expDrillingSchedules[i] !== undefined && dG4Year && expDrillingSchedules[i]?.values?.length !== 0) {
                    objValSum.push((expDrillingSchedules[i]?.values?.map(
                        (v) => Math.round((v + Number.EPSILON) * 10) / 10,
                    ) ?? [])
                        .reduce((x: number, y: number) => x + y))
                    totalValue.push(objValSum[i])
                }
                if (expDrillingSchedules[i] !== undefined && dG4Year && expDrillingSchedules[i]?.values?.length === 0) {
                    objValSum.push(0)
                    totalValue.push(objValSum[i])
                }

                const objValToNumbers: number[] = expDrillingSchedules[i]?.values!
                const rowObj = generateExpTimeSeriesYears(i, dG4Year)
                    .reduce((obj: object, element: string, index: number) => (
                        { ...obj, [element]: objValToNumbers[index] }), {})
                combinedObjArr.push(rowObj)
                const totalValueObj = { "Total wells": Number(totalValue) }
                value.push({ ...combinedObjArr[i], ...timeSeriesData[0][i], ...totalValueObj })
            }
        }
        return value
    }

    const rowDataToColumnsDevWell = () => {
        const combinedObjArr: object[] = []
        const objValSum: number[] = []
        const value: object[] = []
        if (timeSeriesData[1] !== undefined) {
            for (let i = 0; i < timeSeriesData.length; i += 1) {
                const totalValue: number[] = []
                if (devDrillingSchedules[i] !== undefined && dG4Year && devDrillingSchedules[i]?.values?.length !== 0) {
                    objValSum.push((devDrillingSchedules[i]?.values?.map(
                        (v) => Math.round((v + Number.EPSILON) * 10) / 10,
                    ) ?? [])
                        .reduce((x: number, y: number) => x + y))
                    totalValue.push(objValSum[i])
                }
                if (devDrillingSchedules[i] !== undefined && dG4Year && devDrillingSchedules[i]?.values?.length === 0) {
                    objValSum.push(0)
                    totalValue.push(objValSum[i])
                }

                const objValToNumbers: number[] = devDrillingSchedules[i]?.values!
                const rowObj = generateDevTimeSeriesYears(i, dG4Year)
                    .reduce((obj: object, element: string, index: number) => (
                        { ...obj, [element]: objValToNumbers[index] }), {})
                combinedObjArr.push(rowObj)
                const totalValueObj = { "Total wells": Number(totalValue) }
                value.push({ ...combinedObjArr[i], ...timeSeriesData[1][i], ...totalValueObj })
            }
        }
        return value
    }

    const expWellsColDef = () => {
        if (columns.length !== 0) {
            console.log(columns)
            const col = columns
            const columnToColDef = []
            const columnPinned = [
                {
                    field: "Exploration wells",
                    pinned: "left",
                    width: "autoWidth",
                    aggFunc: "",
                },
                {
                    field: "Unit", pinned: "left", width: "120", aggFunc: "",
                },
                {
                    field: "Total wells",
                    pinned: "right",
                    aggFunc: "sum",
                    cellStyle: { fontWeight: "bold" },
                    width: 100,
                }]
            for (let i = 0; i < col.length; i += 1) {
                columnToColDef.push({ field: col[i], aggFunc: "sum" })
            }
            const columnWithProfile = columnToColDef.concat([...columnPinned])
            return columnWithProfile
        }
        return undefined
    }

    const devWellsColDef = () => {
        if (columns.length !== 0) {
            const col = columns
            const columnToColDef = []
            const columnPinned = [
                {
                    field: "Development wells",
                    pinned: "left",
                    width: "autoWidth",
                    aggFunc: "",
                },
                {
                    field: "Unit", pinned: "left", width: "120", aggFunc: "",
                },
                {
                    field: "Total wells",
                    pinned: "right",
                    aggFunc: "sum",
                    cellStyle: { fontWeight: "bold" },
                    width: 100,
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

    useEffect(() => {
    }, [dG4Year, columns, expDrillingSchedules, devDrillingSchedules, timeSeriesData])

    const columnTotalsDataExpWells = () => {
        const footerGridData = {
            "Exploration wells": "Total wells",
            Unit: "Wells",
        }
        const totalValueArray: number[] = []
        const valueArray: number[][] = []
        if (expDrillingSchedules.length >= 1 && columns.length > 1) {
            for (let i = 0; i < columns.length; i += 1) {
                if (expDrillingSchedules[i] !== undefined) {
                    const zeroesAtStart: number[] = Array.from({
                        length: Number(expDrillingSchedules[i]?.startYear)
                            + Number(dG4Year) - Number(columns[0]),
                    }, (() => 0))

                    const zeroesAtEnd: number[] = Array.from({
                        length: Number(columns.slice(-1)[0]) + 1
                            - (Number(expDrillingSchedules[i]?.startYear)
                                + Number(dG4Year)
                                + Number(expDrillingSchedules[i]?.values?.length)),
                    }, (() => 0))

                    const alignedAssetGridData: number[] = zeroesAtStart
                        .concat(expDrillingSchedules[i]?.values!, zeroesAtEnd)
                    valueArray.push(alignedAssetGridData)
                }
            }
            for (let k = 0; k < columns.length; k += 1) {
                totalValueArray.push(valueArray.reduce((prev, curr) => prev + curr[k], 0))
            }
        }
        const value = columns
            .reduce((obj: object, element: string, index: number) => (
                { ...obj, [element]: totalValueArray[index] }), {})
        const totalTotalCostArray = []
        if (expDrillingSchedules.length >= 1 && columns.length !== 0) {
            for (let j = 0; j < expDrillingSchedules.length; j += 1) {
                if (expDrillingSchedules[j] !== undefined && expDrillingSchedules[j]?.values?.length !== 0) {
                    totalTotalCostArray.push((expDrillingSchedules[j]?.values ?? [])
                        .reduce((x: number, y: number) => x + y))
                }
            }
        }
        const sum = totalTotalCostArray.reduce((prev, curr) => prev + curr, 0)
        const totalTotalObj = { "Total wells": Number(sum) }
        const combinedFooterRow = [{ ...value, ...footerGridData, ...totalTotalObj }]
        return combinedFooterRow
    }

    const columnTotalsDataDevWells = () => {
        const footerGridData = {
            "Development wells": "Total wells",
            Unit: "Wells",
        }
        const totalValueArray: number[] = []
        const valueArray: number[][] = []
        if (devDrillingSchedules.length >= 1 && columns.length > 1) {
            for (let i = 0; i < columns.length; i += 1) {
                if (devDrillingSchedules[i] !== undefined) {
                    const zeroesAtStart: number[] = Array.from({
                        length: Number(devDrillingSchedules[i]?.startYear)
                            + Number(dG4Year) - Number(columns[0]),
                    }, (() => 0))

                    const zeroesAtEnd: number[] = Array.from({
                        length: Number(columns.slice(-1)[0]) + 1
                            - (Number(devDrillingSchedules[i]?.startYear)
                                + Number(dG4Year)
                                + Number(devDrillingSchedules[i]?.values?.length)),
                    }, (() => 0))

                    const alignedAssetGridData: number[] = zeroesAtStart
                        .concat(devDrillingSchedules[i]?.values!, zeroesAtEnd)
                    valueArray.push(alignedAssetGridData)
                }
            }
            for (let k = 0; k < columns.length; k += 1) {
                totalValueArray.push(valueArray.reduce((prev, curr) => prev + curr[k], 0))
            }
        }
        const value = columns
            .reduce((obj: object, element: string, index: number) => (
                { ...obj, [element]: totalValueArray[index] }), {})
        const totalTotalCostArray = []
        if (devDrillingSchedules.length >= 1 && columns.length !== 0) {
            for (let j = 0; j < devDrillingSchedules.length; j += 1) {
                if (devDrillingSchedules[j] !== undefined && devDrillingSchedules[j]?.values?.length !== 0) {
                    totalTotalCostArray.push((devDrillingSchedules[j]?.values ?? [])
                        .reduce((x: number, y: number) => x + y))
                }
            }
        }
        const sum = totalTotalCostArray.reduce((prev, curr) => prev + curr, 0)
        const totalTotalObj = { "Total wells": Number(sum) }
        const combinedFooterRow = [{ ...value, ...footerGridData, ...totalTotalObj }]
        return combinedFooterRow
    }

    return (
        <>
            <div
                style={{
                    display: "flex", flexDirection: "column", height: 150, marginBottom: 200, marginTop: 40,
                }}
                className="ag-theme-alpine"
            >
                <div style={{ flex: "1 1 auto" }}>
                    <AgGridReact
                        ref={topGrid}
                        alignedGrids={bottomGrid.current ? [bottomGrid.current] : undefined}
                        rowData={rowDataToColumnsExpWell()}
                        columnDefs={expWellsColDef()}
                        defaultColDef={defaultColDef}
                        animateRows
                        domLayout="autoHeight"
                        enableCellChangeFlash
                        rowSelection="multiple"
                        enableRangeSelection
                        suppressCopySingleCellRanges
                        suppressMovableColumns
                        enableCharts
                    />
                </div>
                <div style={{ flex: "none", height: "60px" }}>
                    <AgGridReact
                        ref={bottomGrid}
                        alignedGrids={topGrid.current ? [topGrid.current] : undefined}
                        rowData={columnTotalsDataExpWells()}
                        defaultColDef={defaultColDef}
                        columnDefs={expWellsColDef()}
                        headerHeight={0}
                        rowStyle={{ fontWeight: "bold" }}
                    />
                </div>
            </div>
            <div
                style={{
                    display: "flex", flexDirection: "column", height: 150, marginBottom: 100,
                }}
                className="ag-theme-alpine"
            >
                <div style={{ flex: "1 1 auto" }}>
                    <AgGridReact
                        ref={topGrid}
                        alignedGrids={bottomGrid.current ? [bottomGrid.current] : undefined}
                        rowData={rowDataToColumnsDevWell()}
                        columnDefs={devWellsColDef()}
                        defaultColDef={defaultColDef}
                        animateRows
                        domLayout="autoHeight"
                        enableCellChangeFlash
                        rowSelection="multiple"
                        enableRangeSelection
                        suppressCopySingleCellRanges
                        suppressMovableColumns
                        enableCharts
                    />
                </div>
                <div style={{ flex: "none", height: "60px" }}>
                    <AgGridReact
                        ref={bottomGrid}
                        alignedGrids={topGrid.current ? [topGrid.current] : undefined}
                        rowData={columnTotalsDataDevWells()}
                        defaultColDef={defaultColDef}
                        columnDefs={devWellsColDef()}
                        headerHeight={0}
                        rowStyle={{ fontWeight: "bold" }}
                    />
                </div>
            </div>

        </>
    )
}

export default DataTableWell
