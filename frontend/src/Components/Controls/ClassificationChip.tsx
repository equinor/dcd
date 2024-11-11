import styled from "styled-components"
import { Icon, Chip, Tooltip } from "@equinor/eds-core-react"
import { useQuery } from "@tanstack/react-query"

import { useParams } from "react-router"
import { PROJECT_CLASSIFICATION } from "@/Utils/constants"
import { projectQueryFn, revisionQueryFn } from "@/Services/QueryFunctions"
import { useProjectContext } from "@/Context/ProjectContext"

const StyledChip = styled(Chip)`
    border-width: 0;
    font-size: 1rem;
    line-height: 1.8rem;
    height: auto;
    padding: 0 0.7rem 0 0.5rem;
    cursor: help;
    &.active {
        background-color: #a1daa01a;
        color: #358132;
        svg {
            fill: #358132;
        }
    }
    &.error {
        background-color: #FF66701A;
        color: #B30D2F;
        svg {
            fill: #B30D2F;
        }
    }
`

const SmallTooltip = styled(Tooltip)`
    white-space: pre-wrap !important;
    max-width: 300px !important;
    font-size: 1rem !important;
    text-align: center !important;
    div[class*="Arrow"] {
        top: -10px !important;
    }
`

const Classification = () => {
    const { projectId } = useProjectContext()

    const { revisionId } = useParams()
    const { data: projectApiData } = useQuery({
        queryKey: ["projectApiData", projectId],
        queryFn: () => projectQueryFn(projectId),
        enabled: !!projectId,
    })

    const revisionClassification = projectApiData?.revisionsDetailsList?.find(
        (revision) => revision.revisionId === revisionId,
    )?.classification

    const revisionClassificationNumber = Object.keys(PROJECT_CLASSIFICATION).find(
        (key) => PROJECT_CLASSIFICATION[Number(key)].label === revisionClassification,
    )

    const selectedClassification = revisionClassificationNumber !== undefined
        ? parseInt(revisionClassificationNumber, 10)
        : projectApiData?.classification

    const classification = selectedClassification !== undefined
        ? PROJECT_CLASSIFICATION[selectedClassification]
        : undefined

    return (
        classification ? (
            <SmallTooltip placement="bottom-start" title={classification.description}>
                <StyledChip
                    variant={classification.color}
                    className={`classification ${classification.color}`}
                >
                    <Icon data={classification.icon} />
                    {classification.label}
                </StyledChip>
            </SmallTooltip>
        ) : null
    )
}

export default Classification
