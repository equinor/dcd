import {
    Dispatch,
    SetStateAction,
    useMemo,
    useState,
    useEffect,
    useCallback,
    useRef,
} from "react"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { useParams } from "react-router"
import {
    CellKeyDownEvent, ColDef, GridReadyEvent,
} from "@ag-grid-community/core"
import isEqual from "lodash/isEqual"
import {
    extractTableTimeSeriesValues,
    generateProfile,
    tableCellisEditable,
    numberValueParser,
    getCaseRowStyle,
    validateInput,
    formatColumnSum,
} from "../../../Utils/common"
import { OverrideTimeSeriesPrompt } from "../../Modal/OverrideTimeSeriesPrompt"
import { useAppContext } from "../../../Context/AppContext"
import ErrorCellRenderer from "./ErrorCellRenderer"
import ClickableLockIcon from "./ClickableLockIcon"
import profileAndUnitInSameCell from "./ProfileAndUnitInSameCell"
import { useProjectContext } from "../../../Context/ProjectContext"
import useDataEdits from "../../../Hooks/useDataEdits"
import { ProfileNames } from "../../../Models/Interfaces"

interface Props {
    timeSeriesData: any[]
    dg4Year: number
    tableYears: [number, number]
    tableName: string
    alignedGridsRef?: any[]
    gridRef?: any
    includeFooter: boolean
    totalRowName?: string
}

const CaseTabTable = ({
    timeSeriesData,
    dg4Year,
    tableYears,
    tableName,
    alignedGridsRef,
    gridRef,
    includeFooter,
    totalRowName,
}: Props) => {
    const { editMode, setSnackBarMessage } = useAppContext()
    const styles = useStyles()
    const { project } = useProjectContext()
    const { addEdit } = useDataEdits()
    const { caseId } = useParams()

    const [overrideModalOpen, setOverrideModalOpen] = useState<boolean>(false)
    const [overrideModalProfileName, setOverrideModalProfileName] = useState<ProfileNames>()
    const [overrideModalProfileSet, setOverrideModalProfileSet] = useState<Dispatch<SetStateAction<any | undefined>>>()
    const [overrideProfile, setOverrideProfile] = useState<any>()
    const [stagedEdit, setStagedEdit] = useState<any>()
    const firstTriggerRef = useRef<boolean>(true)
    const timerRef = useRef<NodeJS.Timeout | null>(null)
    useEffect(() => {
        if (stagedEdit) {
            addEdit(stagedEdit)
        }
    }, [stagedEdit])

    const profilesToRowData = () => {
        const tableRows: any[] = []
        timeSeriesData.forEach((ts) => {
            const isOverridden = ts.overrideProfile?.override === true
            const rowObject: any = {}
            const { profileName, unit } = ts
            rowObject.profileName = profileName
            rowObject.unit = unit
            rowObject.total = ts.total ?? 0
            rowObject.set = isOverridden ? ts.overrideProfileSet : ts.set
            rowObject.profile = isOverridden ? ts.overrideProfile : ts.profile
            rowObject.override = ts.overrideProfile?.override === true
            rowObject.resourceId = ts.resourceId
            rowObject.resourceName = ts.resourceName
            rowObject.overridable = ts.overridable
            rowObject.editable = ts.editable
            rowObject.hideIfEmpty = ts.hideIfEmpty

            rowObject.overrideProfileSet = ts.overrideProfileSet
            rowObject.overrideProfile = ts.overrideProfile ?? {
                startYear: 0, values: [], override: false,
            }

            if (rowObject.profile && rowObject.profile.values?.length > 0) {
                let j = 0
                if (tableName === "Production profiles" || tableName === "CO2 emissions") {
                    for (let i = rowObject.profile.startYear;
                        i < rowObject.profile.startYear + rowObject.profile.values.length;
                        i += 1) {
                        rowObject[(dg4Year + i).toString()] = rowObject.profile.values.map(
                            (v: number) => Math.round((v + Number.EPSILON) * 10000) / 10000,
                        )[j]
                        j += 1
                        rowObject.total = Math.round(rowObject.profile.values.map(
                            (v: number) => (v + Number.EPSILON),
                        ).reduce((x: number, y: number) => x + y) * 10000) / 10000
                        if (ts.total !== undefined) {
                            rowObject.total = Math.round(ts.total * 1000) / 1000
                        }
                    }
                } else {
                    for (let i = rowObject.profile.startYear;
                        i < rowObject.profile.startYear + rowObject.profile.values.length;
                        i += 1) {
                        rowObject[(dg4Year + i).toString()] = rowObject.profile.values.map(
                            (v: number) => Math.round((v + Number.EPSILON) * 10) / 10,
                        )[j]
                        j += 1
                        rowObject.total = Math.round(rowObject.profile.values.map(
                            (v: number) => (v + Number.EPSILON),
                        ).reduce((x: number, y: number) => x + y) * 10) / 10
                    }
                }
            }

            const isNotHidden = !rowObject.hideIfEmpty
            const hasProfileValues = rowObject.hideIfEmpty && rowObject.profile?.values.length > 0

            if (editMode || isNotHidden || hasProfileValues) {
                tableRows.push(rowObject)
            }
        })

        return tableRows
    }

    const gridRowData = useMemo(() => gridRef.current?.api?.setGridOption("rowData", profilesToRowData()), [timeSeriesData, editMode])

    const lockIconRenderer = (params: any) => (
        <ClickableLockIcon
            clickedElement={params}
            setOverrideModalOpen={setOverrideModalOpen}
            setOverrideModalProfileName={setOverrideModalProfileName}
            setOverrideModalProfileSet={setOverrideModalProfileSet}
            setOverrideProfile={setOverrideProfile}
        />
    )

    const generateTableYearColDefs = () => {
        const columnPinned: any[] = [
            {
                field: "profileName",
                headerName: tableName,
                cellRenderer: (params: any) => (
                    profileAndUnitInSameCell(params, profilesToRowData())
                ),
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
                width: 60,
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
    }

    const [columnDefs, setColumnDefs] = useState<ColDef[]>(generateTableYearColDefs())

    const stageEdit = (params: any) => {
        const tableTimeSeriesValues = extractTableTimeSeriesValues(params.data)
        const existingProfile = params.data.profile ? structuredClone(params.data.profile) : {
            startYear: 0, values: [],
        }
        let newProfile
        if (tableTimeSeriesValues.length > 0) {
            const firstYear = tableTimeSeriesValues[0].year
            const lastYear = tableTimeSeriesValues[tableTimeSeriesValues.length - 1].year
            const startYear = firstYear - dg4Year
            newProfile = generateProfile(tableTimeSeriesValues, params.data.profile, startYear, firstYear, lastYear)
        } else {
            newProfile = structuredClone(existingProfile)
            newProfile.values = []
        }

        if (!newProfile || !caseId || !project) {
            return
        }

        const timeSeriesDataIndex = () => timeSeriesData
            .map((v, i) => (v.profileName === params.data.profileName ? timeSeriesData[i] : undefined))
            .find((v) => v !== undefined)

        if (!isEqual(newProfile.values, existingProfile.values)) {
            setStagedEdit({
                newDisplayValue: newProfile.values.map((value: string) => Math.floor(Number(value) * 10000) / 10000).join(" - "),
                previousDisplayValue: existingProfile.values.map((value: string) => Math.floor(Number(value) * 10000) / 10000).join(" - "),
                newValue: params.newValue,
                previousValue: params.oldValue,
                inputLabel: params.data.profileName,
                projectId: project.id,
                resourceName: timeSeriesDataIndex()?.resourceName,
                resourcePropertyKey: timeSeriesDataIndex()?.resourcePropertyKey,
                caseId,
                resourceId: timeSeriesDataIndex()?.resourceId,
                newResourceObject: newProfile,
                previousResourceObject: existingProfile,
                resourceProfileId: timeSeriesDataIndex()?.resourceProfileId,
            })
        }
    }

    const handleCellValueChange = useCallback((params: any) => {
        if (firstTriggerRef.current) {
            firstTriggerRef.current = false
            stageEdit(params)

            if (timerRef.current) {
                clearTimeout(timerRef.current)
            }

            timerRef.current = setTimeout(() => {
                firstTriggerRef.current = true
            }, 500)
        }
    }, [stageEdit, timeSeriesData, dg4Year, project, caseId])

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

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        editable: true,
        onCellValueChanged: handleCellValueChange,
        suppressHeaderMenuButton: true,
    }), [timeSeriesData])

    useEffect(() => {
        const newColDefs = generateTableYearColDefs()
        setColumnDefs(newColDefs)
    }, [tableYears, editMode])

    const onGridReady = useCallback((params: GridReadyEvent) => {
        const generateRowData = profilesToRowData()
        params.api.setGridOption("rowData", generateRowData)
    }, [])

    const defaultExcelExportParams = useMemo(() => {
        const yearColumnKeys = Array.from({ length: tableYears[1] - tableYears[0] + 1 }, (_, i) => (tableYears[0] + i).toString())
        const columnKeys = ["profileName", "unit", ...yearColumnKeys, "total"]
        return {
            columnKeys,
            fileName: "export.xlsx",
        }
    }, [tableYears])

    const clearCellsInRange = (start: any, end: any, columns: any) => {
        Array.from({ length: end - start + 1 }, (_, i) => start + i).forEach((i) => {
            const rowNode = gridRef.current?.api.getRowNode(i)
            if (rowNode) {
                columns.forEach((column: any) => {
                    rowNode.setDataValue(column, "")
                })
            }
        })
    }

    const handleDeleteOnRange = useCallback(
        (e: CellKeyDownEvent) => {
            const keyboardEvent = e.event as unknown as KeyboardEvent
            const { key } = keyboardEvent

            if (key === "Backspace") {
                const cellRanges = e.api.getCellRanges()
                if (!cellRanges || cellRanges.length === 0) {
                    return
                }

                cellRanges.forEach((cells) => {
                    if (cells.startRow && cells.endRow) {
                        const startRowIndex = Math.min(
                            cells.startRow.rowIndex,
                            cells.endRow.rowIndex,
                        )
                        const endRowIndex = Math.max(
                            cells.startRow.rowIndex,
                            cells.endRow.rowIndex,
                        )

                        const colIds = cells.columns.map(
                            (col: any) => col.getColDef().field,
                        )

                        clearCellsInRange(startRowIndex, endRowIndex, colIds)
                    }
                })
            }
        },
        [clearCellsInRange],
    )

    return (
        <>
            <OverrideTimeSeriesPrompt
                isOpen={overrideModalOpen}
                setIsOpen={setOverrideModalOpen}
                profileName={overrideModalProfileName}
                setProfile={overrideModalProfileSet}
                profile={overrideProfile}
            />
            <div className={styles.root}>
                <div
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
                        enableCellChangeFlash={editMode}
                        rowSelection="multiple"
                        enableRangeSelection
                        suppressCopySingleCellRanges
                        suppressMovableColumns
                        enableCharts
                        alignedGrids={gridRefArrayToAlignedGrid()}
                        groupIncludeTotalFooter={includeFooter}
                        getRowStyle={getCaseRowStyle}
                        suppressLastEmptyLineOnPaste
                        stopEditingWhenCellsLoseFocus
                        onGridReady={onGridReady}
                        defaultExcelExportParams={defaultExcelExportParams}
                        onCellKeyDown={editMode ? handleDeleteOnRange : undefined}
                    />
                </div>
            </div>
        </>
    )
}

export default CaseTabTable
