import React, {
    useMemo,
    useRef,
    Dispatch,
    SetStateAction,
} from "react"
import { AgGridReact } from "@ag-grid-community/react"
import { ColDef } from "@ag-grid-community/core"

import { Button, Icon } from "@equinor/eds-core-react"
import { delete_to_trash } from "@equinor/eds-icons"

import SecondaryTableHeader from "@/Components/Tables/Components/SecondaryTableHeader"
import { cellStyleRightAlign } from "@/Utils/common"
import { GetWellService } from "@/Services/WellService"
import { TableWell } from "@/Models/Wells"
import DeleteWellInUseModal from "@/Components/Modal/deleteWellInUseModal"
import { Currency, WellCategory } from "@/Models/enums"

interface WellsTableProps {
    rowData: TableWell[]
    editMode: boolean
    isEditDisabled: boolean
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
            wellCategory: updatedData.wellCategory || previousData.wellCategory || defaultWellCategory,
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
        () => [
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
                headerName: "Drilling days",
                flex: 1,
                cellStyle: cellStyleRightAlign,
                editable: editMode && !isEditDisabled,
                singleClickEdit: true,
            },
            {
                field: "wellCost",
                headerName: `Cost (${revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok
                    ? "mill NOK"
                    : "mill USD"
                })`,
                flex: 1,
                headerComponent: SecondaryTableHeader,
                headerComponentParams: {
                    columnHeader: "Cost",
                    unit:
                        revisionAndProjectData?.commonProjectAndRevisionData.currency === Currency.Nok
                            ? "mill NOK"
                            : "mill USD",
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
        ],
        [editMode, isEditDisabled, revisionAndProjectData],
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
                onClose={() => setWellStagedForDeletion(undefined)}
                onConfirm={() => handleDeleteWell(wellStagedForDeletion)}
            />
        </>
    )
}

export default WellsTable
