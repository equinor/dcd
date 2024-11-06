import { Chip, Tooltip, Icon } from "@equinor/eds-core-react"
import { useState } from "react"
import { Typography } from "@mui/material"
import { close } from "@equinor/eds-icons"
import { useQuery } from "@tanstack/react-query"
import { useParams } from "react-router"
import styled from "styled-components"

import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useProjectContext } from "@/Context/ProjectContext"
import { projectQueryFn, revisionQueryFn } from "@/Services/QueryFunctions"
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
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId

    const { data: revisionApiData } = useQuery({
        queryKey: ["revisionApiData", revisionId],
        queryFn: () => revisionQueryFn(projectId, revisionId),
        enabled: !!revisionId && !!projectId,
        refetchOnWindowFocus: false, // Optional: Adjust as necessary
    })
    const { data: projectApiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const revisionDetailsList = projectApiData?.revisionsDetailsList || []
    const matchingRevision = revisionDetailsList.find(
        (revision) => revision.revisionId === revisionId,
    )
    const revisionName = revisionApiData?.revisionDetails.revisionName
    // console.log("revisiondetails ID", projectApiData?.revisionsDetailsList[1].id)

    console.log("revisionId", revisionId)

    console.log("revisionName", revisionName)

    const revName = () => (
        <Tooltip title={`View details for ${truncateText(revisionName ?? "", 120)}`}>
            <Typography
                onClick={() => setIsMenuOpen(true)}
                variant="body2"
                sx={{ textDecoration: "underline", cursor: "pointer", whiteSpace: "nowrap" }}
            >
                {truncateText(revisionName ?? "", 12)}
            </Typography>
        </Tooltip>
    )

    return (
        <>
            <Chip>
                <CloseRevision>
                    {revName()}
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
