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

export interface CellValue {
    value: number | string
    readOnly?: boolean
}
interface Props {
    columns: string[]
    dG4Year: string
    wellsTimeSeries: (ITimeSeries | undefined)[]
}

function DataTableWell({
    columns,
    dG4Year,
    wellsTimeSeries,
}: Props) {
    const topGrid = useRef<AgGridReact>(null)
    const bottomGrid = useRef<AgGridReact>(null)

    useAgGridStyles()

    const lockIcon = (params: any) => {
        if (params.data.Profile === "Total cost") {
            return ""
        }
        return <Icon data={lock} color="#007079" />
    }

    const setEmptyWellsIfNoData = (value: object[]) => {
        if ((columns.length === 0 && wellsTimeSeries.length !== 0) || columns[0] === "") {
            for (let j = 0; j < wellsTimeSeries.length; j += 1) {
                if (wellsTimeSeries[j]?.values?.length === 0) {
                    const readOnly = {
                        "Exploration wells": "", Unit: "Well", "Total wells": 0, ReadOnly: true,
                    }
                    value.push({ ...readOnly })
                }
            }
        }
    }

    const rowDataToColumns = () => {
        const value: object[] = []

        setEmptyWellsIfNoData(value)
        return value
    }

    const expWellsColDef = () => {
        if (columns.length !== 0) {
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
                },
                {
                    headerName: "",
                    width: 60,
                    field: "ReadOnly",
                    pinned: "right",
                    aggFunc: "",
                    cellStyle: { fontWeight: "normal" },
                    editable: false,
                    hide: wellsTimeSeries.length === 0,
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
                },
                {
                    headerName: "",
                    width: 60,
                    field: "ReadOnly",
                    pinned: "right",
                    aggFunc: "",
                    cellStyle: { fontWeight: "normal" },
                    editable: false,
                    hide: wellsTimeSeries.length === 0,
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
        editable: true,
        flex: 1,
    }), [])

    useEffect(() => {
    }, [wellsTimeSeries, dG4Year])

    const columnTotalsDataExpWells = () => {
        const footerGridData = {
            "Exploration wells": "Total wells",
            Unit: "Wells",
        }
        const totalValueArray: number[] = []
        const valueArray: number[][] = []
        if (wellsTimeSeries.length >= 1 && columns.length > 1) {
            for (let i = 0; i < columns.length; i += 1) {
                if (wellsTimeSeries[i] !== undefined) {
                    const zeroesAtStart: number[] = Array.from({
                        length: Number(wellsTimeSeries[i]?.startYear)
                            + Number(dG4Year) - Number(columns[0]),
                    }, (() => 0))

                    const zeroesAtEnd: number[] = Array.from({
                        length: Number(columns.slice(-1)[0]) + 1
                            - (Number(wellsTimeSeries[i]?.startYear)
                                + Number(dG4Year)
                                + Number(wellsTimeSeries[i]?.values?.length)),
                    }, (() => 0))

                    const alignedAssetGridData: number[] = zeroesAtStart
                        .concat(wellsTimeSeries[i]?.values!, zeroesAtEnd)
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
        if (wellsTimeSeries.length >= 1 && columns.length !== 0) {
            for (let j = 0; j < wellsTimeSeries.length; j += 1) {
                if (wellsTimeSeries[j] !== undefined && wellsTimeSeries[j]?.values?.length !== 0) {
                    totalTotalCostArray.push((wellsTimeSeries[j]?.values ?? [])
                        .reduce((x: number, y: number) => x + y))
                }
            }
        }
        const sum = totalTotalCostArray.reduce((prev, curr) => prev + curr, 0)
        const totalTotalObj = { Total: Number(sum) }
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
        if (wellsTimeSeries.length >= 1 && columns.length > 1) {
            for (let i = 0; i < columns.length; i += 1) {
                if (wellsTimeSeries[i] !== undefined) {
                    const zeroesAtStart: number[] = Array.from({
                        length: Number(wellsTimeSeries[i]?.startYear)
                            + Number(dG4Year) - Number(columns[0]),
                    }, (() => 0))

                    const zeroesAtEnd: number[] = Array.from({
                        length: Number(columns.slice(-1)[0]) + 1
                            - (Number(wellsTimeSeries[i]?.startYear)
                                + Number(dG4Year)
                                + Number(wellsTimeSeries[i]?.values?.length)),
                    }, (() => 0))

                    const alignedAssetGridData: number[] = zeroesAtStart
                        .concat(wellsTimeSeries[i]?.values!, zeroesAtEnd)
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
        if (wellsTimeSeries.length >= 1 && columns.length !== 0) {
            for (let j = 0; j < wellsTimeSeries.length; j += 1) {
                if (wellsTimeSeries[j] !== undefined && wellsTimeSeries[j]?.values?.length !== 0) {
                    totalTotalCostArray.push((wellsTimeSeries[j]?.values ?? [])
                        .reduce((x: number, y: number) => x + y))
                }
            }
        }
        const sum = totalTotalCostArray.reduce((prev, curr) => prev + curr, 0)
        const totalTotalObj = { Total: Number(sum) }
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
                        rowData={rowDataToColumns()}
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
                        rowData={rowDataToColumns()}
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
