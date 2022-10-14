import { Button, NativeSelect } from "@equinor/eds-core-react"
import {
    ChangeEvent,
    Dispatch, SetStateAction, useEffect, useMemo, useRef, useState,
} from "react"
import { AgGridReact } from "ag-grid-react"
import { Project } from "../../models/Project"
import { Well } from "../../models/Well"
import "ag-grid-enterprise"
import styled from "styled-components"

const ButtonWrapper = styled.div`
    margin-top: 20px;
    margin-bottom: 40px;
`

interface Props {
    project: Project
    setProject: Dispatch<SetStateAction<Project | undefined>>
    wells: Well[] | undefined
    setWells: Dispatch<SetStateAction<Well[] | undefined>>
    explorationWells: boolean
}

interface TableWell {
    id: string,
    name: string,
    wellCategory: Components.Schemas.WellCategory,
    drillingDays: number,
    wellCost: number,
    well: Well
}

function WellListEditTechnicalInputNew({
    project, setProject, explorationWells, wells, setWells,
}: Props) {
    const gridRef = useRef(null)

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
        if (wells) {
            const { field } = p.colDef
            const index = wells.findIndex((w) => w === p.data.well)
            if (index > -1) {
                const well = wells[index]
                const updatedWell = well
                updatedWell[field as keyof typeof updatedWell] = field === "name" ? p.newValue : Number(p.newValue)
                const updatedWells = [...wells]
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
        debugger
        const value = Number(p.value)
        console.log(value)

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
                        <option key="3" value={3}>Gas Injector</option>
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
    }), [])

    const [columnDefs] = useState([
        {
            field: "name", sort: order, width: 110,
        },
        {
            field: "wellCategory", headerName: "Well type", cellRenderer: wellCategoryRenderer, width: 250,
        },
        {
            field: "drillingDays", width: 110,
        },
        {
            field: "wellCost", headerName: "Cost", width: 90,
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
            <div
                style={{
                    display: "flex", flexDirection: "column", width: "100%",
                }}
                className="ag-theme-alpine"
            >
                <AgGridReact
                    ref={gridRef}
                    rowData={rowData}
                    columnDefs={columnDefs}
                    defaultColDef={defaultColDef}
                    animateRows
                    domLayout="autoHeight"
                    onGridReady={onGridReady}
                />
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

export default WellListEditTechnicalInputNew
