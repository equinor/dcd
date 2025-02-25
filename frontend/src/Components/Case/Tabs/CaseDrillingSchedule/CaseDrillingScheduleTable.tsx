import {
    useMemo,
    useState,
    useEffect,
    useCallback,
} from "react"

import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { ColDef } from "@ag-grid-community/core"
import { useParams } from "react-router"
import isEqual from "lodash/isEqual"
import { v4 as uuidv4 } from "uuid"
import {
    cellStyleRightAlign,
    getValuesFromEntireRow,
    generateProfile,
    numberValueParser,
} from "../../../../Utils/common"
import { useAppStore } from "../../../../Store/AppStore"
import { useProjectContext } from "../../../../Store/ProjectContext"
import { gridRefArrayToAlignedGrid, wellsToRowData } from "@/Components/AgGrid/AgGridHelperFunctions"
import useEditCase from "@/Hooks/useEditCase"
import { EditInstance } from "@/Models/Interfaces"
import useCanUserEdit from "@/Hooks/useCanUserEdit"

interface Props {
    dg4Year: number
    tableYears: [number, number]
    tableName: string
    alignedGridsRef?: any[]
    gridRef?: any
    assetWells: Components.Schemas.ExplorationWellDto[] | Components.Schemas.DevelopmentWellDto[]
    wells: Components.Schemas.WellOverviewDto[] | undefined
    resourceId: string
    isExplorationTable: boolean
}

const CaseDrillingScheduleTabTable = ({
    dg4Year,
    tableYears,
    tableName,
    alignedGridsRef,
    gridRef,
    assetWells,
    wells,
    resourceId,
    isExplorationTable,
}: Props) => {
    const { editMode, setSnackBarMessage } = useAppStore()
    const { caseId } = useParams()
    const { projectId } = useProjectContext()
    const styles = useStyles()
    const { addEdit } = useEditCase()
    const { canEdit, isEditDisabled } = useCanUserEdit()

    const [rowData, setRowData] = useState<any[]>([])
    const [stagedEdit, setStagedEdit] = useState<EditInstance | undefined>()

    const generateTableYearColDefs = () => {
        const columnPinned: any[] = [
            {
                field: "name",
                headerName: tableName,
                width: 250,
                editable: false,
                pinned: "left",
            },
            {
                field: "total",
                flex: 2,
                editable: false,
                pinned: "right",
                width: 100,
                cellStyle: { fontWeight: "bold", textAlign: "right" },
            },
        ]
        const yearDefs: any[] = []
        for (let index = tableYears[0]; index <= tableYears[1]; index += 1) {
            yearDefs.push({
                field: index.toString(),
                flex: 1,
                minWidth: 100,
                editable: canEdit(),
                cellClass: canEdit() ? "editableCell" : undefined,
                cellStyle: cellStyleRightAlign,
                valueParser: (params: any) => numberValueParser(setSnackBarMessage, params),
            })
        }
        return columnPinned.concat([...yearDefs])
    }

    const [columnDefs, setColumnDefs] = useState<ColDef[]>(generateTableYearColDefs())

    const stageEdit = (params: any) => {
        const rowValues = getValuesFromEntireRow(params.data)
        const existingProfile = params.data.drillingSchedule ? structuredClone(params.data.drillingSchedule) : {
            startYear: 0,
            values: [],
            id: resourceId,
        }

        let newProfile
        if (rowValues.length > 0) {
            const firstYear = rowValues[0].year
            const lastYear = rowValues[rowValues.length - 1].year
            const startYear = firstYear - dg4Year
            newProfile = generateProfile(rowValues, params.data.drillingSchedule, startYear, firstYear, lastYear)
        } else {
            newProfile = structuredClone(existingProfile)
            newProfile.values = []
        }

        if (!caseId || !newProfile) { return }

        const rowWells = params.data.assetWells
        if (rowWells) {
            const index = rowWells.findIndex((w: any) => w === params.data.assetWell)
            if (index > -1) {
                const well = rowWells[index]
                const updatedWell = { ...well, drillingSchedule: newProfile }
                const updatedWells = [...rowWells]
                updatedWells[index] = updatedWell
                const resourceName = isExplorationTable ? "explorationWellDrillingSchedule" : "developmentWellDrillingSchedule"

                if (!isEqual(newProfile.values, existingProfile.values)) {
                    setStagedEdit({
                        uuid: uuidv4(),
                        projectId,
                        resourceName,
                        resourcePropertyKey: "drillingSchedule",
                        caseId,
                        resourceId,
                        resourceObject: newProfile,
                        wellId: updatedWell.wellId,
                        drillingScheduleId: newProfile.id,
                    })
                }
            }
        }
    }

    const handleCellValueChange = useCallback((params: any) => {
        stageEdit(params)
    }, [stageEdit])

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        editable: true,
        enableCellChangeFlash: true,
        onCellValueChanged: handleCellValueChange,
        suppressHeaderMenuButton: true,
    }), [assetWells])

    useEffect(() => {
        const newRowData = wellsToRowData(assetWells, wells, dg4Year, canEdit(), resourceId, isExplorationTable)
        setRowData(newRowData)
        const newColDefs = generateTableYearColDefs()
        setColumnDefs(newColDefs)
    }, [assetWells, tableYears, wells, editMode, isEditDisabled])

    useEffect(() => {
        if (stagedEdit) {
            addEdit(stagedEdit)
        }
    }, [stagedEdit])

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
                    enableCharts
                    alignedGrids={alignedGridsRef ? gridRefArrayToAlignedGrid(alignedGridsRef) : undefined}
                    stopEditingWhenCellsLoseFocus
                    suppressLastEmptyLineOnPaste
                />
            </div>
        </div>
    )
}

export default CaseDrillingScheduleTabTable
