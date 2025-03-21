import {
    Chip,
    Tooltip,
    Icon,
    Typography,
} from "@equinor/eds-core-react"
import { close } from "@equinor/eds-icons"
import { useQuery } from "@tanstack/react-query"
import { useState } from "react"
import { useParams } from "react-router"
import styled from "styled-components"

import RevisionDetailsModal from "../Revision/RevisionDetailsModal"

import { useRevisions } from "@/Hooks/useRevision"
import { revisionQueryFn } from "@/Services/QueryFunctions"
import { useProjectContext } from "@/Store/ProjectContext"
import { truncateText } from "@/Utils/FormatingUtils"

const RevisionChipContainer = styled(Chip)`
    height: auto;
    padding: 5px 12px;
`

const CloseRevision = styled.div`
    cursor: pointer;
    display: flex;
    justify-content: space-between;
    align-items: center;
`

const StyledTypography = styled(Typography)`
    text-decoration: underline;
    cursor: pointer;
    white-space: nowrap;
    font-weight: 500;
    margin-right: 5px;
`

const RevisionChip = () => {
    const { projectId } = useProjectContext()
    const { exitRevisionView } = useRevisions()
    const { revisionId } = useParams()
    const [isMenuOpen, setIsMenuOpen] = useState(false)

    const { data: revisionApiData } = useQuery({
        queryKey: ["revisionApiData", revisionId],
        queryFn: () => revisionQueryFn(projectId, revisionId),
        enabled: !!revisionId && !!projectId,
    })

    const revisionName = revisionApiData?.revisionDetails.revisionName

    const revisionNameDisplay = () => (
        <Tooltip title={`View properties for ${revisionName}`}>
            <StyledTypography onClick={() => setIsMenuOpen(true)} color="primary">
                {truncateText(revisionName ?? "", 25)}
            </StyledTypography>
        </Tooltip>
    )

    return (
        <>
            <RevisionChipContainer>
                <CloseRevision>
                    {revisionNameDisplay()}
                    <Tooltip title="Exit revision">
                        <Icon
                            data={close}
                            size={16}
                            onClick={() => exitRevisionView()}
                        />
                    </Tooltip>
                </CloseRevision>
            </RevisionChipContainer>
            <RevisionDetailsModal isMenuOpen={isMenuOpen} setIsMenuOpen={setIsMenuOpen} />
        </>
    )
}

export default RevisionChip
