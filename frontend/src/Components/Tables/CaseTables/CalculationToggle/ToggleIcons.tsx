import React from "react"
import { microsoft_excel } from "@equinor/eds-icons"
import { Icon, Tooltip, Button } from "@equinor/eds-core-react"
import { ICellRendererParams } from "@ag-grid-community/core"
import styled from "styled-components"
import { ExcelHideIcon } from "@/Media/Icons/ExcelHideIcon"
import { CalculatorIcon } from "@/Media/Icons/CalculatorIcon"
import { CalculatorHideIcon } from "@/Media/Icons/CalculatorHideIcon"
import { ITimeSeriesTableDataOverrideWithSet } from "@/Models/ITimeSeries"

const IconButton = styled(Button)`
    min-width: 32px;
    width: 32px;
    height: 32px;
    padding: 0;
    border-radius: 50%;
`

interface ToggleIconProps {
    clickedElement: ICellRendererParams<ITimeSeriesTableDataOverrideWithSet>
    onClick: (params: ICellRendererParams<ITimeSeriesTableDataOverrideWithSet>) => void
    editMode: boolean
}

export const CalculatorToggle: React.FC<ToggleIconProps> = ({ clickedElement, onClick, editMode }) => {
    const isOverride = clickedElement.data?.overrideProfile?.override

    const tooltipText = editMode
        ? isOverride ? "Show calculated numbers" : "Hide calculated numbers"
        : isOverride ? "Currently using manual input values" : "Currently using calculated values"

    return (
        <Tooltip title={tooltipText}>
            <IconButton variant="ghost_icon" color="secondary" onClick={() => onClick(clickedElement)}>
                {isOverride ? <CalculatorHideIcon size={20} /> : <CalculatorIcon size={20} />}
            </IconButton>
        </Tooltip>
    )
}

export const ExcelToggle: React.FC<ToggleIconProps> = ({ clickedElement, onClick, editMode }) => {
    const isOverride = clickedElement.data?.overrideProfile?.override

    const tooltipText = editMode
        ? isOverride ? "Show numbers from PROSP file" : "Hide numbers from PROSP file"
        : isOverride ? "Currently using manual input values" : "Currently using PROSP file values"

    return (
        <Tooltip title={tooltipText}>
            <IconButton variant="ghost_icon" color="secondary" onClick={() => onClick(clickedElement)}>
                {isOverride ? <ExcelHideIcon size={20} /> : <Icon data={microsoft_excel} color="#007079" />}
            </IconButton>
        </Tooltip>
    )
}
