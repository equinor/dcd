import { ColDef } from "@ag-grid-community/core"
import { AgGridReact } from "@ag-grid-community/react"
import { Button, Icon } from "@equinor/eds-core-react"
import { delete_to_trash } from "@equinor/eds-icons"
import React, {
    useMemo,
    useRef,
    Dispatch,
    SetStateAction,
} from "react"

import DeleteWellInUseModal from "@/Components/Modal/deleteWellInUseModal"
import SecondaryTableHeader from "@/Components/Tables/Components/SecondaryTableHeader"
import { TableWell } from "@/Models/Interfaces"
import { WellCategory } from "@/Models/enums"
import { GetWellService } from "@/Services/WellService"
import { formatCurrencyUnit } from "@/Utils/FormatingUtils"
import { cellStyleRightAlign, getCustomContextMenuItems } from "@/Utils/TableUtils"

interface WellsTableProps {
    rowData: TableWell[]
    editMode: boolean
    isEditDisabled: boolean
    isExplorationWellTable: boolean
    wellOptions: Array<{ key: string; value: WellCategory; label: string }>
    revisionAndProjectData: Components.Schemas.ProjectDataDto | Components.Schemas.RevisionDataDto | null | undefined
    addWellsEdit: (
        projectId: string,
        fusionProjectId: string,
        updatePayload: Components.Schemas.UpdateWellsDto,
    ) => void
    defaultWellCategory: WellCategory
    wellStagedForDeletion: any
    setWellStagedForDeletion: Dispatch<SetStateAction<any>>
}

const WellsTable: React.FC<WellsTableProps> = ({
    rowData,
    editMode,
    isEditDisabled,
    isExplorationWellTable,
    wellOptions,
    revisionAndProjectData,
    addWellsEdit,
    defaultWellCategory,
    wellStagedForDeletion,
    setWellStagedForDeletion,
}) => {
    const gridRef = useRef<AgGridReact<TableWell>>(null)

    const onGridReady = (params: any) => {
        if (gridRef.current) {
            gridRef.current.api = params.api
        }
    }

    const onRowValueChanged = (event: any) => {
        const updatedData = event.data
        const previousData = updatedData?.well

        if (!previousData || !updatedData || !revisionAndProjectData) { return }

        const updatedWell: Components.Schemas.UpdateWellDto = {
            id: previousData.id,
            name: updatedData.name || previousData.name || "",
            wellCategory: updatedData.wellCategory !== undefined && updatedData.wellCategory !== null
                ? updatedData.wellCategory
                : previousData.wellCategory || defaultWellCategory,
            drillingDays: updatedData.drillingDays !== undefined ? updatedData.drillingDays : previousData.drillingDays ?? 0,
            wellCost: updatedData.wellCost !== undefined ? updatedData.wellCost : previousData.wellCost ?? 0,
            wellInterventionCost: updatedData.wellInterventionCost !== undefined ? updatedData.wellInterventionCost : previousData.wellInterventionCost ?? 0,
            plugingAndAbandonmentCost: updatedData.plugingAndAbandonmentCost !== undefined ? updatedData.plugingAndAbandonmentCost : previousData.plugingAndAbandonmentCost ?? 0,
        }

        const updatePayload: Components.Schemas.UpdateWellsDto = {
            createWellDtos: [],
            updateWellDtos: [updatedWell],
            deleteWellDtos: [],
        }

        addWellsEdit(revisionAndProjectData.projectId, revisionAndProjectData.commonProjectAndRevisionData.fusionProjectId, updatePayload)
    }

    const handleDeleteWell = async (params: any) => {
        if (!revisionAndProjectData) { return }

        const { well: wellToDelete } = params.data
        const deleteWells: Components.Schemas.UpdateWellsDto = {
            createWellDtos: [],
            updateWellDtos: [],
            deleteWellDtos: [{ id: wellToDelete.id }],
        }

        addWellsEdit(revisionAndProjectData.projectId, revisionAndProjectData.commonProjectAndRevisionData.fusionProjectId, deleteWells)

        setWellStagedForDeletion(undefined)
    }

    const deleteWellRenderer = (p: any) => (
        <Button
            variant="ghost_icon"
            disabled={!editMode || isEditDisabled}
            onClick={async () => {
                if (!revisionAndProjectData) { return }
                const isWellInUse = await GetWellService().isWellInUse(revisionAndProjectData.projectId, p.data.id)

                if (isWellInUse) {
                    setWellStagedForDeletion(p)
                } else {
                    handleDeleteWell(p)
                }
            }}
        >
            <Icon data={delete_to_trash} />
        </Button>
    )

    const columnDefs = useMemo<ColDef[]>(
        () => {
            const commonColumns: ColDef[] = [
                {
                    field: "name",
                    flex: 2,
                    editable: editMode && !isEditDisabled,
                    singleClickEdit: true,
                },
                {
                    field: "wellCategory",
                    headerName: "Well type",
                    cellEditor: "agSelectCellEditor",
                    cellEditorParams: {
                        values: wellOptions.map((option) => option.value),
                        formatValue: (value: number) => wellOptions.find((opt) => opt.value === value)?.label || "",
                    },
                    valueFormatter: (params) => wellOptions.find((opt) => opt.value === params.value)?.label || "",
                    editable: editMode && !isEditDisabled,
                    flex: 2,
                    singleClickEdit: true,
                },
                {
                    field: "drillingDays",
                    headerName: "Drilling and completion days",
                    flex: 1,
                    cellStyle: cellStyleRightAlign,
                    editable: editMode && !isEditDisabled,
                    singleClickEdit: true,
                },
                {
                    field: "wellCost",
                    headerName: `Cost (${formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency)})`,
                    flex: 1,
                    headerComponent: SecondaryTableHeader,
                    headerComponentParams: {
                        columnHeader: "Cost",
                        unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    },
                    cellStyle: cellStyleRightAlign,
                    editable: editMode && !isEditDisabled,
                    singleClickEdit: true,
                },
                {
                    field: "delete",
                    headerName: "",
                    cellRenderer: deleteWellRenderer,
                    editable: false,
                    width: 80,
                    singleClickEdit: false,
                },
            ]

            if (!isExplorationWellTable) {
                const additionalColumns: ColDef[] = [
                    {
                        field: "wellInterventionCost",
                        headerName: `Cost (${formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency)})`,
                        flex: 2,
                        headerComponent: SecondaryTableHeader,
                        headerComponentParams: {
                            columnHeader: "Well intervention cost",
                            unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                        },
                        cellStyle: cellStyleRightAlign,
                        editable: editMode && !isEditDisabled,
                        singleClickEdit: true,
                    },
                    {
                        field: "plugingAndAbandonmentCost",
                        headerName: `Cost (${formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency)})`,
                        flex: 2,
                        headerComponent: SecondaryTableHeader,
                        headerComponentParams: {
                            columnHeader: "Pluging and abandonment cost",
                            unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                        },
                        cellStyle: cellStyleRightAlign,
                        editable: editMode && !isEditDisabled,
                        singleClickEdit: true,
                    },
                ]

                commonColumns.splice(commonColumns.length - 1, 0, ...additionalColumns)
            }

            return commonColumns
        },
        [editMode, isEditDisabled, revisionAndProjectData, isExplorationWellTable],
    )

    const defaultColDef = useMemo<ColDef>(
        () => ({
            sortable: true,
            filter: true,
            resizable: true,
            editable: true,
            suppressHeaderMenuButton: true,
            cellClass: editMode && !isEditDisabled ? "editableCell" : undefined,
        }),
        [editMode, isEditDisabled],
    )

    return (
        <>
            <AgGridReact
                ref={gridRef}
                rowData={rowData}
                columnDefs={columnDefs}
                defaultColDef={defaultColDef}
                getContextMenuItems={getCustomContextMenuItems}
                animateRows
                editType="fullRow"
                domLayout="autoHeight"
                onGridReady={onGridReady}
                stopEditingWhenCellsLoseFocus
                onRowValueChanged={onRowValueChanged}
                rowSelection={{
                    mode: "multiRow",
                    enableClickSelection: false,
                    checkboxes: false,
                    headerCheckbox: false,
                }}
            />
            <DeleteWellInUseModal
                isOpen={!!wellStagedForDeletion}
                onClose={(): void => setWellStagedForDeletion(undefined)}
                onConfirm={(): Promise<void> => handleDeleteWell(wellStagedForDeletion)}
            />
        </>
    )
}

export default WellsTable
