import {
    useMemo,
    useState,
    useEffect,
    useCallback,
    memo,
} from "react"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { useParams } from "react-router"
import {
    ColDef,
    GridReadyEvent,
    ICellRendererParams,
} from "@ag-grid-community/core"
import isEqual from "lodash/isEqual"
import { CircularProgress } from "@equinor/eds-core-react"
import styled from "styled-components"

import {
    getValuesFromEntireRow,
    generateProfile,
    tableCellisEditable,
    numberValueParser,
    getCaseRowStyle,
    validateInput,
    formatColumnSum,
    roundToFourDecimalsAndJoin,
} from "@/Utils/common"
import { useAppContext } from "@/Context/AppContext"
import { useProjectContext } from "@/Context/ProjectContext"
import { TABLE_VALIDATION_RULES } from "@/Utils/constants"
import profileAndUnitInSameCell from "./CellRenderers/ProfileAndUnitCellRenderer"
import ErrorCellRenderer from "./CellRenderers/ErrorCellRenderer"
import ClickableLockIcon from "./ClickableLockIcon"
import {
    ITimeSeriesTableDataOverrideWithSet,
    ITimeSeriesTableDataWithSet,
} from "@/Models/ITimeSeries"
import { gridRefArrayToAlignedGrid, profilesToRowData } from "@/Components/AgGrid/AgGridHelperFunctions"
import { createLogger } from "@/Utils/logger"

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
    addEdit: any
    isProsp?: boolean
    sharepointFileId?: string
}

const CenterGridIcons = styled.div`
    padding-top: 0px;
    padding-left: 0px;
    height: 100%;
`

const logger = createLogger({ name: "CaseTabTable", enabled: true })

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
    addEdit,
    isProsp,
    sharepointFileId,
}: Props) => {
    const { editMode, setSnackBarMessage, isSaving } = useAppContext()
    const styles = useStyles()
    const { caseId, tab } = useParams()
    const { projectId } = useProjectContext()
    useEffect(() => {
        if (ongoingCalculation !== undefined) {
            gridRef.current?.api?.redrawRows()
        }
    }, [ongoingCalculation])

    const handleCellValueChange = useCallback((event: any) => {
        const rule = TABLE_VALIDATION_RULES[event.data.profileName]
        if (rule && (event.newValue < rule.min || event.newValue > rule.max)) {
            setSnackBarMessage(`Value must be between ${rule.min} and ${rule.max}. Please correct the input to save the input.`)
            return
        }

        const rowValues = getValuesFromEntireRow(event.data)
        const existingProfile = event.data.profile ? structuredClone(event.data.profile) : {
            startYear: 0,
            values: [],
            id: event.data.resourceId,
        }

        let newProfile
        if (rowValues.length > 0) {
            const firstYear = rowValues[0].year
            const lastYear = rowValues[rowValues.length - 1].year
            const startYear = firstYear - dg4Year
            newProfile = generateProfile(rowValues, event.data.profile, startYear, firstYear, lastYear)
        } else {
            newProfile = structuredClone(existingProfile)
            newProfile.values = []
        }

        if (!caseId || !newProfile) { return }

        const getProfileData = () => timeSeriesData.find(
            (v) => v.profileName === event.data.profileName,
        )

        const profileInTimeSeriesData = getProfileData()

        if (!isEqual(newProfile.values, existingProfile.values)) {
            const edit = {
                newDisplayValue: roundToFourDecimalsAndJoin(newProfile.values),
                previousDisplayValue: roundToFourDecimalsAndJoin(existingProfile.values),
                inputLabel: event.data.profileName,
                projectId,
                resourceName: profileInTimeSeriesData?.resourceName,
                resourcePropertyKey: profileInTimeSeriesData?.resourcePropertyKey,
                caseId,
                resourceId: profileInTimeSeriesData?.resourceId,
                newResourceObject: newProfile,
                previousResourceObject: existingProfile,
                resourceProfileId: profileInTimeSeriesData?.resourceProfileId,
                tabName: tab,
                tableName,
            }

            // Process edit immediately
            logger.info("Processing edit", { edit })
            addEdit(edit)
        }
    }, [timeSeriesData, dg4Year, caseId, projectId, tab, tableName, setSnackBarMessage])

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        editable: (params: any) => {
            if (isSaving) { return false }
            return tableCellisEditable(params, editMode)
        },
        onCellValueChanged: handleCellValueChange,
        suppressHeaderMenuButton: true,
        enableCellChangeFlash: editMode,
        suppressMovable: true,
    }), [editMode, handleCellValueChange, isSaving])

    const gridRowData = useMemo(
        () => {
            if (!timeSeriesData?.length) { return [] }
            const data = profilesToRowData(timeSeriesData, dg4Year, tableName, editMode)
            return data
        },
        [timeSeriesData, editMode, dg4Year, tableName, tableYears],
    )

    useEffect(() => {
        if (gridRef.current?.api && timeSeriesData?.length > 0 && gridRowData.length > 0) {
            const currentNodes = gridRef.current.api.getRenderedNodes()
            const currentRowData = currentNodes.map((node: { data: any }) => node.data)
            if (!isEqual(currentRowData, gridRowData)) {
                gridRef.current.api.setGridOption("rowData", gridRowData)
            }
        }
    }, [gridRowData, timeSeriesData])

    const lockIconRenderer = (params: ICellRendererParams<ITimeSeriesTableDataOverrideWithSet>) => {
        if (!params.data || !editMode) {
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
                <ClickableLockIcon
                    isProsp={isProsp}
                    sharepointFileId={sharepointFileId}
                    clickedElement={params}
                    addEdit={addEdit}
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
                editable: (params: any) => {
                    if (isSaving) { return false }
                    return tableCellisEditable(params, editMode)
                },
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
                cellClass: (params: any) => (editMode && !isSaving && tableCellisEditable(params, editMode) ? "editableCell" : undefined),
                valueParser: (params: any) => numberValueParser(setSnackBarMessage, params),
            })
        }
        return columnPinned.concat([...yearDefs])
    }, [tableYears, editMode, gridRowData, tableName, totalRowName, isSaving])

    const [columnDefs, setColumnDefs] = useState<ColDef[]>([])

    useEffect(() => {
        const newColDefs = generateTableYearColDefs()
        setColumnDefs(newColDefs)
    }, [generateTableYearColDefs])

    const onGridReady = useCallback((gridReadyEvent: GridReadyEvent) => {
        if (timeSeriesData?.length > 0) {
            gridReadyEvent.api.setGridOption("rowData", gridRowData)
        }
    }, [gridRowData, timeSeriesData])

    // Handle grid blur event
    useEffect(() => {
        const gridDiv = document.getElementById(tableName)
        if (!gridDiv) { return undefined }

        const handleBlur = (event: FocusEvent) => {
            // Only log if the focus is moving outside the grid container
            const relatedTarget = event.relatedTarget as HTMLElement
            if (!relatedTarget
                || gridDiv.contains(relatedTarget)
                || (event.target instanceof HTMLInputElement && relatedTarget.classList.contains("ag-cell"))) {
                return
            }

            logger.info("Grid lost focus", {
                tableName,
                event: "focus_lost",
            })
        }

        gridDiv.addEventListener("focusout", handleBlur, true)
        return () => {
            gridDiv.removeEventListener("focusout", handleBlur, true)
        }
    }, [tableName])

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
                id={tableName}
                style={{
                    display: "flex", flexDirection: "column", width: "100%",
                }}
            >
                <AgGridReact
                    ref={gridRef}
                    rowData={gridRowData}
                    columnDefs={columnDefs}
                    defaultColDef={defaultColDef}
                    animateRows
                    domLayout="autoHeight"
                    rowSelection={{
                        mode: "multiRow",
                        copySelectedRows: false,
                        checkboxes: false,
                        headerCheckbox: false,
                        enableClickSelection: true,
                    }}
                    suppressCellFocus
                    enableCellTextSelection={false}
                    suppressClickEdit={false}
                    singleClickEdit
                    stopEditingWhenCellsLoseFocus
                    alignedGrids={alignedGridsRef ? gridRefArrayToAlignedGrid(alignedGridsRef) : undefined}
                    grandTotalRow={includeFooter ? "bottom" : undefined}
                    getRowStyle={getCaseRowStyle}
                    suppressLastEmptyLineOnPaste
                    onGridReady={onGridReady}
                    defaultExcelExportParams={defaultExcelExportParams}
                />
            </div>
        </div>
    )
})

export default CaseTabTable
