import React, { useEffect, useState } from "react"
import { Typography, Icon } from "@equinor/eds-core-react"
import { add, exit_to_app } from "@equinor/eds-icons"
import Menu from "@mui/material/Menu"
import MenuItem from "@mui/material/MenuItem"
import styled from "styled-components"

import { useProjectContext } from "@/Context/ProjectContext"
import { formatFullDate, truncateText } from "@/Utils/common"
import useEditDisabled from "@/Hooks/useEditDisabled"
import { useRevisions } from "@/Hooks/useRevision"
import { useDataFetch } from "@/Hooks/useDataFetch"

type RevisionsDropMenuProps = {
    isMenuOpen: boolean
    setIsMenuOpen: (isOpen: boolean) => void
    menuAnchorEl: HTMLElement | null
    isCaseMenu: boolean
}

interface Revision {
    id: string
    name: string
    date: string
}

const StyledMenuContainer = styled.div`
    width: 300px;
    flex-direction: column;
    max-height: 500px;
    overflow-y: scroll;
    display: flex;
`

const MenuItemContent = styled.div`
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    padding: 5px 0;

    && p {
        width: 100%;
        white-space: wrap;
    }
`

const MenuControls = styled.div`
    margin-top: 10px;
    border-top: 1px solid #dfd0d0;
    display: flex;
    flex-direction: column;
    width: 100%;
`

const RevisionsDropMenu: React.FC<RevisionsDropMenuProps> = ({
    isMenuOpen, setIsMenuOpen, menuAnchorEl, isCaseMenu,
}) => {
    const { isRevision, setIsCreateRevisionModalOpen } = useProjectContext()
    const { navigateToRevision, exitRevisionView, disableCurrentRevision } = useRevisions()
    const { isEditDisabled } = useEditDisabled()

    const revisionAndProjectData = useDataFetch()
    const { dataType, revisionDetailsList } = revisionAndProjectData?.dataType === "project"
        ? (revisionAndProjectData as Components.Schemas.ProjectDataDto)
        : { dataType: null, revisionDetailsList: [] }

    const [revisions, setRevisions] = useState<Revision[]>([])

    useEffect(() => {
        if (dataType === "project") {
            const formattedRevisions = revisionDetailsList.map(({ revisionId, revisionName, revisionDate }: Components.Schemas.RevisionDetailsDto) => ({
                id: revisionId,
                name: revisionName,
                date: revisionDate,
            }))
            setRevisions(formattedRevisions)
        }
    }, [dataType, revisionDetailsList])

    const handleRevisionNavigation = async (revisionId: string) => {
        setIsMenuOpen(false)
        await navigateToRevision(revisionId)
    }

    const handleExitRevision = () => {
        if (isCaseMenu) {
            setIsMenuOpen(false)
        }
        exitRevisionView()
    }

    return (
        <Menu
            id="revisions-menu"
            open={isMenuOpen && !!menuAnchorEl}
            anchorEl={menuAnchorEl}
            onClose={() => setIsMenuOpen(false)}
        >
            <StyledMenuContainer>
                {revisions.map(({ id, name, date }) => (
                    <MenuItem
                        key={id}
                        onClick={() => handleRevisionNavigation(id)}
                        disabled={disableCurrentRevision(id)}
                    >
                        <MenuItemContent>
                            <Typography>
                                {truncateText(name, 50)}
                            </Typography>
                            <Typography variant="meta">
                                {formatFullDate(date)}
                            </Typography>
                        </MenuItemContent>
                    </MenuItem>

                ))}
            </StyledMenuContainer>
            <MenuControls>
                <MenuItem
                    onClick={() => setIsCreateRevisionModalOpen(true)}
                    disabled={isEditDisabled}
                >
                    <Icon data={add} size={16} />
                    <Typography>
                        Create new revision
                    </Typography>
                </MenuItem>
                <MenuItem
                    onClick={handleExitRevision}
                    disabled={!isRevision}
                >
                    <Icon data={exit_to_app} size={16} />
                    <Typography>
                        Exit revision view
                    </Typography>
                </MenuItem>
            </MenuControls>
        </Menu>
    )
}

export default RevisionsDropMenu
