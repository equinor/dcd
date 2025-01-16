import {
    Button, Icon, NativeSelect, Typography,
} from "@equinor/eds-core-react"
import {
    ChangeEvent, useEffect, useMemo, useRef, useState,
} from "react"
import { add, delete_to_trash } from "@equinor/eds-icons"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { ColDef } from "@ag-grid-community/core"
import Grid from "@mui/material/Grid"

import { TableWell } from "@/Models/Wells"
import SecondaryTableHeader from "@/Components/AgGrid/SecondaryTableHeader"
import { useAppContext } from "@/Context/AppContext"
import { cellStyleRightAlign, isExplorationWell } from "@/Utils/common"
import { GetWellService } from "@/Services/WellService"
import useEditDisabled from "@/Hooks/useEditDisabled"
import { useDataFetch } from "@/Hooks/useDataFetch"
import Modal from "@/Components/Modal/Modal"
import { SectionHeader } from "../Shared/SharedWellStyles"
import useTechnicalInputEdits from "@/Hooks/useEditTechnicalInput"

const ExplorationWells = () => {
    const { addWellsEdit } = useTechnicalInputEdits()
    const revisionAndProjectData = useDataFetch()
    const { isEditDisabled } = useEditDisabled()
    const { editMode } = useAppContext()
    const gridRef = useRef(null)
    const styles = useStyles()

    const [wells, setWells] = useState<Components.Schemas.WellOverviewDto[] | []>([])

    const [rowData, setRowData] = useState<TableWell[]>()
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

    const updateWells = (p: any) => {
        const currentWells = p.data.wells as Components.Schemas.WellOverviewDto[]
        const { field } = p.colDef
        const wellToUpdate = p.data.well
        const { newValue } = p

        if (!currentWells || !revisionAndProjectData || !field || !wellToUpdate) { return }

        const wellIndex = currentWells.findIndex((w) => w === wellToUpdate)
        if (wellIndex === -1) { return }

        const updateField = (well: Components.Schemas.WellOverviewDto, fieldToChange: string, value: any) => ({
            ...well,
            [fieldToChange]: fieldToChange === "name" ? value : Number(value.toString().replace(/,/g, ".")),
        })

        const updatedWell = updateField(wells[wellIndex], field, newValue)

        const updatedWellsArray = [...wells]
        updatedWellsArray[wellIndex] = updatedWell

        const updatePayload: Components.Schemas.UpdateWellsDto = {
            createWellDtos: [],
            updateWellDtos: updatedWellsArray.filter(isExplorationWell),
            deleteWellDtos: [],
        }

        addWellsEdit(revisionAndProjectData.projectId, updatePayload)
    }

    const wellsToRowData = () => {
        if (wells) {
            const tableWells: TableWell[] = []
            wells.forEach((w) => {
                const tableWell: TableWell = {
                    id: w.id!,
                    name: w.name ?? "",
                    wellCategory: 4,
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
                <option key="4" value={4}>Exploration well</option>
                <option key="5" value={5}>Appraisal well</option>
                <option key="6" value={6}>Sidetrack</option>
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
        onCellValueChanged: updateWells,
        suppressHeaderMenuButton: true,
        cellClass: editMode ? "editableCell" : undefined,
    }), [])

    const columnDefs = useMemo(() => [
        {
            field: "name",
            flex: 2,
            editable: editMode,
        },
        {
            field: "wellCategory",
            headerName: "Well type",
            cellRenderer: wellCategoryRenderer,
            editable: false,
            flex: 2,
        },
        {
            field: "drillingDays",
            headerName: "Drilling days",
            flex: 1,
            cellStyle: cellStyleRightAlign,
            editable: editMode,
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
        },
        {
            field: "delete",
            headerName: "",
            cellRenderer: deleteWellRenderer,
            editable: false,
            width: 80,
        },
    ] as ColDef[], [editMode])

    useEffect(() => {
        if (revisionAndProjectData?.commonProjectAndRevisionData.wells) {
            setWells(revisionAndProjectData.commonProjectAndRevisionData.wells.filter((w) => isExplorationWell(w)))
        }
    }, [revisionAndProjectData])

    useEffect(() => {
        wellsToRowData()
    }, [wells])

    return (
        <>
            <SectionHeader>
                <Typography variant="h2">Exploration Well Costs</Typography>
                {editMode && (
                    <Button
                        onClick={
                            () => CreateWell(4)
                        }
                        variant="outlined"
                    >
                        <Icon data={add} />
                        Add new exploration well type
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
                        domLayout="autoHeight"
                        onGridReady={onGridReady}
                        stopEditingWhenCellsLoseFocus
                        singleClickEdit={editMode}
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
export default ExplorationWells
