import { useMemo } from "react"
import { cellStyleRightAlign } from "@/Utils/common"
import { CaseNameCell } from "../CellRenderers/CaseNameCell"
import { MenuButtonCell } from "../CellRenderers/MenuButtonCell"
import { ProductionStrategyCell } from "../CellRenderers/ProductionStrategyCell"
import { BaseAgGridTable } from "./BaseAgGridTable"
import { TableCase } from "@/Models/Interfaces"

interface Props {
    cases: TableCase[]
    isRevision: boolean
    revisionId?: string
    onMenuClick: (caseId: string, target: HTMLElement) => void
}

export const ActiveCasesTable = ({
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

    const columnDefinitions = useMemo(() => [
        {
            field: "name",
            cellRenderer: CaseNameCell,
            cellRendererParams: { isRevision, revisionId },
            minWidth: 150,
            maxWidth: 500,
            flex: 1,
        },
        {
            field: "productionStrategyOverview",
            headerName: "Production Strategy Overview",
            headerTooltip: "Production Strategy Overview",
            cellRenderer: ProductionStrategyCell,
            width: 280,
        },
        {
            field: "producerCount",
            headerName: "Producers",
            width: 130,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "gasInjectorCount",
            headerName: "Gas injectors",
            width: 155,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "waterInjectorCount",
            headerName: "Water injectors",
            width: 170,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "createdAt",
            headerName: "Created",
            width: 120,
        },
        {
            field: "Options",
            cellRenderer: MenuButtonCell,
            cellRendererParams: { onMenuClick },
            width: 120,
        },
    ], [isRevision, revisionId, onMenuClick])

    return (
        <BaseAgGridTable
            cases={cases}
            columnDefinitions={columnDefinitions}
            defaultColumnDefinition={defaultColumnDefinition}
        />
    )
}
