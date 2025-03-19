import React from "react"
import { microsoft_excel } from "@equinor/eds-icons"
import { Icon, Tooltip, Button } from "@equinor/eds-core-react"
import { ICellRendererParams } from "@ag-grid-community/core"
import styled from "styled-components"
import { ExcelHideIcon } from "@/Assets/Icons/ExcelHideIcon"
import { CalculatorIcon } from "@/Assets/Icons/CalculatorIcon"
import { CalculatorHideIcon } from "@/Assets/Icons/CalculatorHideIcon"
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
    editAllowed: boolean
}

export const CalculatorToggle: React.FC<ToggleIconProps> = ({ clickedElement, onClick, editAllowed }) => {
    const isOverride = clickedElement.data?.overrideProfile?.override

    let tooltipText = ""
    if (editAllowed) {
        tooltipText = isOverride ? "Show calculated numbers" : "Hide calculated numbers"
    } else {
        tooltipText = isOverride ? "Currently using manual input values" : "Currently using calculated values"
    }

    return (
        <Tooltip title={tooltipText}>
            <IconButton variant="ghost_icon" color="secondary" onClick={() => onClick(clickedElement)}>
                {isOverride ? <CalculatorHideIcon size={20} /> : <CalculatorIcon size={20} />}
            </IconButton>
        </Tooltip>
    )
}

export const ExcelToggle: React.FC<ToggleIconProps> = ({ clickedElement, onClick, editAllowed }) => {
    const isOverride = clickedElement.data?.overrideProfile?.override

    let tooltipText = ""
    if (editAllowed) {
        tooltipText = isOverride ? "Show numbers from PROSP file" : "Hide numbers from PROSP file"
    } else {
        tooltipText = isOverride ? "Currently using manual input values" : "Currently using PROSP file values"
    }

    return (
        <Tooltip title={tooltipText}>
            <IconButton variant="ghost_icon" color="secondary" onClick={() => onClick(clickedElement)}>
                {isOverride ? <ExcelHideIcon size={20} /> : <Icon data={microsoft_excel} color="#007079" />}
            </IconButton>
        </Tooltip>
    )
}
