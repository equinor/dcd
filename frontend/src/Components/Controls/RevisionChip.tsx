import styled from "styled-components"
import { Chip, Tooltip, Icon } from "@equinor/eds-core-react"
import { useState } from "react"
import { Typography } from "@mui/material"
import { close } from "@equinor/eds-icons"
import RevisionDetailsModal from "./RevisionDetailsModal"
import { useProjectContext } from "../../Context/ProjectContext"

const StyledChip = styled(Chip)`
    border-width: 0;
    font-size: 1rem;
    line-height: 1.8rem;
    height: auto;
    padding: 0 0.7rem 0 0.5rem;
    cursor: help;
    color: #797979;
    svg {
        fill: #358132;
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

const RevisionChip = () => {
    const { setIsRevision } = useProjectContext()
    const [isMenuOpen, setIsMenuOpen] = useState(false)

    return (
        <>
            <SmallTooltip placement="bottom-start" title="View details">
                <StyledChip
                    variant="default"

                >
                    <Typography onClick={() => setIsMenuOpen(true)} variant="body2">
                        APx Rev 1
                    </Typography>
                    <Icon data={close} onClick={() => setIsRevision(false)} />

                </StyledChip>
            </SmallTooltip>
            <RevisionDetailsModal isMenuOpen={isMenuOpen} setIsMenuOpen={setIsMenuOpen} />
        </>
    )
}

export default RevisionChip
