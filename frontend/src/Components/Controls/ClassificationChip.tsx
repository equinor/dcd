import styled from "styled-components"
import { Icon, Chip, Tooltip } from "@equinor/eds-core-react"
import { useQuery } from "@tanstack/react-query"

import { useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { PROJECT_CLASSIFICATION } from "@/Utils/constants"
import { projectQueryFn, revisionQueryFn } from "@/Services/QueryFunctions"
import { useProjectContext } from "@/Context/ProjectContext"
import { useMemo } from "react"

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
    const { isRevision, projectId } = useProjectContext()
    const externalId = currentContext?.externalId

    const { data: projectApiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const { data: apiRevisionData } = useQuery({
        queryKey: ["revisionApiData", revisionId],
        queryFn: () => revisionQueryFn(projectId, revisionId),
        enabled: !!projectId && !!revisionId && isRevision,
    })

    const getClassification = useMemo(() => {
        if (isRevision && apiRevisionData) {
            return PROJECT_CLASSIFICATION[apiRevisionData.commonProjectAndRevisionData.classification]
        } else if (projectApiData) {
            return PROJECT_CLASSIFICATION[projectApiData?.commonProjectAndRevisionData.classification]
        } else return false
    }, [isRevision, apiRevisionData, projectApiData])

    return (
        getClassification ? (
            <SmallTooltip placement="bottom-start" title={getClassification.description}>
                <StyledChip
                    variant={getClassification.color}
                    className={`classification ${getClassification.color}`}
                >
                    <Icon data={getClassification.icon} />
                    {getClassification.label}
                </StyledChip>
            </SmallTooltip>
        ) : null
    )
}

export default Classification
