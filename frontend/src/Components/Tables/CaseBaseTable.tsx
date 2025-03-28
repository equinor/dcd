import {
    ColDef,
    GridReadyEvent,
    CellClickedEvent,
} from "@ag-grid-community/core"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import isEqual from "lodash/isEqual"
import {
    useMemo,
    useState,
    useEffect,
    useCallback,
    memo,
    useRef,
} from "react"
import { useParams } from "react-router"
import styled from "styled-components"

import SidesheetWrapper from "../TableSidesheet/SidesheetWrapper"

import ErrorCellRenderer from "./Components/CellRenderers/ErrorCellRenderer"
import OverrideToggleRenderer from "./Components/CellRenderers/OverrideToggleRenderer"
import profileAndUnitInSameCell from "./Components/CellRenderers/ProfileAndUnitCellRenderer"

import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { useEditQueueHandler } from "@/Hooks/useEditQueue"
import { ITimeSeriesTableDataWithSet } from "@/Models/ITimeSeries"
import { useAppStore } from "@/Store/AppStore"
import { useProjectContext } from "@/Store/ProjectContext"
import { roundToDecimals } from "@/Utils/FormatingUtils"
import {
    getCustomContextMenuItems,
    gridRefArrayToAlignedGrid,
    profilesToRowData,
    tableCellisEditable,
    numberValueParser,
    getCaseRowStyle,
    formatColumnSum,
    validateTableCellChange,
    generateTableCellEdit,
    ITableCellChangeParams,
    ITableCellChangeConfig,
    validateInput,
} from "@/Utils/TableUtils"

const CenterGridIcons = styled.div`
    padding-top: 0px;
    padding-left: 0px;
    height: 100%;
    display: flex;
    align-items: center;
    gap: 8px;
`

interface Props {
    timeSeriesData: ITimeSeriesTableDataWithSet[]
    dg4Year: number
    tableYears: [number, number]
    tableName: string
    alignedGridsRef?: any[]
    gridRef?: any
    includeFooter: boolean
    totalRowName?: string
    calculatedFields?: string[]
    ongoingCalculation?: boolean
    isProsp?: boolean
    sharepointFileId?: string
    decimalPrecision: number
}

const createOverrideToggleCellRenderer = (
    calculatedFields?: string[],
    ongoingCalculation?: boolean,
    isProsp?: boolean,
    sharepointFileId?: string,
) => (params: any) => (
    <OverrideToggleRenderer
        params={params}
        calculatedFields={calculatedFields}
        ongoingCalculation={ongoingCalculation}
        isProsp={isProsp}
        sharepointFileId={sharepointFileId}
    />
)

const CaseBaseTable = memo(({
    timeSeriesData,
    dg4Year,
    tableYears,
    tableName,
    alignedGridsRef,
    gridRef,
    includeFooter,
    totalRowName,
    calculatedFields,
    ongoingCalculation,
    isProsp,
    sharepointFileId,
    decimalPrecision,
}: Props) => {
    const { editMode, setSnackBarMessage, isSaving } = useAppStore()
    const styles = useStyles()
    const { caseId, tab } = useParams()
    const { projectId } = useProjectContext()
    const { canEdit, isEditDisabled } = useCanUserEdit()
    const { addToQueue } = useEditQueueHandler({ gridRef })

    const previousTimeSeriesDataRef = useRef(timeSeriesData)
    const gridInitializedRef = useRef(false)

    const [presentedTableData, setPresentedTableData] = useState<ITimeSeriesTableDataWithSet[]>([])
    const [selectedRow, setSelectedRow] = useState<any>(null)
    const [isSidesheetOpen, setIsSidesheetOpen] = useState(false)
    const [columnDefs, setColumnDefs] = useState<ColDef[]>([])

    const gridRowData = useMemo(
        () => {
            if (!presentedTableData?.length) { return [] }

            return profilesToRowData(presentedTableData, dg4Year, canEdit(), decimalPrecision)
        },
        [presentedTableData, editMode, dg4Year, tableName, isEditDisabled, decimalPrecision],
    )

    const generateTableYearColDefs = useCallback(() => {
        const columnPinned: any[] = [
            {
                field: "profileName",
                headerName: tableName,
                cellRenderer: (params: any) => profileAndUnitInSameCell(params, gridRowData),
                width: 250,
                editable: false,
                pinned: "left",
                aggFunc: () => totalRowName ?? "Total",
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
                aggFunc: (params: any) => formatColumnSum(params, decimalPrecision),
                cellStyle: { fontWeight: "bold", textAlign: "right" },
                valueFormatter: (params: any) => {
                    if (!canEdit() && typeof params.value === "number") {
                        return roundToDecimals(params.value, decimalPrecision).toString()
                    }

                    return params.value
                },
            },
            {
                headerName: "",
                width: 70,
                field: "set",
                pinned: "right",
                aggFunc: "",
                editable: false,
                cellRenderer: createOverrideToggleCellRenderer(
                    calculatedFields,
                    ongoingCalculation,
                    isProsp,
                    sharepointFileId,
                ),
            },
        ]

        const yearDefs: any[] = []

        for (let index = tableYears[0]; index <= tableYears[1]; index += 1) {
            yearDefs.push({
                field: index.toString(),
                flex: 1,
                editable: (params: any) => tableCellisEditable(params, canEdit(), isSaving),
                minWidth: 100,
                aggFunc: (params: any) => formatColumnSum(params, decimalPrecision),
                valueFormatter: (params: any) => {
                    if (!canEdit() && typeof params.value === "number") {
                        return roundToDecimals(params.value, decimalPrecision).toString()
                    }

                    return params.value
                },
                cellRenderer: ErrorCellRenderer,
                cellRendererParams: (params: any) => ({
                    errorMsg: !params.node.footer && validateInput(params, canEdit(), isSaving),
                    value: params.value,
                    isEditMode: editMode,
                    precision: decimalPrecision,
                }),
                cellClass: (params: any) => (tableCellisEditable(params, canEdit(), isSaving) ? "editableCell" : undefined),
                valueParser: (params: any) => numberValueParser(setSnackBarMessage, params),
                cellStyle: {
                    padding: "0px",
                    textAlign: "right",
                },
            })
        }

        return columnPinned.concat([...yearDefs])
    }, [tableYears, editMode, gridRowData, tableName, totalRowName, isEditDisabled, isSaving, decimalPrecision])

    const handleCellValueChange = useCallback((event: any) => {
        const params: ITableCellChangeParams = {
            data: event.data,
            newValue: event.newValue,
            oldValue: event.oldValue,
            profileName: event.data.profileName,
            profile: event.data.profile,
            resourceId: event.data.resourceId,
            wellId: event.data.wellId,
        }

        const config: ITableCellChangeConfig = {
            dg4Year,
            caseId,
            projectId,
            tab,
            tableName,
            timeSeriesData: presentedTableData,
            setSnackBarMessage,
        }

        if (!validateTableCellChange(params, config)) { return }

        const edit = generateTableCellEdit(params, config)

        if (edit) {
            addToQueue(edit)
        }
    }, [presentedTableData, dg4Year, caseId, projectId, tab, tableName, setSnackBarMessage, addToQueue])

    const handleCellClicked = (event: CellClickedEvent) => {
        if (!event.data || (canEdit())) { return }

        const clickedYear = event.column.getColId()

        setSelectedRow({
            ...event.data,
            clickedYear,
        })
        setIsSidesheetOpen(true)
    }

    const initializeGridWithData = useCallback((gridReadyEvent: GridReadyEvent) => {
        gridInitializedRef.current = true
        if (gridRowData.length > 0) {
            gridReadyEvent.api.setGridOption("rowData", gridRowData)
        }
    }, [gridRowData])

    const { key, ...gridConfigWithoutKey } = useMemo(() => ({
        defaultColDef: {
            sortable: true,
            filter: true,
            resizable: true,
            editable: (params: any) => tableCellisEditable(params, canEdit(), isSaving),
            onCellValueChanged: handleCellValueChange,
            suppressHeaderMenuButton: true,
            enableCellChangeFlash: canEdit(),
            suppressMovable: true,
        },
        rowData: gridRowData,
        columnDefs,
        animateRows: true,
        domLayout: "autoHeight" as const,
        alignedGrids: alignedGridsRef ? gridRefArrayToAlignedGrid(alignedGridsRef) : undefined,
        grandTotalRow: includeFooter ? ("bottom" as const) : undefined,
        getRowStyle: getCaseRowStyle,
        suppressLastEmptyLineOnPaste: true,
        onGridReady: initializeGridWithData,
        defaultExcelExportParams: {
            columnKeys: ["profileName", "unit", ...Array.from({ length: tableYears[1] - tableYears[0] + 1 }, (_, i) => (tableYears[0] + i).toString()), "total"],
            fileName: "export.xlsx",
        },
        cellSelection: true,
        copyHeadersToClipboard: false,
        stopEditingWhenCellsLoseFocus: true,
        onCellClicked: handleCellClicked,
        context: {
            setSelectedRow,
            setIsSidesheetOpen,
        },
        key: `grid-${editMode ? "edit" : "view"}-${decimalPrecision}`,
    }), [
        editMode,
        handleCellValueChange,
        gridRowData,
        columnDefs,
        alignedGridsRef,
        includeFooter,
        initializeGridWithData,
        handleCellClicked,
        tableYears,
        setSelectedRow,
        setIsSidesheetOpen,
        isEditDisabled,
        isSaving,
        decimalPrecision,
    ])

    useEffect(() => {
        if (timeSeriesData?.length > 0) {
            setPresentedTableData(timeSeriesData)
        }
    }, [timeSeriesData])

    useEffect(() => {
        if (gridRef.current?.api && gridRowData.length > 0) {
            gridRef.current.api.setGridOption("rowData", gridRowData)
        }
    }, [gridRowData])

    // Generate column definitions when tableYears change
    useEffect(() => {
        setColumnDefs(generateTableYearColDefs())
    }, [generateTableYearColDefs, editMode, tableYears])

    // Force redraw when edit mode changes
    useEffect(() => {
        if (gridRef.current?.api) {
            // Force a complete redraw of all rows when switching between edit/view modes
            gridRef.current.api.redrawRows()
            gridRef.current.api.refreshCells({ force: true })
        }
    }, [editMode])

    useEffect(() => {
        if (!isEqual(previousTimeSeriesDataRef.current, timeSeriesData)) {
            previousTimeSeriesDataRef.current = timeSeriesData
        }
    }, [timeSeriesData])

    useEffect(() => {
        setColumnDefs(generateTableYearColDefs())
    }, [generateTableYearColDefs, editMode])

    return (
        <>
            <div className={styles.root}>
                <div
                    style={{
                        display: "flex",
                        flexDirection: "column",
                        width: "100%",
                    }}
                    className="ag-theme-alpine-fusion"
                >
                    <AgGridReact
                        ref={gridRef}
                        key={key}
                        {...gridConfigWithoutKey}
                        getContextMenuItems={getCustomContextMenuItems}
                    />
                </div>
            </div>
            <SidesheetWrapper
                isOpen={isSidesheetOpen}
                onClose={(): void => setIsSidesheetOpen(false)}
                rowData={selectedRow}
                dg4Year={dg4Year}
                allTimeSeriesData={timeSeriesData}
                isProsp={isProsp}
            />
        </>
    )
})

export default CaseBaseTable
