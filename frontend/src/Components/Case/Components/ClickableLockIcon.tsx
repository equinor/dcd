import React from "react"
import { lock, lock_open } from "@equinor/eds-icons"
import { Icon } from "@equinor/eds-core-react"
import { useParams } from "react-router-dom"
import { useProjectContext } from "../../../Context/ProjectContext"

interface LockIconProps {
    clickedElement: any
    addEdit: any
}

const LockIcon: React.FC<LockIconProps> = ({
    clickedElement,
    addEdit,
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

    if (clickedElement.data?.overridable) {
        return (clickedElement.data.overrideProfile?.override) ? (
            <Icon
                data={lock_open}
                opacity={0.5}
                color="#007079"
                onClick={() => handleLockIconClick(clickedElement)}
            />
        )
            : (
                <Icon
                    data={lock}
                    color="#007079"
                    onClick={() => handleLockIconClick(clickedElement)}
                />
            )
    }
    return null
}

export default LockIcon
