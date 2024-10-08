import { Chip, Tooltip, Icon } from "@equinor/eds-core-react"
import { useState } from "react"
import { Typography } from "@mui/material"
import { close } from "@equinor/eds-icons"
import RevisionDetailsModal from "./RevisionDetailsModal"
import { useProjectContext } from "../../Context/ProjectContext"

const RevisionChip = () => {
    const { setIsRevision } = useProjectContext()
    const [isMenuOpen, setIsMenuOpen] = useState(false)
    const [showCloseIcon, setShowCloseIcon] = useState(false)

    const handleMouseOver = () => {
        setShowCloseIcon(true)
    }

    const handleMouseOut = () => {
        setShowCloseIcon(false)
    }

    const revisionName = () => (
        <Tooltip title="View details">
            <Typography onClick={() => setIsMenuOpen(true)} variant="body2">
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
                            <Icon data={close} size={16} onClick={() => setIsRevision(false)} />
                        </Tooltip>
                    </>
                )}
            </Chip>
            <RevisionDetailsModal isMenuOpen={isMenuOpen} setIsMenuOpen={setIsMenuOpen} />
        </>
    )
}

export default RevisionChip
