import React, { useState } from "react"
import { microsoft_excel } from "@equinor/eds-icons"
import { Icon, Tooltip, Button } from "@equinor/eds-core-react"
import { useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"

import { ExcelHideIcon } from "../../../Media/Icons/ExcelHideIcon"
import { CalculatorIcon } from "../../../Media/Icons/CalculatorIcon"
import { CalculatorHideIcon } from "../../../Media/Icons/CalculatorHideIcon"
import { DisabledExcelHideIcon } from "../../../Media/Icons/DisabledExcelHideIcon"


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
    sharepointFileId
}) => {
    const { caseId } = useParams()
    const [sharepointId] = useState(sharepointFileId)
    const { currentContext } = useModuleCurrentContext()
    const projectId = currentContext?.externalId

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

    if (isProsp && !sharepointId) {
        return <Tooltip title="To show numbers from PROSP, please add a PROPS file to the case."><Button variant="ghost_icon" color="secondary" disabled><DisabledExcelHideIcon size={20} /></Button></Tooltip>
    }

    if (clickedElement.data?.overridable) {
        return (clickedElement.data.overrideProfile?.override) ? (
            <>

                {isProsp ? (
                    <Tooltip title="Show numbers from PROSP file">
                        <Button variant="ghost_icon" color="secondary" onClick={() => handleLockIconClick(clickedElement)}>
                            <ExcelHideIcon size={20} /></Button>
                    </Tooltip>) : <Tooltip title="Show Calculated Numbers"><Button variant="ghost_icon" color="secondary" onClick={() => handleLockIconClick(clickedElement)}><CalculatorHideIcon size={20} /></Button></Tooltip>
                }
            </>
        )
            : (
                <>
                    {isProsp ? (
                        <Tooltip title="Hide numbers from PROSP file">
                            <Button variant="ghost_icon" color="secondary" onClick={() => handleLockIconClick(clickedElement)}>
                                <Icon
                                    data={microsoft_excel}
                                    color="#007079" />
                            </Button>
                        </Tooltip>) : <Tooltip title="Hide Calculated Numbers"><Button variant="ghost_icon" color="secondary" onClick={() => handleLockIconClick(clickedElement)}><CalculatorIcon size={20} /></Button></Tooltip>
                    }
                </>
            )
    }
    return null
}

export default LockIcon
