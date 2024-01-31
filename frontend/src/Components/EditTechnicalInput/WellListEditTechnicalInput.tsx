import { Button, NativeSelect } from "@equinor/eds-core-react"
import {
    ChangeEvent, Dispatch, SetStateAction, useEffect, useMemo, useRef, useState,
} from "react"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import styled from "styled-components"
import { ColDef } from "@ag-grid-community/core"
import { Project } from "../../models/Project"
import { Well } from "../../models/Well"
import { customUnitHeaderTemplate } from "../../AgGridUnitInHeader"
import { delete_to_trash } from "@equinor/eds-icons"
import { Icon } from "@equinor/eds-core-react"
import { GetWellService } from "../../Services/WellService"

const ButtonWrapper = styled.div`
    margin-top: 20px;
    margin-bottom: 40px;
`

interface Props {
    project: Project
    wells: Well[] | undefined
    setWells: Dispatch<SetStateAction<Well[]>>
    explorationWells: boolean
}

interface TableWell {
    id: string,
    name: string,
    wellCategory: Components.Schemas.WellCategory,
    drillingDays: number,
    wellCost: number,
    well: Well
    wells: Well[]
}

interface DeleteButtonProps {
    wellId: string;
    onDelete: (wellId: string) => void;
}

const DeleteButton: React.FC<DeleteButtonProps> = ({ wellId, onDelete }) => (
    <Button type="button" className="delete-button" onClick={() => onDelete(wellId)}>
        <Icon data={delete_to_trash} size={16} />
    </Button>
)

function WellListEditTechnicalInput({
    project, explorationWells, wells, setWells,
}: Props) {
    const gridRef = useRef(null)
    const styles = useStyles()
    const onGridReady = (params: any) => {
        gridRef.current = params.api
    }

    const [rowData, setRowData] = useState<TableWell[]>()

    const wellsToRowData = () => {
        if (wells) {
            const tableWells: TableWell[] = []
            wells.forEach((w) => {
                const tableWell: TableWell = {
                    id: w.id!,
                    name: w.name ?? "",
                    wellCategory: explorationWells ? 4 : 0,
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

    useEffect(() => {
        wellsToRowData()
    }, [wells])

    const updateWells = (p: any) => {
        const rowWells: Well[] = p.data.wells
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
                onChange={(e: ChangeEvent<HTMLSelectElement>) => handleWellCategoryChange(e, p)}
            >
                {!explorationWells ? (
                    <>
                        <option key="0" value={0}>Oil producer</option>
                        <option key="1" value={1}>Gas producer</option>
                        <option key="2" value={2}>Water injector</option>
                        <option key="3" value={3}>Gas injector</option>
                    </>
                )
                    : (
                        <>
                            <option key="4" value={4}>Exploration well</option>
                            <option key="5" value={5}>Appraisal well</option>
                            <option key="6" value={6}>Sidetrack</option>
                        </>
                    )}
            </NativeSelect>
        )
    }

    type SortOrder = "desc" | "asc" | null
    const order: SortOrder = "asc"

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        editable: true,
        onCellValueChanged: updateWells,
        suppressMenu: true,
    }), [])

    const deleteWell = async (wellIdToDelete: string) => {
        try {
            if (wellIdToDelete && wells) {
                // Attempt to delete the well from the backend
                await (await GetWellService()).deleteWell(wellIdToDelete)

                // If successful, update local state to remove the well
                const updatedWells = wells.filter((well) => well.id !== wellIdToDelete)
                setWells(updatedWells)
            }
        } catch (error: any) {
            // If there's an error (e.g., well not found in the database), handle it
            if (error.response && error.response.status === 404) {
                // If the well is not found, remove it from the local state as well
                if (wells) {
                    const updatedWells = wells.filter((well) => well.id !== wellIdToDelete)
                    setWells(updatedWells)
                }
                console.error(`Well with id ${wellIdToDelete} not found and removed from the local list.`)
            } else {
                // For other errors, log them
                console.error("[ProjectView] error while submitting form data", error)
            }
        }
    }

    const deleteCellRenderer = (params: any) => (
        <DeleteButton wellId={params.data.id} onDelete={deleteWell} />
    )

    const [columnDefs] = useState<ColDef[]>([
        {
            field: "name", sort: order, width: 110,
        },
        {
            field: "wellCategory",
            headerName: "Well type",
            cellRenderer: wellCategoryRenderer,
            width: 250,
            editable: false,
        },
        {
            field: "drillingDays", headerName: "Drilling days", width: 110, flex: 1,
        },
        {
            field: "wellCost",
            headerName: "",
            width: 90,
            flex: 1,
            headerComponentParams: {
                template: customUnitHeaderTemplate("Cost", `${project?.currency === 1 ? "mill NOK" : "mill USD"}`),
            },
        },
        {
            headerName: "",
            width: 90,
            cellRenderer: deleteCellRenderer,

        },
    ])

    const CreateWell = async () => {
        const newWell = new Well()
        newWell.wellCategory = !explorationWells ? 0 : 4
        newWell.name = "New well"
        newWell.projectId = project.projectId
        if (wells) {
            const newWells = [...wells, newWell]
            setWells(newWells)
        } else {
            setWells([newWell])
        }
    }

    return (
        <>
            <div className={styles.root}>
                <div
                    style={{
                        display: "flex", flexDirection: "column", width: "100%",
                    }}
                    className="ag-theme-alpine-fusion"
                >
                    {/* Hardcoded title and description using Typography */}
                    {/* <Title variant="h1">Well Costs</Title>
                    <Description variant="body_long">
                        This input is used to calculate each case's well costs based on their drilling schedules.
                    </Description> */}

                    <AgGridReact
                        ref={gridRef}
                        rowData={rowData}
                        columnDefs={columnDefs}
                        defaultColDef={defaultColDef}
                        animateRows
                        domLayout="autoHeight"
                        onGridReady={onGridReady}
                        stopEditingWhenCellsLoseFocus={true}
                    />
                </div>
            </div>
            <ButtonWrapper>
                <Button onClick={CreateWell} variant="outlined">
                    {explorationWells
                        ? "+   Add new exploration well type" : "+   Add new development/drilling well type"}
                </Button>
            </ButtonWrapper>
        </>
    )
}
export default WellListEditTechnicalInput
