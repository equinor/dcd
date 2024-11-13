import styled from "styled-components"
import { Icon, Chip, Tooltip } from "@equinor/eds-core-react"
import { useQuery } from "@tanstack/react-query"

import { useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { PROJECT_CLASSIFICATION } from "@/Utils/constants"
import { projectQueryFn } from "@/Services/QueryFunctions"

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
    const { revisionId } = useParams()
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId
    const { data: projectApiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })
    const revisionClassification = projectApiData?.revisionsDetailsList?.find(
        (revision) => revision.revisionId === revisionId,
    )?.classification

    const revisionClassificationNumber = Object.keys(PROJECT_CLASSIFICATION).find(
        (key) => revisionClassification !== undefined
            && PROJECT_CLASSIFICATION[Number(key)].label === PROJECT_CLASSIFICATION[revisionClassification].label,
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
