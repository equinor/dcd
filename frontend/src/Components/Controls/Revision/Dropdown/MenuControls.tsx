import React from "react"
import { Typography, Icon } from "@equinor/eds-core-react"
import { add, exit_to_app } from "@equinor/eds-icons"
import MenuItem from "@mui/material/MenuItem"
import styled from "styled-components"

import { useProjectContext } from "@/Context/ProjectContext"
import useEditDisabled from "@/Hooks/useEditDisabled"
import { useRevisions } from "@/Hooks/useRevision"

interface MenuControlsContainerProps {
    hasRevisions: boolean
}

const MenuControlsContainer = styled.div<MenuControlsContainerProps>`
    ${({ hasRevisions }) => hasRevisions && `
        margin-top: 10px;
        border-top: 1px solid #dfd0d0;
    `}
    display: flex;
    flex-direction: column;
    width: 100%;
`

const StyledMenuItem = styled(MenuItem)`
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
}

const MenuControls: React.FC<MenuControlsProps> = ({
    isCaseMenu,
    setIsMenuOpen,
    hasRevisions,
}) => {
    const { isRevision, setIsCreateRevisionModalOpen } = useProjectContext()
    const { exitRevisionView } = useRevisions()
    const { isEditDisabled } = useEditDisabled()

    const handleExitRevision = () => {
        if (isCaseMenu) {
            setIsMenuOpen(false)
        }
        exitRevisionView()
    }

    return (
        <MenuControlsContainer hasRevisions={hasRevisions}>
            <StyledMenuItem
                onClick={() => setIsCreateRevisionModalOpen(true)}
                disabled={isEditDisabled}
            >
                <Icon data={add} size={16} />
                <Typography>
                    Create new revision
                </Typography>
            </StyledMenuItem>
            <StyledMenuItem
                onClick={handleExitRevision}
                disabled={!isRevision}
            >
                <Icon data={exit_to_app} size={16} />
                <Typography>
                    Exit revision view
                </Typography>
            </StyledMenuItem>
        </MenuControlsContainer>
    )
}

export default MenuControls
