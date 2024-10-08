import React, { useState } from "react"
import { microsoft_excel } from "@equinor/eds-icons"
import { Icon, Tooltip, Button, CircularProgress } from "@equinor/eds-core-react"
import { useParams } from "react-router-dom"
import { useProjectContext } from "../../../Context/ProjectContext"

import { ExcelHideIcon } from "../../../Media/Icons/ExcelHideIcon"
import { CalculatorIcon } from "../../../Media/Icons/CalculatorIcon"
import { CalculatorHideIcon } from "../../../Media/Icons/CalculatorHideIcon"
import { DisabledExcelHideIcon } from "../../../Media/Icons/DisabledExcelHideIcon"
import { useAppContext } from "@/Context/AppContext"
import { ProfileNames, ResourceName } from "@/Models/Interfaces"

interface LockIconProps {
    clickedElement: any
    addEdit: any
    isProsp?: boolean
    sharepointFileId?: string
}

const LockIcon: React.FC<LockIconProps> = ({
    clickedElement,
    addEdit,
    isProsp,
    sharepointFileId,
}) => {
    const { caseId } = useParams()
    const { projectId } = useProjectContext()
    const [sharepointId] = useState(sharepointFileId)
    const { apiQueue } = useAppContext()

    const handleLockIconClick = (params: any) => {
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

    if (apiQueue.find((item) => item.resourceName as ProfileNames === clickedElement.data?.resourceName as ProfileNames)) {
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

    if (clickedElement.data?.overridable) {
        return (clickedElement.data.overrideProfile?.override) ? (
            <>

                {isProsp ? (
                    <Tooltip title="Show numbers from PROSP file">
                        <Button variant="ghost_icon" color="secondary" onClick={() => handleLockIconClick(clickedElement)}>
                            <ExcelHideIcon size={20} /></Button>
                    </Tooltip>
                ) : (
                    <Tooltip title="Show calculated numbers">
                        <Button variant="ghost_icon" color="secondary" onClick={() => handleLockIconClick(clickedElement)}>
                            <CalculatorHideIcon size={20} />
                        </Button>
                    </Tooltip>)}
            </>
        ) : (
            <>
                {isProsp ? (
                    <Tooltip title="Hide numbers from PROSP file">
                        <Button variant="ghost_icon" color="secondary" onClick={() => handleLockIconClick(clickedElement)}>
                            <Icon
                                data={microsoft_excel}
                                color="#007079" />
                        </Button>
                    </Tooltip>
                ) : (
                    <Tooltip title="Hide calculated numbers">
                        <Button variant="ghost_icon" color="secondary" onClick={() => handleLockIconClick(clickedElement)}>
                            <CalculatorIcon size={20} />
                        </Button>
                    </Tooltip>)}
            </>
            )
    }
    return null
}

export default LockIcon
