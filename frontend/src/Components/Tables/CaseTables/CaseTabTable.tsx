import {
    useMemo,
    useState,
    useEffect,
    useCallback,
    memo,
    useRef,
} from "react"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { useParams } from "react-router"
import {
    ColDef,
    GridReadyEvent,
    ICellRendererParams,
    CellClickedEvent,
} from "@ag-grid-community/core"
import isEqual from "lodash/isEqual"
import { CircularProgress } from "@equinor/eds-core-react"
import styled from "styled-components"

import {
    tableCellisEditable,
    numberValueParser,
    getCaseRowStyle,
    formatColumnSum,
    validateTableCellChange,
    generateTableCellEdit,
    ITableCellChangeParams,
    ITableCellChangeConfig,
    validateInput,
} from "@/Utils/common"
import { useAppStore } from "@/Store/AppStore"
import { useProjectContext } from "@/Store/ProjectContext"
import profileAndUnitInSameCell from "./CellRenderers/ProfileAndUnitCellRenderer"
import ErrorCellRenderer from "./CellRenderers/ErrorCellRenderer"
import CalculationSourceToggle from "./CalculationToggle/CalculationSourceToggle"
import {
    ITimeSeriesTableDataOverrideWithSet,
    ITimeSeriesTableDataWithSet,
} from "@/Models/ITimeSeries"
import { gridRefArrayToAlignedGrid, profilesToRowData } from "@/Components/AgGrid/AgGridHelperFunctions"
import { createLogger } from "@/Utils/logger"
import SidesheetWrapper from "../TableSidesheet/SidesheetWrapper"
import useEditCase from "@/Hooks/useEditCase"

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
}

const CenterGridIcons = styled.div`
    padding-top: 0px;
    padding-left: 0px;
    height: 100%;
    display: flex;
    align-items: center;
    gap: 8px;
`

const logger = createLogger({
    name: "CaseTabTable",
    enabled: false, // Set to true to enable debug logging. dont leave this on for production
})

const CaseTabTable = memo(({
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
}: Props) => {
    const { editMode, setSnackBarMessage, isSaving } = useAppStore()
    const styles = useStyles()
    const { caseId, tab } = useParams()
    const { projectId } = useProjectContext()

    const [editQueue, setEditQueue] = useState<any[]>([])
    const [presentedTableData, setPresentedTableData] = useState<ITimeSeriesTableDataWithSet[]>([])
    const previousTimeSeriesDataRef = useRef(timeSeriesData)
    const gridInitializedRef = useRef(false)

    const [selectedRow, setSelectedRow] = useState<any>(null)
    const [isSidesheetOpen, setIsSidesheetOpen] = useState(false)
    const [lastEditTime, setLastEditTime] = useState<number>(Date.now())

    const { addEdit } = useEditCase()

    const gridRowData = useMemo(
        () => {
            if (!presentedTableData?.length) { return [] }
            return profilesToRowData(presentedTableData, dg4Year, tableName, editMode)
        },
        [presentedTableData, editMode, dg4Year, tableName],
    )

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

    useEffect(() => {
        if (!isEqual(previousTimeSeriesDataRef.current, timeSeriesData)) {
            previousTimeSeriesDataRef.current = timeSeriesData
        }
    }, [timeSeriesData])

    const submitEditQueue = useCallback(() => {
        logger.info("submitting edit queue", { editQueue })
        if (isSaving) {
            return
        }

        if (editQueue.length === 0) {
            return
        }

        const submittedEdits = []
        editQueue.forEach((edit) => {
            logger.info("Submitting edit", { edit })
            const submitted = addEdit(edit)
            if (!submitted) {
                logger.error("Failed to submit edit", { edit })
            } else {
                logger.info("Submitted edit", { edit })
                submittedEdits.push(edit)
            }
        })
        if (submittedEdits.length === editQueue.length) {
            setEditQueue([])
        }
    }, [editQueue, isSaving])

    useEffect(() => {
        if (editQueue.length > 0) {
            const timer = setTimeout(() => {
                const timeSinceLastEdit = Date.now() - lastEditTime
                if (timeSinceLastEdit >= 3000) {
                    logger.info("Auto-submitting edit queue after 5 seconds of inactivity")
                    if (gridRef.current.api.getEditingCells().length > 0) {
                        gridRef.current.api.stopEditing()
                    } else {
                        submitEditQueue()
                    }
                }
            }, 3000)
            return () => clearTimeout(timer)
        }
        return undefined
    }, [editQueue, lastEditTime, submitEditQueue])

    const handleCellValueChange = useCallback((event: any) => {
        const params: ITableCellChangeParams = {
            data: event.data,
            newValue: event.newValue,
            oldValue: event.oldValue,
            profileName: event.data.profileName,
            profile: event.data.profile,
            resourceId: event.data.resourceId,
        }
        logger.info("handling cell value change, where the cell is", event.data.profileName, event.data.resourceName)
        logger.info("the new value is", event.newValue)
        logger.info("the old value is", event.oldValue)

        const config: ITableCellChangeConfig = {
            dg4Year,
            caseId,
            projectId,
            tab,
            tableName,
            timeSeriesData: presentedTableData,
            setSnackBarMessage,
        }

        if (!validateTableCellChange(params, config)) {
            return
        }

        const edit = generateTableCellEdit(params, config)
        if (edit) {
            logger.info("Processing edit", { edit })
            setLastEditTime(Date.now())
            setEditQueue((prev) => [...prev, edit])
        }
    }, [presentedTableData, dg4Year, caseId, projectId, tab, tableName, setSnackBarMessage])

    useEffect(() => {
        if (gridRef.current?.api && presentedTableData?.length > 0 && gridRowData.length > 0) {
            const currentNodes = gridRef.current.api.getRenderedNodes()
            const currentRowData = currentNodes.map((node: { data: any }) => node.data)
            if (!isEqual(currentRowData, gridRowData)) {
                gridRef.current.api.setGridOption("rowData", gridRowData)
            }
        }
    }, [gridRowData, presentedTableData])

    const lockIconRenderer = (params: ICellRendererParams<ITimeSeriesTableDataOverrideWithSet>) => {
        if (!params.data) {
            return null
        }

        const isUnlocked = params.data.overrideProfile?.override

        if (
            !isUnlocked
            && calculatedFields
            && calculatedFields.includes(params.data.resourceName)
            && ongoingCalculation) {
            return <CircularProgress size={24} />
        }

        return (
            <CenterGridIcons>
                <CalculationSourceToggle
                    addEdit={addEdit}
                    editMode={editMode}
                    isProsp={isProsp}
                    sharepointFileId={sharepointFileId}
                    clickedElement={params}

                />
            </CenterGridIcons>
        )
    }

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
                aggFunc: formatColumnSum,
                cellStyle: { fontWeight: "bold", textAlign: "right" },
            },
            {
                headerName: "",
                width: 70,
                field: "set",
                pinned: "right",
                aggFunc: "",
                editable: false,
                cellRenderer: lockIconRenderer,
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
                cellRenderer: ErrorCellRenderer,
                cellRendererParams: (params: any) => ({
                    value: params.value,
                    errorMsg: !params.node.footer && validateInput(params, editMode),
                }),
                cellStyle: {
                    padding: "0px",
                    textAlign: "right",
                },
                cellClass: (params: any) => (editMode && tableCellisEditable(params, editMode) ? "editableCell" : undefined),
                valueParser: (params: any) => numberValueParser(setSnackBarMessage, params),
            })
        }
        return columnPinned.concat([...yearDefs])
    }, [tableYears, editMode, gridRowData, tableName, totalRowName])

    const [columnDefs, setColumnDefs] = useState<ColDef[]>([])

    useEffect(() => {
        const newColDefs = generateTableYearColDefs()
        setColumnDefs(newColDefs)
    }, [generateTableYearColDefs])

    const initializeGridWithData = useCallback((gridReadyEvent: GridReadyEvent) => {
        gridInitializedRef.current = true
        if (gridRowData.length > 0) {
            gridReadyEvent.api.setGridOption("rowData", gridRowData)
        }
    }, [gridRowData])

    // Handle grid blur event
    useEffect(() => {
        const containerRef = document.getElementById(tableName)?.parentElement

        const handleClickOutside = (event: MouseEvent) => {
            if (gridRef.current?.api) {
                gridRef.current.api.clearFocusedCell()
                gridRef.current.api.stopEditing()
                gridRef.current.api.deselectAll()
            }

            if (containerRef && !containerRef.contains(event.target as Node) && editQueue.length > 0) {
                submitEditQueue()
            }
        }

        document.addEventListener("mousedown", handleClickOutside)
        return () => {
            document.removeEventListener("mousedown", handleClickOutside)
        }
    }, [tableName, editQueue, submitEditQueue])

    const defaultExcelExportParams = useMemo(() => {
        const yearColumnKeys = Array.from({ length: tableYears[1] - tableYears[0] + 1 }, (_, i) => (tableYears[0] + i).toString())
        const columnKeys = ["profileName", "unit", ...yearColumnKeys, "total"]
        return {
            columnKeys,
            fileName: "export.xlsx",
        }
    }, [tableYears])

    const handleCellClicked = (event: CellClickedEvent) => {
        if (!event.data || editMode) { return } // Don't open sidesheet in edit mode

        // Get the clicked column's field (year)
        const clickedYear = event.column.getColId()

        logger.info("Cell clicked", {
            rowData: event.data,
            profileName: event.data.profileName,
            values: event.data.profile?.values,
            clickedYear,
        })

        setSelectedRow({
            ...event.data,
            clickedYear, // Add the clicked year to the row data
        })
        setIsSidesheetOpen(true)
    }

    const gridConfig = useMemo(() => ({
        // Column configuration
        defaultColDef: {
            sortable: true,
            filter: true,
            resizable: true,
            editable: (params: any) => tableCellisEditable(params, editMode),
            onCellValueChanged: handleCellValueChange,
            suppressHeaderMenuButton: true,
            enableCellChangeFlash: editMode,
            suppressMovable: true,
        },
        // Grid configuration
        rowData: gridRowData,
        columnDefs,
        animateRows: true,
        domLayout: "autoHeight" as const,
        alignedGrids: alignedGridsRef ? gridRefArrayToAlignedGrid(alignedGridsRef) : undefined,
        grandTotalRow: includeFooter ? ("bottom" as const) : undefined,
        getRowStyle: getCaseRowStyle,
        suppressLastEmptyLineOnPaste: true,
        onGridReady: initializeGridWithData,
        defaultExcelExportParams,
        cellSelection: true,
        copyHeadersToClipboard: false,
        stopEditingWhenCellsLoseFocus: true,
        onCellClicked: handleCellClicked,
        rowSelection: "single" as const,
        context: {
            setSelectedRow,
            setIsSidesheetOpen,
        },
    }), [
        editMode,
        handleCellValueChange,
        gridRowData,
        columnDefs,
        alignedGridsRef,
        includeFooter,
        initializeGridWithData,
        defaultExcelExportParams,
        handleCellClicked,
        setSelectedRow,
        setIsSidesheetOpen,
    ])

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
                        {...gridConfig}
                    />
                </div>
            </div>
            <SidesheetWrapper
                isOpen={isSidesheetOpen}
                onClose={() => setIsSidesheetOpen(false)}
                rowData={selectedRow}
                dg4Year={dg4Year}
                allTimeSeriesData={timeSeriesData}
                isProsp={isProsp}
            />
        </>
    )
})

export default CaseTabTable
