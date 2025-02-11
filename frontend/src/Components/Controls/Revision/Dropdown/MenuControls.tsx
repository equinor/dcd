import React, { useState } from "react"
import { Typography, Icon } from "@equinor/eds-core-react"
import { add, exit_to_app, gear } from "@equinor/eds-icons"
import MenuItem from "@mui/material/MenuItem"
import styled from "styled-components"

import { useProjectContext } from "@/Store/ProjectContext"
import { useRevisions } from "@/Hooks/useRevision"
import { useDataFetch } from "@/Hooks/useDataFetch"

interface MenuControlsContainerProps {
    $hasRevisions: boolean
}

const MenuControlsContainer = styled.div<MenuControlsContainerProps>`
    ${({ $hasRevisions }) => $hasRevisions && `
        margin-top: 10px;
        border-top: 1px solid #dfd0d0;
    `}
    display: flex;
    flex-direction: column;
    width: 100%;
`

const StyledMenuItem = styled(MenuItem)`
    display: flex;
    align-items: center;
    gap: 8px;

    && {
        padding: 18px 16px;
    }

    && .MuiIcon-root {
        margin-right: 8px;
    }
`

interface MenuControlsProps {
    isCaseMenu: boolean
    setIsMenuOpen: (isOpen: boolean) => void
    hasRevisions: boolean
    onOpenRevisionDetails: () => void
}

const MenuControls: React.FC<MenuControlsProps> = ({
    isCaseMenu,
    setIsMenuOpen,
    hasRevisions,
    onOpenRevisionDetails,
}) => {
    const { isRevision, setIsCreateRevisionModalOpen } = useProjectContext()
    const { exitRevisionView } = useRevisions()
    const revisionAndProjectData = useDataFetch()

    const handleExitRevision = () => {
        if (isCaseMenu) {
            setIsMenuOpen(false)
        }
        exitRevisionView()
    }

    return (
        <MenuControlsContainer $hasRevisions={hasRevisions}>
            <StyledMenuItem
                onClick={handleExitRevision}
                disabled={!isRevision}
            >
                <Icon data={exit_to_app} size={16} />
                <Typography>
                    Exit revision view
                </Typography>
            </StyledMenuItem>
            <StyledMenuItem
                onClick={onOpenRevisionDetails}
                disabled={!isRevision}
            >
                <Icon data={gear} size={16} />
                <Typography>
                    Revision Properties
                </Typography>
            </StyledMenuItem>
            <StyledMenuItem
                onClick={() => setIsCreateRevisionModalOpen(true)}
                disabled={!revisionAndProjectData?.userActions.canCreateRevision}
            >
                <Icon data={add} size={16} />
                <Typography>
                    Create new revision
                </Typography>
            </StyledMenuItem>
        </MenuControlsContainer>
    )
}

export default MenuControls
