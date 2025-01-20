import {
    Button, Icon, NativeSelect, Typography,
} from "@equinor/eds-core-react"
import { delete_to_trash, add } from "@equinor/eds-icons"
import {
    ChangeEvent, useEffect, useMemo, useRef, useState,
} from "react"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { ColDef } from "@ag-grid-community/core"
import Grid from "@mui/material/Grid"

import SecondaryTableHeader from "@/Components/AgGrid/SecondaryTableHeader"
import { useAppContext } from "@/Context/AppContext"
import { cellStyleRightAlign } from "@/Utils/common"
import { GetWellService } from "@/Services/WellService"
import useEditDisabled from "@/Hooks/useEditDisabled"
import { useDataFetch } from "@/Hooks/useDataFetch"
import Modal from "@/Components/Modal/Modal"
import { TableWell } from "@/Models/Wells"
import { SectionHeader } from "./SharedWellStyles"
import useTechnicalInputEdits from "@/Hooks/useEditTechnicalInput"

interface WellsProps {
    title: string
    addButtonText: string
    defaultWellCategory: number
    wellOptions: Array<{ key: string; value: number; label: string }>
    filterWells: (well: Components.Schemas.WellOverviewDto) => boolean
}

const Wells = ({
    title,
    addButtonText,
    defaultWellCategory,
    wellOptions,
    filterWells,
}: WellsProps) => {
    const revisionAndProjectData = useDataFetch()
    const { addWellsEdit } = useTechnicalInputEdits()
    const { editMode } = useAppContext()
    const { isEditDisabled } = useEditDisabled()
    const gridRef = useRef(null)
    const styles = useStyles()

    const [rowData, setRowData] = useState<TableWell[]>([])
    const [wellStagedForDeletion, setWellStagedForDeletion] = useState<any | undefined>()

    const onGridReady = (params: any) => { gridRef.current = params.api }

    const CreateWell = async (category: number) => {
        const newWell: Components.Schemas.CreateWellDto = {
            wellCategory: category as Components.Schemas.WellCategory,
            name: "New well",
        }
        if (revisionAndProjectData) {
            const createWells: Components.Schemas.UpdateWellsDto = {
                createWellDtos: [newWell],
                updateWellDtos: [],
                deleteWellDtos: [],
            }
            addWellsEdit(revisionAndProjectData.projectId, createWells)
        }
    }

    const wellsToRowData = (wells: Components.Schemas.WellOverviewDto[]) => {
        if (wells) {
            const tableWells: TableWell[] = []
            wells.forEach((w) => {
                const tableWell: TableWell = {
                    id: w.id!,
                    name: w.name ?? "",
                    wellCategory: defaultWellCategory as Components.Schemas.WellCategory,
                    drillingDays: w.drillingDays ?? 0,
                    wellCost: w.wellCost ?? 0,
                    well: w,
                    wells,
                }
                if (w.wellCategory) { tableWell.wellCategory = w.wellCategory }
                tableWells.push(tableWell)
            })
            setRowData(tableWells)
        }
    }

    const onRowValueChanged = (event: any) => {
        const updatedData = event.data
        const previousData = updatedData.well

        if (!previousData || !updatedData || !revisionAndProjectData) {
            return
        }

        const updatedWell = {
            id: previousData.id,
            name: updatedData.name || previousData.name,
            wellCategory: updatedData.wellCategory || previousData.wellCategory,
            drillingDays: updatedData.drillingDays !== undefined ? updatedData.drillingDays : previousData.drillingDays,
            wellCost: updatedData.wellCost !== undefined ? updatedData.wellCost : previousData.wellCost,
        }

        const updatePayload: Components.Schemas.UpdateWellsDto = {
            createWellDtos: [],
            updateWellDtos: [updatedWell],
            deleteWellDtos: [],
        }

        addWellsEdit(revisionAndProjectData.projectId, updatePayload)
    }

    const handleWellCategoryChange = async (
        e: ChangeEvent<HTMLSelectElement>,
        p: any,
    ) => {
        if ([0, 1, 2, 3, 4, 5, 6, 7].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newProductionStrategy: Components.Schemas.WellCategory = Number(
                e.currentTarget.value,
            ) as Components.Schemas.WellCategory

            p.setValue(newProductionStrategy)
        }
    }

    const wellCategoryRenderer = (p: any) => {
        const value = Number(p.value)
        return (
            <NativeSelect
                id="wellCategory"
                label=""
                value={value}
                disabled={!editMode || isEditDisabled}
                onChange={(e: ChangeEvent<HTMLSelectElement>) => handleWellCategoryChange(e, p)}
            >
                {wellOptions.map((option) => (
                    <option key={option.key} value={option.value}>
                        {option.label}
                    </option>
                ))}
            </NativeSelect>
        )
    }

    const handleDeleteWell = (params: any) => {
        const { well: wellToDelete } = params.data

        if (revisionAndProjectData) {
            const deleteWells: Components.Schemas.UpdateWellsDto = {
                createWellDtos: [],
                updateWellDtos: [],
                deleteWellDtos: [{ id: wellToDelete.id }],
            }
            addWellsEdit(revisionAndProjectData.projectId, deleteWells)
        }

        setWellStagedForDeletion(undefined)
    }

    const deleteWellRenderer = (p: any) => (
        <Button
            variant="ghost_icon"
            disabled={!editMode || isEditDisabled}
            onClick={async () => {
                if (!revisionAndProjectData) { return }
                const isWellInUse = await (await GetWellService()).isWellInUse(revisionAndProjectData.projectId, p.data.id)
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

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        editable: true,
        suppressHeaderMenuButton: true,
        cellClass: editMode ? "editableCell" : undefined,
    }), [editMode])

    const columnDefs = useMemo(() => [
        {
            field: "name",
            flex: 2,
            editable: editMode,
            singleClickEdit: true,
        },
        {
            field: "wellCategory",
            headerName: "Well type",
            cellRenderer: wellCategoryRenderer,
            editable: false,
            flex: 2,
            singleClickEdit: true,
        },
        {
            field: "drillingDays",
            headerName: "Drilling days",
            flex: 1,
            cellStyle: cellStyleRightAlign,
            editable: editMode,
            singleClickEdit: true,
        },
        {
            field: "wellCost",
            headerName: `Cost (${revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "mill NOK" : "mill USD"})`,
            flex: 1,
            headerComponent: SecondaryTableHeader,
            headerComponentParams: {
                columnHeader: "Cost",
                unit: revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "mill NOK" : "mill USD",
            },
            cellStyle: cellStyleRightAlign,
            editable: editMode,
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
    ] as ColDef[], [editMode, revisionAndProjectData?.commonProjectAndRevisionData.currency])

    useEffect(() => {
        const allWells: Components.Schemas.WellOverviewDto[] = revisionAndProjectData?.commonProjectAndRevisionData.wells ?? []
        const filteredWells: Components.Schemas.WellOverviewDto[] = allWells.filter(filterWells) ?? []

        if (allWells && filteredWells.length > 0) {
            wellsToRowData(filteredWells)
        } else {
            setRowData([])
        }
    }, [revisionAndProjectData, filterWells])

    return (
        <>
            <SectionHeader>
                <Typography variant="h2">{title}</Typography>
                {editMode && (
                    <Button
                        onClick={() => CreateWell(defaultWellCategory)}
                        variant="outlined"
                    >
                        <Icon data={add} />
                        {addButtonText}
                    </Button>
                )}
            </SectionHeader>
            <Modal
                isOpen={!!wellStagedForDeletion}
                title="Delete well"
                size="sm"
                content={(
                    <Typography>
                        This well is currently in use in a case. Are you sure you want to delete it?
                    </Typography>
                )}
                actions={(
                    <>
                        <Button
                            onClick={() => setWellStagedForDeletion(undefined)}
                            variant="outlined"
                        >
                            Cancel
                        </Button>
                        <Button
                            onClick={() => handleDeleteWell(wellStagedForDeletion)}
                            variant="contained"
                            color="danger"
                        >
                            Delete well
                        </Button>
                    </>
                )}
            />
            <Grid item xs={12} className={styles.root}>
                <div>
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
                        onCellClicked={(params) => {
                            if (params.column.getColId() === "delete") {
                                params.api.stopEditing()
                            }
                        }}
                        rowSelection={{
                            mode: "multiRow",
                            enableClickSelection: false,
                            checkboxes: false,
                            headerCheckbox: false,
                        }}
                    />
                </div>
            </Grid>
        </>
    )
}

export default Wells
