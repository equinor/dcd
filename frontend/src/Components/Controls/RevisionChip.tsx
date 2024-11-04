import { Chip, Tooltip, Icon } from "@equinor/eds-core-react"
import { useState } from "react"
import { Typography } from "@mui/material"
import { close } from "@equinor/eds-icons"
import { useQuery } from "@tanstack/react-query"
import { useParams } from "react-router"
import styled from "styled-components"

import { useProjectContext } from "@/Context/ProjectContext"
import { revisionQueryFn } from "@/Services/QueryFunctions"
import { truncateText } from "@/Utils/common"
import RevisionDetailsModal from "./RevisionDetailsModal"
import { useRevisions } from "@/Hooks/useRevision"

const CloseRevision = styled.div`
    cursor: pointer;
`

const RevisionChip = () => {
    const { projectId } = useProjectContext()
    const {
        exitRevisionView,
    } = useRevisions()
    const { revisionId } = useParams()
    const [isMenuOpen, setIsMenuOpen] = useState(false)

    const { data: revisionData } = useQuery({
        queryKey: ["revisionApiData", revisionId],
        queryFn: () => revisionQueryFn(projectId, revisionId),
        enabled: !!revisionId && !!projectId,
    })

    const revisionName = () => (
        <Tooltip title={`View details for ${truncateText(revisionData?.name ?? "", 120)}`}>
            <Typography
                onClick={() => setIsMenuOpen(true)}
                variant="body2"
                sx={{ textDecoration: "underline", cursor: "pointer", whiteSpace: "nowrap" }}
            >
                {truncateText(revisionData?.name ?? "", 12)}
            </Typography>
        </Tooltip>
    )

    return (
        <>
            <Chip>
                <CloseRevision>
                    {revisionName()}
                    <Tooltip title="Exit revision">
                        <Icon
                            data={close}
                            size={16}
                            onClick={() => exitRevisionView()}
                        />
                    </Tooltip>
                </CloseRevision>
            </Chip>
            <RevisionDetailsModal isMenuOpen={isMenuOpen} setIsMenuOpen={setIsMenuOpen} />
        </>
    )
}

export default RevisionChip
