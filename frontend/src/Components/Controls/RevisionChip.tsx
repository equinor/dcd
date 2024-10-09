import { Chip, Tooltip, Icon } from "@equinor/eds-core-react"
import { useState } from "react"
import { Typography } from "@mui/material"
import { close } from "@equinor/eds-icons"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQueryClient } from "@tanstack/react-query"
import { useNavigate } from "react-router"
import RevisionDetailsModal from "./RevisionDetailsModal"
import { useProjectContext } from "../../Context/ProjectContext"
import { exitRevisionView } from "@/Utils/RevisionUtils"

const RevisionChip = () => {
    const { setIsRevision } = useProjectContext()
    const [isMenuOpen, setIsMenuOpen] = useState(false)
    const [showCloseIcon, setShowCloseIcon] = useState(false)

    const navigate = useNavigate()
    const queryClient = useQueryClient()

    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId

    const handleMouseOver = () => {
        setShowCloseIcon(true)
    }

    const handleMouseOut = () => {
        setShowCloseIcon(false)
    }

    const revisionName = () => (
        <Tooltip title="View details">
            <Typography onClick={() => setIsMenuOpen(true)} variant="body2" sx={{ textDecoration: "underline" }}>
                APx Rev 1
            </Typography>
        </Tooltip>
    )

    return (
        <>
            <Chip onMouseOver={handleMouseOver} onMouseOut={handleMouseOut}>
                {!showCloseIcon ? revisionName() : (
                    <>
                        {revisionName()}
                        <Tooltip title="Exit revision">
                            <Icon data={close} size={16} onClick={() => exitRevisionView(setIsRevision, queryClient, externalId, currentContext, navigate)} />
                        </Tooltip>
                    </>
                )}
            </Chip>
            <RevisionDetailsModal isMenuOpen={isMenuOpen} setIsMenuOpen={setIsMenuOpen} />
        </>
    )
}

export default RevisionChip
