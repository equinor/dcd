import {
    Chip,
    Tooltip,
    Icon,
    Typography,
} from "@equinor/eds-core-react"
import { useState } from "react"
import { close } from "@equinor/eds-icons"
import { useQuery } from "@tanstack/react-query"
import { useParams } from "react-router"
import styled from "styled-components"
import { useProjectContext } from "@/Store/ProjectContext"
import { revisionQueryFn } from "@/Services/QueryFunctions"
import { truncateText } from "@/Utils/common"
import RevisionDetailsModal from "../Revision/RevisionDetailsModal"
import { useRevisions } from "@/Hooks/useRevision"

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
        enabled: !!revisionId,
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
