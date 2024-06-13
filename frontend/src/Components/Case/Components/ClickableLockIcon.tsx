import React, { Dispatch, SetStateAction } from "react"
import { lock, lock_open } from "@equinor/eds-icons"
import { Icon } from "@equinor/eds-core-react"
import { ProfileNames } from "../../../Models/Interfaces"

interface LockIconProps {
    clickedElement: any
    setOverrideModalOpen: Dispatch<SetStateAction<boolean>>
    setOverrideModalProfileName: Dispatch<SetStateAction<ProfileNames | undefined>>
    setOverrideModalProfileSet: Dispatch<SetStateAction<any | undefined>>
    setOverrideProfile: Dispatch<SetStateAction<any | undefined>>
}

const LockIcon: React.FC<LockIconProps> = ({
    clickedElement,
    setOverrideModalOpen,
    setOverrideModalProfileName,
    setOverrideModalProfileSet,
    setOverrideProfile,
}) => {
    const handleLockIconClick = (params: any) => {
        console.log("params: ", params)
        if (params?.data?.override !== undefined) {
            setOverrideModalOpen(true)
            setOverrideModalProfileName(params.data.profileName)
            setOverrideModalProfileSet(() => params.data.overrideProfileSet)
            setOverrideProfile({
                ...params.data.overrideProfile,
                resourceId: params.data.resourceId,
                resourceName: params.data.resourceName,
            })

            params.api.redrawRows()
            params.api.refreshCells()
        }
    }

    if (clickedElement.data?.overrideProfile !== undefined) {
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
    if (clickedElement.data && !clickedElement?.data?.set) {
        return <Icon data={lock} color="#007079" />
    }
    return null
}

export default LockIcon
