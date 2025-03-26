import { ColDef } from "@ag-grid-community/core"
import { AgGridReact } from "@ag-grid-community/react"
import { useMemo } from "react"
import styled from "styled-components"

import { CaseNameCell } from "@/Components/Tables/Components/CellRenderers/CaseNameCell"
import { MenuButtonCell } from "@/Components/Tables/Components/CellRenderers/MenuButtonCell"
import { ProductionStrategyCell } from "@/Components/Tables/Components/CellRenderers/ProductionStrategyCell"
import { TableCase } from "@/Models/Interfaces"
import { getCustomContextMenuItems } from "@/Utils/AgGridUtils"
import { cellStyleRightAlign } from "@/Utils/commonUtils"

const AgTableContainer = styled.div`
    overflow: auto;
`

interface Props {
    cases: TableCase[]
    isRevision: boolean
    revisionId?: string
    onMenuClick: (caseId: string, target: HTMLElement) => void
}

export const CasesTable = ({
    cases,
    isRevision,
    revisionId,
    onMenuClick,
}: Props) => {
    const defaultColumnDefinition = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        suppressHeaderMenuButton: true,
    }), [])

    const columnDefinitions = useMemo<ColDef<TableCase>[]>(() => [
        {
            field: "name" as keyof TableCase,
            cellRenderer: CaseNameCell,
            cellRendererParams: { isRevision, revisionId },
            minWidth: 150,
            maxWidth: 500,
            flex: 1,
        },
        {
            field: "productionStrategyOverview" as keyof TableCase,
            headerName: "Production Strategy Overview",
            headerTooltip: "Production Strategy Overview",
            cellRenderer: ProductionStrategyCell,
            width: 280,
        },
        {
            field: "producerCount" as keyof TableCase,
            headerName: "Producers",
            width: 130,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "gasInjectorCount" as keyof TableCase,
            headerName: "Gas injectors",
            width: 155,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "waterInjectorCount" as keyof TableCase,
            headerName: "Water injectors",
            width: 170,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "createdAt" as keyof TableCase,
            headerName: "Created",
            width: 120,
        },
        {
            field: "Options" as keyof TableCase,
            cellRenderer: MenuButtonCell,
            cellRendererParams: { onMenuClick },
            width: 120,
        },
    ], [isRevision, revisionId, onMenuClick])

    return (
        <AgTableContainer>
            <AgGridReact<TableCase>
                rowData={cases}
                columnDefs={columnDefinitions}
                defaultColDef={defaultColumnDefinition}
                getContextMenuItems={getCustomContextMenuItems}
                animateRows
                domLayout="autoHeight"
            />
        </AgTableContainer>
    )
}
