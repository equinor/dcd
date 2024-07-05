/* eslint-disable camelcase */
import {
    useMemo,
    useState,
    useEffect,
} from "react"

import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { ColDef } from "@ag-grid-community/core"
import { formatColumnSum, isInteger, tableCellisEditable } from "../../../Utils/common"
import { EMPTY_GUID } from "../../../Utils/constants"
import { useAppContext } from "../../../Context/AppContext"
import profileAndUnitInSameCell from "./ProfileAndUnitInSameCell"

interface Props {
    allTimeSeriesData: any[]
    dg4Year: number
    tableYears: [number, number]
    alignedGridsRef?: any[]
    gridRef?: any
    includeFooter: boolean
    totalRowName?: string
}

const CaseTabTableWithGrouping = ({
    allTimeSeriesData,
    dg4Year,
    tableYears,
    alignedGridsRef, gridRef,
    includeFooter, totalRowName,
}: Props) => {
    const styles = useStyles()
    const [rowData, setRowData] = useState<any[]>([{ name: "as" }])
    const { editMode } = useAppContext()

    const profilesToRowData = () => {
        const tableRows: any[] = []
        const timeSeriesData = allTimeSeriesData?.flat()
        timeSeriesData?.forEach((ts: { overrideProfile?: any; total?: any; overrideProfileSet?: any; set?: any; profile?: any; group?: any; profileName?: any; unit?: any }) => {
            const isOverridden = ts.overrideProfile?.override === true
            const rowObject: any = {}
            const { group, profileName, unit } = ts
            rowObject.group = group
            rowObject.profileName = profileName
            rowObject.unit = unit
            rowObject.total = ts.total ?? 0
            rowObject.set = isOverridden ? ts.overrideProfileSet : ts.set
            rowObject.profile = isOverridden ? ts.overrideProfile : ts.profile
            rowObject.override = ts.overrideProfile?.override === true

            rowObject.overrideProfileSet = ts.overrideProfileSet
            rowObject.overrideProfile = ts.overrideProfile ?? {
                id: EMPTY_GUID, startYear: 0, values: [], override: false,
            }

            if (rowObject.profile && rowObject.profile.values?.length > 0) {
                let j = 0
                for (let i = rowObject.profile.startYear; i < rowObject.profile.startYear + rowObject.profile.values.length; i += 1) {
                    const yearKey = (dg4Year + i).toString()
                    const value = rowObject.profile.values[j]
                    const roundedValue = Math.round((value + Number.EPSILON) * 100) / 100 // Adjust rounding logic as needed
                    rowObject[yearKey] = roundedValue

                    j += 1
                }

                const totalValue = rowObject.profile.values.reduce((acc: any, value: any) => acc + value, 0)
                rowObject.total = Math.round((totalValue + Number.EPSILON) * 100) / 100 // Adjust rounding logic as needed
                if (ts.total !== undefined) {
                    rowObject.total = Math.round(ts.total * 100) / 100
                }
            }
            tableRows.push(rowObject)
        })

        return tableRows
    }

    const getRowStyle = (params: any) => {
        if (params.node.footer) {
            return { fontWeight: "bold" }
        }
        return undefined
    }

    const generateTableYearColDefs = () => {
        const columnPinned: any[] = [
            {
                field: "group",
                headerName: "Summary",
                rowGroup: true,
                pinned: "left",
                hide: true, // Hide this column but use it for grouping
                aggFunc: () => totalRowName ?? "Total",
            },
            {
                field: "profileName",
                headerName: "Field",
                width: 350,
                editable: false,
                cellStyle: { fontWeight: "normal" },
                cellRenderer: (params: any) => (
                    profileAndUnitInSameCell(params, rowData)
                ),
                // pinned: "left",
                // aggFunc: () => totalRowName,
            },
            {
                field: "unit",
                headerName: "Unit",
                hide: true,
                width: 100,
            },
            {
                field: "total",
                flex: 2,
                editable: false,
                pinned: "right",
                width: 100,
                aggFunc: formatColumnSum,
                cellStyle: { fontWeight: "bold", textAlign: "right" },
            },

        ]

        const yearDefs: any[] = []
        for (let index = tableYears[0]; index <= tableYears[1]; index += 1) {
            yearDefs.push({
                field: index.toString(),
                flex: 1,
                editable: (params: any) => tableCellisEditable(params, editMode),
                minWidth: 100,
                aggFunc: formatColumnSum,
                cellClass: (params: any) => (editMode && tableCellisEditable(params, editMode) ? "editableCell" : undefined),
                cellStyle: { fontWeight: "bold", textAlign: "right" },
            })
        }
        return columnPinned.concat([...yearDefs])
    }
    const groupDefaultExpanded = 1

    const [columnDefs, setColumnDefs] = useState<ColDef[]>(generateTableYearColDefs())

    useEffect(() => {
        setRowData(profilesToRowData())
        const newColDefs = generateTableYearColDefs()
        setColumnDefs(newColDefs)
    }, [])

    const handleCellValueChange = (p: any) => {
        const properties = Object.keys(p.data)
        const tableTimeSeriesValues: any[] = []
        properties.forEach((prop) => {
            if (isInteger(prop)
                && p.data[prop] !== ""
                && p.data[prop] !== null
                && !Number.isNaN(Number(p.data[prop].toString().replace(/,/g, ".")))) {
                tableTimeSeriesValues.push({
                    year: parseInt(prop, 10),
                    value: Number(p.data[prop].toString().replace(/,/g, ".")),
                })
            }
        })
        tableTimeSeriesValues.sort((a, b) => a.year - b.year)
        if (tableTimeSeriesValues.length > 0) {
            const tableTimeSeriesFirstYear = tableTimeSeriesValues[0].year
            const tableTimeSerieslastYear = tableTimeSeriesValues.at(-1).year
            const timeSeriesStartYear = tableTimeSeriesFirstYear - dg4Year
            const values: number[] = []
            for (let i = tableTimeSeriesFirstYear; i <= tableTimeSerieslastYear; i += 1) {
                const tableTimeSeriesValue = tableTimeSeriesValues.find((v) => v.year === i)
                if (tableTimeSeriesValue) {
                    values.push(tableTimeSeriesValue.value)
                } else {
                    values.push(0)
                }
            }
            const newProfile = { ...p.data.profile }
            newProfile.startYear = timeSeriesStartYear
            newProfile.values = values
            p.data.set(newProfile)
        }
    }

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        editable: true,
        onCellValueChanged: handleCellValueChange,
        suppressHeaderMenuButton: true,
    }), [])

    const gridRefArrayToAlignedGrid = () => {
        if (alignedGridsRef && alignedGridsRef.length > 0) {
            const refArray: any[] = []
            alignedGridsRef.forEach((agr: any) => {
                if (agr && agr.current) {
                    refArray.push(agr.current)
                }
            })
            if (refArray.length > 0) {
                return refArray
            }
        }
        return undefined
    }

    const defaultExcelExportParams = useMemo(() => {
        const yearColumnKeys = Array.from({ length: tableYears[1] - tableYears[0] + 1 }, (_, i) => (tableYears[0] + i).toString())
        const columnKeys = ["profileName", "unit", ...yearColumnKeys, "total"]
        return {
            columnKeys,
            fileName: "export.xlsx",
        }
    }, [tableYears])

    return (
        <div className={styles.root}>
            <div
                style={{
                    display: "flex", flexDirection: "column", width: "100%",
                }}
                className="ag-theme-alpine-fusion"
            >
                <AgGridReact
                    ref={gridRef}
                    rowData={rowData}
                    columnDefs={columnDefs}
                    defaultColDef={defaultColDef}
                    animateRows
                    domLayout="autoHeight"
                    enableCellChangeFlash
                    rowSelection="multiple"
                    enableRangeSelection
                    suppressCopySingleCellRanges
                    suppressMovableColumns
                    suppressAggFuncInHeader
                    enableCharts
                    alignedGrids={gridRefArrayToAlignedGrid()}
                    groupIncludeTotalFooter={includeFooter}
                    getRowStyle={getRowStyle}
                    suppressLastEmptyLineOnPaste
                    groupDefaultExpanded={groupDefaultExpanded}
                    stopEditingWhenCellsLoseFocus
                    defaultExcelExportParams={defaultExcelExportParams}
                />
            </div>
        </div>
    )
}

export default CaseTabTableWithGrouping
