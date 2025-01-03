import { AgGridReact } from "@ag-grid-community/react"
import { ColDef } from "@ag-grid-community/core"
import styled from "styled-components"
import { useRef } from "react"
import { TableCase } from "@/Models/Interfaces"

const AgTableContainer = styled.div`
    overflow: auto;
`

interface Props {
    cases: TableCase[]
    columnDefinitions: ColDef[]
    defaultColumnDefinition: ColDef
}

export const BaseAgGridTable = ({
    cases,
    columnDefinitions,
    defaultColumnDefinition,
}: Props) => {
    const gridRef = useRef<AgGridReact>(null)

    return (
        <AgTableContainer>
            <AgGridReact
                ref={gridRef}
                rowData={cases}
                columnDefs={columnDefinitions}
                defaultColDef={defaultColumnDefinition}
                animateRows
                domLayout="autoHeight"
            />
        </AgTableContainer>
    )
}
