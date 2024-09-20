import React from "react"
import { lock, lock_open, microsoft_excel } from "@equinor/eds-icons"
import { Icon } from "@equinor/eds-core-react"
import { useParams } from "react-router-dom"
import { useProjectContext } from "../../../Context/ProjectContext"
import { ExcelHideIcon } from "../../../Media/Icons/ExcelHideIcon"
import { CalculatorIcon } from "../../../Media/Icons/CalculatorIcon"
import { CalculatorHideIcon } from "../../../Media/Icons/CalculatorHideIcon"


interface LockIconProps {
    clickedElement: any
    addEdit: any
    isProsp?: boolean
}

const LockIcon: React.FC<LockIconProps> = ({
    clickedElement,
    addEdit,
    isProsp
}) => {
    const { project } = useProjectContext()
    const { caseId } = useParams()

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

    //øverste er åpen lås, nederste er låst lås
    if (clickedElement.data?.overridable) {
        return (clickedElement.data.overrideProfile?.override) ? (
            <Icon
                data={lock_open}
                opacity={0.5}
                color="#007079"
                onClick={() => handleLockIconClick(clickedElement)}
            />
        )
            : (<>
                {isProsp ? (
                    <Icon
                        data={microsoft_excel}
                        color="#007079"
                        onClick={() => handleLockIconClick(clickedElement)}
                    />) : <div onClick={() => handleLockIconClick(clickedElement)}><CalculatorIcon size={20} /></div>
                }
            </>

            )
    }
    return null
}

export default LockIcon
