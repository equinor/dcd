/* eslint-disable camelcase */
import {
    useMemo,
    useState,
    useEffect,
} from "react"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import {
    CellKeyDownEvent,
    ColDef,
    GetContextMenuItemsParams,
    MenuItemDef,
    CellClickedEvent,
} from "@ag-grid-community/core"

import { formatColumnSum, tableCellisEditable } from "@/Utils/common"
import { EMPTY_GUID } from "@/Utils/constants"
import { useAppStore } from "@/Store/AppStore"
import { ITimeSeriesTableDataWithSet } from "@/Models/ITimeSeries"
import profileAndUnitInSameCell from "./CellRenderers/ProfileAndUnitCellRenderer"
import { gridRefArrayToAlignedGrid } from "@/Components/AgGrid/AgGridHelperFunctions"
import SidesheetWrapper from "@/Components/Tables/TableSidesheet/SidesheetWrapper"
import { createLogger } from "@/Utils/logger"

interface Props {
    allTimeSeriesData: any[]
    dg4Year: number
    tableYears: [number, number]
    alignedGridsRef?: any[]
    gridRef?: any
    includeFooter: boolean
    totalRowName?: string
}

const logger = createLogger({ name: "CaseTabTableWithGrouping", enabled: false })

const CaseTabTableWithGrouping = ({
    allTimeSeriesData,
    dg4Year,
    tableYears,
    alignedGridsRef,
    gridRef,
    includeFooter,
    totalRowName,
}: Props) => {
    const styles = useStyles()
    const [rowData, setRowData] = useState<any[]>([{ name: "as" }])
    const { editMode, setShowRevisionReminder } = useAppStore()
    const [isSidesheetOpen, setIsSidesheetOpen] = useState(false)
    const [selectedRow, setSelectedRow] = useState<any>(null)

    const profilesToRowData = () => {
        const tableRows: ITimeSeriesTableDataWithSet[] = []
        const timeSeriesData = allTimeSeriesData?.flat()
        timeSeriesData?.forEach((ts: ITimeSeriesTableDataWithSet) => {
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
                    rowObject.total = Math.round(Number(ts.total) * 100) / 100
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

    const handleCopy = (gridEvent: CellKeyDownEvent) => {
        const keyboardEvent = gridEvent.event as KeyboardEvent
        if ((keyboardEvent.ctrlKey || keyboardEvent.metaKey) && keyboardEvent.key === "c") {
            setShowRevisionReminder(true)
        }
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
    }, [allTimeSeriesData])

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        editable: true,
        suppressHeaderMenuButton: true,
        enableCellChangeFlash: true,
    }), [])

    const getContextMenuItems = (params: GetContextMenuItemsParams): (MenuItemDef | string)[] => {
        const defaultItems = params.defaultItems || []

        return defaultItems.map((item) => {
            if (item === "copy") {
                return {
                    name: "Copy",
                    action: () => {
                        params.api.copySelectedRangeToClipboard()
                        setShowRevisionReminder(true)
                    },
                } as MenuItemDef
            }
            return item
        })
    }

    const defaultExcelExportParams = useMemo(() => {
        const yearColumnKeys = Array.from({ length: tableYears[1] - tableYears[0] + 1 }, (_, i) => (tableYears[0] + i).toString())
        const columnKeys = ["profileName", "unit", ...yearColumnKeys, "total"]
        return {
            columnKeys,
            fileName: "export.xlsx",
        }
    }, [tableYears])

    const handleCellClicked = (event: CellClickedEvent) => {
        if (!event.data || editMode) return // Don't open sidesheet in edit mode
        
        // Get the clicked column's field (year)
        const clickedYear = event.column.getColId()
        
        logger.info("Cell clicked", {
            isGroup: event.data.group,
            rowData: event.data,
            profileName: event.data.profileName,
            values: event.data.profile?.values,
            clickedYear
        })

        setSelectedRow({
            ...event.data,
            clickedYear // Add the clicked year to the row data
        })
        setIsSidesheetOpen(true)
    }

    return (
        <>
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
                        rowSelection={{
                            mode: "multiRow",
                            copySelectedRows: true,
                            checkboxes: false,
                            headerCheckbox: false,
                            enableClickSelection: true,
                        }}
                        cellSelection
                        suppressMovableColumns
                        suppressAggFuncInHeader
                        enableCharts
                        alignedGrids={alignedGridsRef ? gridRefArrayToAlignedGrid(alignedGridsRef) : undefined}
                        grandTotalRow={includeFooter ? "bottom" : undefined}
                        getRowStyle={getRowStyle}
                        suppressLastEmptyLineOnPaste
                        groupDefaultExpanded={groupDefaultExpanded}
                        stopEditingWhenCellsLoseFocus
                        defaultExcelExportParams={defaultExcelExportParams}
                        getContextMenuItems={getContextMenuItems}
                        onCellKeyDown={handleCopy}
                        onCellClicked={handleCellClicked}
                    />
                </div>
            </div>
            <SidesheetWrapper
                isOpen={isSidesheetOpen}
                onClose={() => setIsSidesheetOpen(false)}
                rowData={selectedRow}
                dg4Year={dg4Year}
                allTimeSeriesData={allTimeSeriesData}
            />
        </>
    )
}

export default CaseTabTableWithGrouping
