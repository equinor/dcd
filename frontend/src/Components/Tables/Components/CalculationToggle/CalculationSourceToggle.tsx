import React, { useState } from "react"
import { Button, CircularProgress, Tooltip } from "@equinor/eds-core-react"
import { useParams } from "react-router-dom"
import { ICellRendererParams } from "@ag-grid-community/core"
import { useCaseApiData } from "@/Hooks"
import { DisabledExcelHideIcon } from "@/Media/Icons/DisabledExcelHideIcon"
import { useAppStore } from "@/Store/AppStore"
import { ITimeSeriesTableDataOverrideWithSet } from "@/Models/ITimeSeries"
import { CalculatorToggle, ExcelToggle } from "./ToggleIcons"
import { ProfileTypes } from "@/Models/enums"
import { useTimeSeriesMutation } from "@/Hooks/Mutations"

interface CalculationSourceToggleProps {
    clickedElement: ICellRendererParams<ITimeSeriesTableDataOverrideWithSet>
    isProsp?: boolean
    sharepointFileId?: string
    editAllowed: boolean
}

const CalculationSourceToggle: React.FC<CalculationSourceToggleProps> = ({
    editAllowed,
    clickedElement,
    isProsp,
    sharepointFileId,
}) => {
    const { caseId } = useParams()
    const [sharepointId] = useState(sharepointFileId)
    const { apiQueue } = useAppStore()
    const { apiData } = useCaseApiData()
    const { updateProfileOverride } = useTimeSeriesMutation()

    const handleToggleClick = async (params: ICellRendererParams<ITimeSeriesTableDataOverrideWithSet>) => {
        if (!editAllowed || params?.data?.override === undefined || !caseId) {
            return
        }

        const resourceKey = params.data.resourceName.charAt(0).toLowerCase() + params.data.resourceName.slice(1)
        const overrideData = apiData![resourceKey as keyof typeof apiData] as Record<string, any>

        // Create the profile object with all necessary data
        const profile = {
            resourceId: params.data.resourceId,
            resourceName: params.data.resourceName,
            startYear: overrideData?.startYear ?? 0,
            values: overrideData?.values ?? [],
            override: !params.data.override, // Toggle the override state
        }

        try {
            // Use the mutation hook to update the profile override
            await updateProfileOverride(profile)

            // Update the grid
            params.api.redrawRows()
            params.api.refreshCells()
        } catch (error) {
            console.error("Failed to toggle profile override:", error)
        }
    }

    if (apiQueue.find((item) => item.resourceName === clickedElement.data?.resourceName as ProfileTypes)) {
        return (
            <Button variant="ghost_icon" color="secondary" disabled>
                <CircularProgress value={0} size={16} />
            </Button>
        )
    }

    if (isProsp && !sharepointId) {
        return (
            <Tooltip title="To show numbers from PROSP, please add a PROSP file to the case.">
                <Button variant="ghost_icon" color="secondary" disabled>
                    <DisabledExcelHideIcon size={20} />
                </Button>
            </Tooltip>
        )
    }

    if (!clickedElement.data?.overridable) {
        return null
    }

    return isProsp ? (
        <ExcelToggle clickedElement={clickedElement} onClick={handleToggleClick} editAllowed={editAllowed} />
    ) : (
        <CalculatorToggle clickedElement={clickedElement} onClick={handleToggleClick} editAllowed={editAllowed} />
    )
}

export default CalculationSourceToggle
