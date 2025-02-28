import React, { useState } from "react"
import { Button, CircularProgress, Tooltip } from "@equinor/eds-core-react"
import { useParams } from "react-router-dom"
import { ICellRendererParams } from "@ag-grid-community/core"
import { useSubmitToApi, useCaseApiData } from "@/Hooks"
import { DisabledExcelHideIcon } from "@/Media/Icons/DisabledExcelHideIcon"
import { useProjectContext } from "@/Store/ProjectContext"
import { useAppStore } from "@/Store/AppStore"
import { ITimeSeriesTableDataOverrideWithSet } from "@/Models/ITimeSeries"
import { CalculatorToggle, ExcelToggle } from "./ToggleIcons"
import { ProfileTypes } from "@/Models/enums"
import { ResourceObject } from "@/Models/Interfaces"

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
    const { projectId } = useProjectContext()
    const [sharepointId] = useState(sharepointFileId)
    const { apiQueue } = useAppStore()
    const { submitToApi } = useSubmitToApi()
    const { apiData } = useCaseApiData()

    const handleToggleClick = async (params: ICellRendererParams<ITimeSeriesTableDataOverrideWithSet>) => {
        if (!editAllowed || params?.data?.override === undefined || !caseId) {
            return
        }

        const profile = {
            ...params.data.overrideProfile,
            resourceId: params.data.resourceId,
            resourceName: params.data.resourceName,
            overridable: params.data.overridable,
            editable: params.data.editable,
        }

        const resourceKey = profile.resourceName.charAt(0).toLowerCase() + profile.resourceName.slice(1)

        const overrideData = apiData![resourceKey as keyof typeof apiData] as Record<string, any>

        const createOverrideDto = (data: Record<string, any> | undefined, profileType: string, override: boolean) => ({
            values: data?.values ?? [],
            startYear: data?.startYear ?? 0,
            profileType,
            override,
        })

        await submitToApi({
            projectId,
            caseId,
            resourceName: "caseProfiles",
            resourceObject: {
                timeSeries: [],
                overrideTimeSeries: [
                    createOverrideDto(overrideData, profile.resourceName, !profile.override),
                ],
            } as unknown as ResourceObject,
        })

        params.api.redrawRows()
        params.api.refreshCells()
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
