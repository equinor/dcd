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
    addEdit: any
    isProsp?: boolean
    sharepointFileId?: string
    editAllowed: boolean
}

const CalculationSourceToggle: React.FC<CalculationSourceToggleProps> = ({
    editAllowed,
    clickedElement,
    addEdit,
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
        const nonOverrideResourceKey = resourceKey.replace("Override", "")

        const overrideData = apiData![resourceKey as keyof typeof apiData] as Record<string, any>
        const nonOverrideData = apiData![nonOverrideResourceKey as keyof typeof apiData] as Record<string, any>

        // cleans data by removing updatedUtc
        const cleanData = (data: Record<string, any> | undefined): Record<string, any> => {
            if (!data) { return {} }
            const cleaned = { ...data }
            if ("updatedUtc" in cleaned) {
                delete cleaned.updatedUtc
            }
            return cleaned
        }

        const cleanedNonOverrideData = cleanData(nonOverrideData)
        const cleanedOverrideData = cleanData(overrideData)

        cleanedNonOverrideData.profileType = profile.resourceName

        const newOverrideValue = !profile.override

        await submitToApi({
            projectId,
            caseId,
            resourceName: "caseProfiles",
            resourceObject: {
                timeSeries: [cleanedNonOverrideData],
                overrideTimeSeries: [
                    {
                        ...cleanedOverrideData,
                        profileType: profile.resourceName,
                        override: newOverrideValue,
                    },
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
