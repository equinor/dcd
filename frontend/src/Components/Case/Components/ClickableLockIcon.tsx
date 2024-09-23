import React, { useState } from "react"
import { microsoft_excel } from "@equinor/eds-icons"
import { Icon, Tooltip } from "@equinor/eds-core-react"
import { useParams } from "react-router-dom"

import { useProjectContext } from "../../../Context/ProjectContext"
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
    const { project } = useProjectContext()
    const { caseId } = useParams()
    const [sharepointId] = useState(sharepointFileId)

    const handleLockIconClick = (params: any) => {
        if (params?.data?.override !== undefined && project && caseId) {
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
                projectId: project.id,
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

    if(isProsp && !sharepointId) {
        return <Tooltip title="To show numbers from PROSP, please add a PROPS file to the case."><div><DisabledExcelHideIcon size={20} /></div></Tooltip>
    }

    if (clickedElement.data?.overridable) {
        return (clickedElement.data.overrideProfile?.override) ? (
            <>

                {isProsp ? (
                    <Tooltip title="Show numbers from PROSP file">
                        <div onClick={() => handleLockIconClick(clickedElement)}>
                            <ExcelHideIcon size={20} /></div>
                    </Tooltip>) : <Tooltip title="Show Calculated Numbers"><div onClick={() => handleLockIconClick(clickedElement)}><CalculatorHideIcon size={20} /></div></Tooltip>
                }
            </>
        )
            : (
                <>
                    {isProsp ? (
                        <Tooltip title="Hide numbers from PROSP file">
                            <Icon
                                data={microsoft_excel}
                                color="#007079"
                                onClick={() => handleLockIconClick(clickedElement)}
                            /></Tooltip>) : <Tooltip title="Hide Calculated Numbers"><div onClick={() => handleLockIconClick(clickedElement)}><CalculatorIcon size={20} /></div></Tooltip>
                    }
                </>
            )
    }
    return null
}

export default LockIcon
