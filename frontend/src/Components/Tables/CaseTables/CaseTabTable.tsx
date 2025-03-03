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
import SidesheetWrapper from "../TableSidesheet/SidesheetWrapper"
import { useTableQueue } from "@/Hooks/useTableQueue"
import useCanUserEdit from "@/Hooks/useCanUserEdit"

// Styled Components
const CenterGridIcons = styled.div`
    padding-top: 0px;
    padding-left: 0px;
    height: 100%;
    display: flex;
    align-items: center;
    gap: 8px;
`

// Component Props Interface
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
    // Hooks and Context
    const {
        setIsSaving, editMode, setSnackBarMessage, isSaving,
    } = useAppStore()
    const styles = useStyles()
    const { caseId, tab } = useParams()
    const { projectId } = useProjectContext()
    const { canEdit, isEditDisabled } = useCanUserEdit()

    // State Management
    const [presentedTableData, setPresentedTableData] = useState<ITimeSeriesTableDataWithSet[]>([])
    const [selectedRow, setSelectedRow] = useState<any>(null)
    const [isSidesheetOpen, setIsSidesheetOpen] = useState(false)
    const [columnDefs, setColumnDefs] = useState<ColDef[]>([])

    // Custom Hooks
    const { editQueue, addToQueue, submitEditQueue } = useTableQueue({
        isSaving,
        gridRef,
        setIsSaving,
    })

    // Refs
    const previousTimeSeriesDataRef = useRef(timeSeriesData)
    const gridInitializedRef = useRef(false)

    // Memoized Data
    const gridRowData = useMemo(
        () => {
            if (!presentedTableData?.length) { return [] }
            return profilesToRowData(presentedTableData, dg4Year, tableName, canEdit())
        },
        [presentedTableData, editMode, dg4Year, tableName, isEditDisabled],
    )

    // Cell Renderers
    const lockIconRenderer = (params: ICellRendererParams<ITimeSeriesTableDataOverrideWithSet>) => {
        if (!params.data) { return null }

        const isUnlocked = params.data.overrideProfile?.override

        if (!isUnlocked && calculatedFields?.includes(params.data.resourceName) && ongoingCalculation) {
            return <CircularProgress size={24} />
        }

        return (
            <CenterGridIcons>
                <CalculationSourceToggle
                    editAllowed={canEdit()}
                    isProsp={isProsp}
                    sharepointFileId={sharepointFileId}
                    clickedElement={params}
                />
            </CenterGridIcons>
        )
    }

    // Column Definitions
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
                editable: (params: any) => tableCellisEditable(params, canEdit()),
                minWidth: 100,
                aggFunc: formatColumnSum,
                cellRenderer: ErrorCellRenderer,
                cellRendererParams: (params: any) => ({
                    value: params.value,
                    errorMsg: !params.node.footer && validateInput(params, canEdit()),
                }),
                cellStyle: {
                    padding: "0px",
                    textAlign: "right",
                },
                cellClass: (params: any) => (tableCellisEditable(params, canEdit()) ? "editableCell" : undefined),
                valueParser: (params: any) => numberValueParser(setSnackBarMessage, params),
            })
        }
        return columnPinned.concat([...yearDefs])
    }, [tableYears, editMode, gridRowData, tableName, totalRowName, isEditDisabled])

    // Event Handlers
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

    // Grid Configuration
    const gridConfig = useMemo(() => ({
        defaultColDef: {
            sortable: true,
            filter: true,
            resizable: true,
            editable: (params: any) => tableCellisEditable(params, canEdit()),
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
    ])

    // Effects
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

    useEffect(() => {
        setColumnDefs(generateTableYearColDefs())
    }, [generateTableYearColDefs])

    useEffect(() => {
        const containerRef = document.getElementById(tableName)?.parentElement

        const handleClickOutside = (event: MouseEvent) => {
            if (containerRef && !containerRef.contains(event.target as Node) && editQueue.length > 0) {
                submitEditQueue()
            }
        }

        document.addEventListener("mousedown", handleClickOutside)
        return () => {
            document.removeEventListener("mousedown", handleClickOutside)
        }
    }, [tableName, editQueue, submitEditQueue])

    // Render
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
