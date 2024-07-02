import styled from "styled-components"
import { Icon, Chip, Tooltip } from "@equinor/eds-core-react"
import { useParams } from "react-router-dom"
import { useProjectContext } from "../../Context/ProjectContext"
import { PROJECT_CLASSIFICATION } from "../../Utils/constants"

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
    const { project } = useProjectContext()
    const { caseId } = useParams()

    return (
        project && !caseId
            ? (
                <SmallTooltip placement="bottom-start" title={PROJECT_CLASSIFICATION[project?.classification].description}>
                    <StyledChip
                        variant={PROJECT_CLASSIFICATION[project?.classification].color}
                        className={`ProjectClassification ${PROJECT_CLASSIFICATION[project?.classification].color}`}
                    >
                        <Icon data={PROJECT_CLASSIFICATION[project?.classification].icon} />
                        {PROJECT_CLASSIFICATION[project?.classification].label}
                    </StyledChip>
                </SmallTooltip>
            )
            : null
    )
}

export default Classification
