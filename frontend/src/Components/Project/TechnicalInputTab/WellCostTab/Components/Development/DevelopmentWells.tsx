import {
    Button, Icon, NativeSelect, Typography,
} from "@equinor/eds-core-react"
import { delete_to_trash } from "@equinor/eds-icons"
import {
    ChangeEvent, Dispatch, SetStateAction, useEffect, useMemo, useRef, useState,
} from "react"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { ColDef } from "@ag-grid-community/core"
import Grid from "@mui/material/Grid"

import SecondaryTableHeader from "@/Components/AgGrid/SecondaryTableHeader"
import { useAppContext } from "@/Context/AppContext"
import { cellStyleRightAlign, isExplorationWell } from "@/Utils/common"
import { GetWellService } from "@/Services/WellService"
import useEditDisabled from "@/Hooks/useEditDisabled"
import { useDataFetch } from "@/Hooks/useDataFetch"
import Modal from "@/Components/Modal/Modal"
import { TableWell } from "@/Models/Wells"

interface Props {
    setDevelopmentWells: Dispatch<SetStateAction<Components.Schemas.WellOverviewDto[]>>
    setDeletedWells: Dispatch<SetStateAction<string[]>>
}

const DevelopmentWells = ({
    setDevelopmentWells,
    setDeletedWells,
}: Props) => {
    const revisionAndProjectData = useDataFetch()
    const { editMode } = useAppContext()
    const { isEditDisabled } = useEditDisabled()
    const gridRef = useRef(null)
    const styles = useStyles()

    const [wells, setWells] = useState<Components.Schemas.WellOverviewDto[] | []>([])

    const [rowData, setRowData] = useState<TableWell[]>()
    const [wellStagedForDeletion, setWellStagedForDeletion] = useState<any | undefined>()

    const onGridReady = (params: any) => { gridRef.current = params.api }

    const updateWells = (p: any) => {
        const rowWells: any[] = p.data.wells
        if (rowWells) {
            const { field } = p.colDef
            const index = rowWells.findIndex((w) => w === p.data.well)
            if (index > -1) {
                const well = rowWells[index]
                const updatedWell = well
                updatedWell[field as keyof typeof updatedWell] = field === "name"
                    ? p.newValue : Number(p.newValue.toString().replace(/,/g, "."))
                const updatedWells = [...rowWells]
                updatedWells[index] = updatedWell
                setWells(updatedWells)
            }
        }
    }

    const wellsToRowData = () => {
        if (wells) {
            const tableWells: TableWell[] = []
            wells.forEach((w) => {
                const tableWell: TableWell = {
                    id: w.id!,
                    name: w.name ?? "",
                    wellCategory: 0,
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
                <option key="0" value={0}>Oil producer</option>
                <option key="1" value={1}>Gas producer</option>
                <option key="2" value={2}>Water injector</option>
                <option key="3" value={3}>Gas injector</option>
            </NativeSelect>
        )
    }

    const handleDeleteWell = (p: any) => {
        const rowWells: any[] = p.data.wells
        if (rowWells) {
            const index = rowWells.findIndex((w) => w === p.data.well)
            if (index > -1) {
                const updatedWells = [...rowWells]
                updatedWells.splice(index, 1)
                setWells(updatedWells)
                setDeletedWells((prev) => {
                    if (!prev.includes(p.data.well.id)) {
                        const deletedWells = [...prev]
                        deletedWells.push(p.data.well.id)
                        return deletedWells
                    }
                    return prev
                })
            }
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
            setWells(revisionAndProjectData.commonProjectAndRevisionData.wells.filter((w) => !isExplorationWell(w)))
        }
    }, [revisionAndProjectData])

    useEffect(() => {
        wellsToRowData()
    }, [wells])

    useEffect(() => {
        setDevelopmentWells(wells)
    }, [wells])

    return (
        <>
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
export default DevelopmentWells
