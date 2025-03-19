import styled from "styled-components"
import { Icon, Chip, Tooltip } from "@equinor/eds-core-react"
import { useMemo } from "react"

import { PROJECT_CLASSIFICATION } from "@/Utils/Config/constants"
import { useDataFetch } from "@/Hooks"

const StyledChip = styled(Chip)`
    border-width: 0;
    font-size: 16px;
    height: auto;
    padding: 5px 12px;
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

const Classification = () => {
    const revisionAndProjectData = useDataFetch()

    const getClassification = useMemo(() => {
        const classification = String(revisionAndProjectData?.commonProjectAndRevisionData.classification)
        return classification ? PROJECT_CLASSIFICATION[classification as unknown as number] : false
    }, [revisionAndProjectData])

    return (
        getClassification ? (
            <Tooltip placement="bottom-start" title={getClassification.description}>
                <StyledChip
                    variant={getClassification.color}
                    className={`classification ${getClassification.color}`}
                >
                    <Icon data={getClassification.icon} />
                    {getClassification.label}
                </StyledChip>
            </Tooltip>
        ) : null
    )
}

export default Classification
