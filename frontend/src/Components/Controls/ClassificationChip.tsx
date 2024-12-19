import styled from "styled-components"
import { Icon, Chip, Tooltip } from "@equinor/eds-core-react"
import { useMemo } from "react"

import { PROJECT_CLASSIFICATION } from "@/Utils/constants"
import { useDataFetch } from "@/Hooks/useDataFetch"

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
    const revisionAndProjectData = useDataFetch()

    const getClassification = useMemo(() => {
        const classification = String(revisionAndProjectData?.commonProjectAndRevisionData.classification)
        return classification ? PROJECT_CLASSIFICATION[classification as unknown as number] : false
    }, [revisionAndProjectData])

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
