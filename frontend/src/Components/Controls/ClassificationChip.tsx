import styled from "styled-components"
import { Icon, Chip, Tooltip } from "@equinor/eds-core-react"
import { useQuery } from "@tanstack/react-query"

import { PROJECT_CLASSIFICATION } from "@/Utils/constants"
import { projectQueryFn } from "@/Services/QueryFunctions"
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

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", projectId],
        queryFn: () => projectQueryFn(projectId),
        enabled: !!projectId,
    })

    return (
        apiData
            ? (
                <SmallTooltip placement="bottom-start" title={PROJECT_CLASSIFICATION[apiData.classification].description}>
                    <StyledChip
                        variant={PROJECT_CLASSIFICATION[apiData?.classification].color}
                        className={`ProjectClassification ${PROJECT_CLASSIFICATION[apiData.classification].color}`}
                    >
                        <Icon data={PROJECT_CLASSIFICATION[apiData.classification].icon} />
                        {PROJECT_CLASSIFICATION[apiData.classification].label}
                    </StyledChip>
                </SmallTooltip>
            )
            : null
    )
}

export default Classification
