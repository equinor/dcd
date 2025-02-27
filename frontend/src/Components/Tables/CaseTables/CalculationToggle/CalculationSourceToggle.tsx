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
        if (!editAllowed) {
            return
        }

        if (params?.data?.override !== undefined && caseId) {
            const profile = {
                ...params.data.overrideProfile,
                resourceId: params.data.resourceId,
                resourceName: params.data.resourceName,
                overridable: params.data.overridable,
                editable: params.data.editable,
            }

            const resourceObject = structuredClone(profile)
            resourceObject.override = !profile.override
            console.log("profile:", profile)

            // addEdit({
            //     uuid: uuidv4(),
            //     projectId,
            //     resourceName: profile.resourceName,
            //     resourcePropertyKey: "override",
            //     caseId,
            //     resourceId: profile.resourceId,
            //     resourceObject,
            // })

            const resourceKey = profile.resourceName.charAt(0).toLowerCase() + profile.resourceName.slice(1)
            const nonOverrideResourceKey = resourceKey.replace("Override", "")
            const overrideData = apiData![resourceKey as keyof typeof apiData] as { override?: boolean }
            const nonOverrideData = apiData![nonOverrideResourceKey as keyof typeof apiData] as Record<string, any>

            // Remove updatedUtc key from nonOverrideData
            const cleanedNonOverrideData = nonOverrideData ? { ...nonOverrideData } : {}
            if (cleanedNonOverrideData && "updatedUtc" in cleanedNonOverrideData) {
                delete cleanedNonOverrideData.updatedUtc
            }

            // Add profileType to cleanedNonOverrideData
            cleanedNonOverrideData.profileType = profile.resourceName

            console.log("overrideData;", overrideData)
            console.log("nonoverrideData;", cleanedNonOverrideData)

            /*
            await submitToApi({
                projectId,
                caseId,
                resourceName: "caseProfiles",
                resourceObject: {
                    timeSeries: [cleanedNonOverrideData],
                    overrideTimeSeries: [
                        {
                            profileType: profile.resourceName,
                            startYear: 12,
                            values: [],
                            override: !profile.override,
                        },
                    ],
                } as unknown as ResourceObject,
            }) */

            params.api.redrawRows()
            params.api.refreshCells()
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
