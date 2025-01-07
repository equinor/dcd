import { AgGridReact } from "@ag-grid-community/react"
import { ColDef } from "@ag-grid-community/core"
import styled from "styled-components"
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
}: Props) => (
    <AgTableContainer>
        <AgGridReact
            rowData={cases}
            columnDefs={columnDefinitions}
            defaultColDef={defaultColumnDefinition}
            animateRows
            domLayout="autoHeight"
        />
    </AgTableContainer>
)
