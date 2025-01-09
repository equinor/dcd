import {
    useMemo,
    useState,
    useEffect,
    useCallback,
} from "react"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { useParams } from "react-router"
import {
    ColDef,
    GridReadyEvent,
    ICellRendererParams,
    CellClickedEvent,
    CellDoubleClickedEvent,
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

const CaseTabTable = ({
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
    const { editMode, setSnackBarMessage } = useAppContext()
    const styles = useStyles()
    const { caseId, tab } = useParams()
    const [stagedEdit, setStagedEdit] = useState<any>()
    const { projectId } = useProjectContext()

    useEffect(() => {
        if (stagedEdit) {
            addEdit(stagedEdit)
        }
    }, [stagedEdit])

    useEffect(() => {
        gridRef.current?.api?.redrawRows()
    }, [ongoingCalculation])

    const gridRowData = useMemo(
        () => {
            if (!timeSeriesData?.length) { return [] }
            return profilesToRowData(timeSeriesData, dg4Year, tableName, editMode)
        },
        [timeSeriesData, editMode, dg4Year, tableName, tableYears],
    )

    useEffect(() => {
        if (gridRef.current?.api && timeSeriesData?.length > 0) {
            gridRef.current.api.setGridOption("rowData", gridRowData)
        }
    }, [gridRowData, timeSeriesData])

    const stageEdit = useCallback((tableData: any) => {
        const rowValues = getValuesFromEntireRow(tableData)
        const existingProfile = tableData.profile ? structuredClone(tableData.profile) : {
            startYear: 0,
            values: [],
            id: tableData.resourceId,
        }

        let newProfile
        if (rowValues.length > 0) {
            const firstYear = rowValues[0].year
            const lastYear = rowValues[rowValues.length - 1].year
            const startYear = firstYear - dg4Year
            newProfile = generateProfile(rowValues, tableData.profile, startYear, firstYear, lastYear)
        } else {
            newProfile = structuredClone(existingProfile)
            newProfile.values = []
        }

        if (!caseId || !newProfile) { return }

        const getProfileData = () => timeSeriesData.find(
            (v) => v.profileName === tableData.profileName,
        )

        const profileInTimeSeriesData = getProfileData()

        if (!isEqual(newProfile.values, existingProfile.values)) {
            setStagedEdit({
                newDisplayValue: roundToFourDecimalsAndJoin(newProfile.values),
                previousDisplayValue: roundToFourDecimalsAndJoin(existingProfile.values),
                inputLabel: tableData.profileName,
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
            })
        }
    }, [timeSeriesData, dg4Year, caseId, projectId, tab, tableName])

    const handleCellValueChange = useCallback((event: any) => {
        console.log("[DEBUG] handleCellValueChange called", {
            newValue: event.newValue,
            oldValue: event.oldValue,
            profileName: event.data?.profileName,
            column: event.column?.getColId(),
        })
        const rule = TABLE_VALIDATION_RULES[event.data.profileName]
        if (rule && (event.newValue < rule.min || event.newValue > rule.max)) {
            setSnackBarMessage(`Value must be between ${rule.min} and ${rule.max}. Please correct the input to save the input.`)
            return
        }

        const currentCell = gridRef.current?.api?.getFocusedCell()

        stageEdit(event.data)

        if (currentCell) {
            requestAnimationFrame(() => {
                gridRef.current?.api?.setFocusedCell(currentCell.rowIndex, currentCell.column)
            })
        }
    }, [stageEdit])

    const defaultColDef = useMemo(() => {
        console.log("[DEBUG] defaultColDef updated", { editMode })
        return {
            sortable: true,
            filter: true,
            resizable: true,
            editable: true,
            onCellValueChanged: handleCellValueChange,
            suppressHeaderMenuButton: true,
            enableCellChangeFlash: editMode,
            suppressMovableColumns: true,
            suppressNavigableTimestamp: true,
        }
    }, [editMode, handleCellValueChange])

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
                    const isEditable = tableCellisEditable(params, editMode)
                    console.log("[DEBUG] cell editable check", {
                        year: index,
                        profileName: params.data?.profileName,
                        isEditable,
                        editMode,
                        params,
                    })
                    return isEditable
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

    const onGridReady = useCallback((gridReadyEvent: GridReadyEvent) => {
        if (timeSeriesData?.length > 0) {
            gridReadyEvent.api.setGridOption("rowData", gridRowData)
        }
    }, [gridRowData, timeSeriesData])

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
                    cellSelection
                    suppressMovableColumns
                    enableCharts
                    onCellEditingStarted={(event) => {
                        console.log("[DEBUG] cell editing started", {
                            column: event.column?.getColId(),
                            profileName: event.data?.profileName,
                            editMode,
                            value: event.value,
                        })
                    }}
                    onCellEditingStopped={(event) => {
                        console.log("[DEBUG] cell editing stopped", {
                            column: event.column?.getColId(),
                            profileName: event.data?.profileName,
                            editMode,
                            value: event.value,
                            newValue: event.newValue,
                        })
                    }}
                    singleClickEdit
                    stopEditingWhenCellsLoseFocus
                    alignedGrids={alignedGridsRef ? gridRefArrayToAlignedGrid(alignedGridsRef) : undefined}
                    grandTotalRow={includeFooter ? "bottom" : undefined}
                    getRowStyle={getCaseRowStyle}
                    suppressLastEmptyLineOnPaste
                    onGridReady={onGridReady}
                    defaultExcelExportParams={defaultExcelExportParams}
                    onCellClicked={(event: CellClickedEvent) => {
                        console.log("[DEBUG] cell clicked", {
                            column: event.column?.getColId(),
                            profileName: event.data?.profileName,
                            editMode,
                            colDef: event.colDef,
                            editing: event.column?.getColDef()?.editable,
                            api: event.api.getEditingCells().length > 0,
                        })
                    }}
                    onCellDoubleClicked={(event: CellDoubleClickedEvent) => {
                        console.log("[DEBUG] cell double clicked", {
                            column: event.column?.getColId(),
                            profileName: event.data?.profileName,
                            editMode,
                            colDef: event.colDef,
                            editing: event.column?.getColDef()?.editable,
                            api: event.api.getEditingCells().length > 0,
                        })
                    }}
                />
            </div>
        </div>
    )
}

export default CaseTabTable
