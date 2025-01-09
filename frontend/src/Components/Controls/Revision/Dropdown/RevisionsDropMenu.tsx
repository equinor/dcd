import React, { useEffect, useState } from "react"
import { Typography } from "@equinor/eds-core-react"
import Menu from "@mui/material/Menu"
import MenuItem from "@mui/material/MenuItem"
import styled from "styled-components"

import { formatFullDate, truncateText } from "@/Utils/common"
import { useRevisions } from "@/Hooks/useRevision"
import { useDataFetch } from "@/Hooks/useDataFetch"
import MenuControls from "./MenuControls"

interface RevisionsDropMenuProps {
    isMenuOpen: boolean
    setIsMenuOpen: (isOpen: boolean) => void
    menuAnchorEl: HTMLElement | null
    isCaseMenu: boolean
    onOpenRevisionDetails: () => void
}

interface Revision {
    revisionId: string
    name: string
    date: string
}

const StyledMenuContainer = styled.div`
    width: 300px;
    flex-direction: column;
    max-height: 500px;
    overflow-y: auto;
    display: flex;
`

const StyledMenuItem = styled(MenuItem)`
    && {
        padding: 12px 16px;
    }
`

const MenuItemContent = styled.div`
    display: flex;
    flex-direction: column;
    align-items: flex-start;

    && p {
        width: 100%;
        white-space: wrap;
    }
`

const RevisionsDropMenu: React.FC<RevisionsDropMenuProps> = ({
    isMenuOpen, setIsMenuOpen, menuAnchorEl, isCaseMenu, onOpenRevisionDetails,
}) => {
    const { navigateToRevision, disableCurrentRevision } = useRevisions()

    const revisionAndProjectData = useDataFetch()
    const { revisionDetailsList } = revisionAndProjectData
        ? (revisionAndProjectData as Components.Schemas.ProjectDataDto)
        : { revisionDetailsList: [] }

    const [revisions, setRevisions] = useState<Revision[]>([])

    useEffect(() => {
        const formattedRevisions = revisionDetailsList.map(({ revisionId, revisionName, revisionDate }: Components.Schemas.RevisionDetailsDto) => ({
            revisionId,
            name: revisionName,
            date: revisionDate,
        }))
        setRevisions(formattedRevisions)
    }, [revisionAndProjectData])

    const handleRevisionNavigation = async (revisionId: string) => {
        setIsMenuOpen(false)
        await navigateToRevision(revisionId)
    }

    return (
        <Menu
            id="revisions-menu"
            open={isMenuOpen && !!menuAnchorEl}
            anchorEl={menuAnchorEl}
            onClose={() => setIsMenuOpen(false)}
        >
            <StyledMenuContainer>
                {revisions.map(({ revisionId, name, date }) => (
                    <StyledMenuItem
                        key={revisionId}
                        onClick={() => handleRevisionNavigation(revisionId)}
                        disabled={disableCurrentRevision(revisionId)}
                    >
                        <MenuItemContent>
                            <Typography>
                                {truncateText(name, 50)}
                            </Typography>
                            <Typography variant="meta">
                                {formatFullDate(date)}
                            </Typography>
                        </MenuItemContent>
                    </StyledMenuItem>
                ))}
            </StyledMenuContainer>
            <MenuControls
                isCaseMenu={isCaseMenu}
                setIsMenuOpen={setIsMenuOpen}
                hasRevisions={revisions.length > 0}
                onOpenRevisionDetails={onOpenRevisionDetails}
            />
        </Menu>
    )
}

export default RevisionsDropMenu
