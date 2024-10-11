import { Chip, Tooltip, Icon } from "@equinor/eds-core-react"
import { useState } from "react"
import { Typography } from "@mui/material"
import { close } from "@equinor/eds-icons"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery, useQueryClient } from "@tanstack/react-query"
import { useNavigate, useParams } from "react-router"
import RevisionDetailsModal from "./RevisionDetailsModal"
import { useProjectContext } from "../../Context/ProjectContext"
import { exitRevisionView } from "@/Utils/RevisionUtils"
import { revisionQueryFn } from "@/Services/QueryFunctions"

const RevisionChip = () => {
    const { setIsRevision, projectId } = useProjectContext()
    const [isMenuOpen, setIsMenuOpen] = useState(false)
    const [showCloseIcon, setShowCloseIcon] = useState(false)

    const navigate = useNavigate()
    const queryClient = useQueryClient()

    const { revisionId } = useParams()

    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId

    const { data: revisionData } = useQuery({
        queryKey: ["revisionApiData", revisionId],
        queryFn: () => revisionQueryFn(projectId, revisionId),
        enabled: !!revisionId,
    })

    const handleMouseOver = () => {
        setShowCloseIcon(true)
    }

    const handleMouseOut = () => {
        setShowCloseIcon(false)
    }

    const revisionName = () => (
        <Tooltip title="View details">
            <Typography
                onClick={() => setIsMenuOpen(true)}
                variant="body2"
                sx={{ textDecoration: "underline" }}
            >
                {revisionData?.name}
            </Typography>
        </Tooltip>
    )

    const toggleChipBackgroundColor = () => {
        if (showCloseIcon) {
            return "#f7f7f7"
        }
        return "white"
    }

    return (
        <>
            <Chip
                onMouseOver={handleMouseOver}
                onMouseOut={handleMouseOut}
                style={{ backgroundColor: toggleChipBackgroundColor() }}
            >
                {!showCloseIcon ? revisionName() : (
                    <>
                        {revisionName()}
                        <Tooltip title="Exit revision">
                            <Icon
                                data={close}
                                size={16}
                                onClick={() => exitRevisionView(setIsRevision, queryClient, externalId, currentContext, navigate)}
                            />
                        </Tooltip>
                    </>
                )}
            </Chip>
            <RevisionDetailsModal isMenuOpen={isMenuOpen} setIsMenuOpen={setIsMenuOpen} />
        </>
    )
}

export default RevisionChip
