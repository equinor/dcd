import React from "react"
import { microsoft_excel } from "@equinor/eds-icons"
import { Icon, Tooltip, Button } from "@equinor/eds-core-react"
import { ICellRendererParams } from "@ag-grid-community/core"
import { ExcelHideIcon } from "@/Media/Icons/ExcelHideIcon"
import { CalculatorIcon } from "@/Media/Icons/CalculatorIcon"
import { CalculatorHideIcon } from "@/Media/Icons/CalculatorHideIcon"
import { ITimeSeriesTableDataOverrideWithSet } from "@/Models/ITimeSeries"

interface ToggleIconProps {
    clickedElement: ICellRendererParams<ITimeSeriesTableDataOverrideWithSet>
    onClick: (params: ICellRendererParams<ITimeSeriesTableDataOverrideWithSet>) => void
}

export const CalculatorToggle: React.FC<ToggleIconProps> = ({ clickedElement, onClick }) => {
    const isOverride = clickedElement.data?.overrideProfile?.override

    return (
        <Tooltip title={isOverride ? "Show calculated numbers" : "Hide calculated numbers"}>
            <Button variant="ghost_icon" color="secondary" onClick={() => onClick(clickedElement)}>
                {isOverride ? <CalculatorHideIcon size={20} /> : <CalculatorIcon size={20} />}
            </Button>
        </Tooltip>
    )
}

export const ExcelToggle: React.FC<ToggleIconProps> = ({ clickedElement, onClick }) => {
    const isOverride = clickedElement.data?.overrideProfile?.override

    return (
        <Tooltip title={isOverride ? "Show numbers from PROSP file" : "Hide numbers from PROSP file"}>
            <Button variant="ghost_icon" color="secondary" onClick={() => onClick(clickedElement)}>
                {isOverride ? <ExcelHideIcon size={20} /> : <Icon data={microsoft_excel} color="#007079" />}
            </Button>
        </Tooltip>
    )
}
