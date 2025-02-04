import React, { useState } from "react"
import { Button, CircularProgress, Tooltip } from "@equinor/eds-core-react"
import { useParams } from "react-router-dom"
import { ICellRendererParams } from "@ag-grid-community/core"
import { DisabledExcelHideIcon } from "@/Media/Icons/DisabledExcelHideIcon"
import { useProjectContext } from "@/Context/ProjectContext"
import { useAppContext } from "@/Context/AppContext"
import { ProfileNames } from "@/Models/Interfaces"
import { ITimeSeriesTableDataOverrideWithSet } from "@/Models/ITimeSeries"
import { CalculatorToggle, ExcelToggle } from "./ToggleIcons"

interface CalculationSourceToggleProps {
    clickedElement: ICellRendererParams<ITimeSeriesTableDataOverrideWithSet>
    addEdit: any
    isProsp?: boolean
    sharepointFileId?: string
    editMode: boolean
}

const CalculationSourceToggle: React.FC<CalculationSourceToggleProps> = ({
    editMode,
    clickedElement,
    addEdit,
    isProsp,
    sharepointFileId,
}) => {
    const { caseId } = useParams()
    const { projectId } = useProjectContext()
    const [sharepointId] = useState(sharepointFileId)
    const { apiQueue } = useAppContext()

    const handleToggleClick = (params: ICellRendererParams<ITimeSeriesTableDataOverrideWithSet>) => {

        if (!editMode) {
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

            const newResourceObject = structuredClone(profile)
            newResourceObject.override = !profile.override

            addEdit({
                inputLabel: params.data.profileName,
                projectId,
                resourceName: profile.resourceName,
                resourcePropertyKey: "override",
                caseId,
                resourceId: profile.resourceId,
                newResourceObject,
                previousResourceObject: profile,
                resourceProfileId: profile.id,
            })

            params.api.redrawRows()
            params.api.refreshCells()
        }
    }

    if (apiQueue.find((item) => item.resourceName === clickedElement.data?.resourceName as ProfileNames)) {
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
        <ExcelToggle clickedElement={clickedElement} onClick={handleToggleClick} editMode={editMode} />
    ) : (
        <CalculatorToggle clickedElement={clickedElement} onClick={handleToggleClick} editMode={editMode} />
    )
}

export default CalculationSourceToggle
