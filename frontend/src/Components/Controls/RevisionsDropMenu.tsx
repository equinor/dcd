import React, { useEffect, useState } from "react"
import {
    Menu, Typography, Icon,
} from "@equinor/eds-core-react"
import { add, exit_to_app } from "@equinor/eds-icons"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"
import styled from "styled-components"

import { useProjectContext } from "@/Context/ProjectContext"
import { projectQueryFn } from "@/Services/QueryFunctions"
import { formatFullDate, truncateText } from "@/Utils/common"
import useEditDisabled from "@/Hooks/useEditDisabled"
import { useRevisions } from "@/Hooks/useRevision"

type RevisionsDropMenuProps = {
    isMenuOpen: boolean
    setIsMenuOpen: (isMenuOpen: boolean) => void
    menuAnchorEl: HTMLElement | null
    isCaseMenu: boolean
}

interface Revision {
    revisionId: string
    name: string
    date: string
}

const StyledInnerModal = styled.div`
    flex-direction: column;
    max-height: 500px;
    overflow-y: scroll;
    display: flex;
`

const ModalControls = styled.div`
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
    const {
        navigateToRevision,
        exitRevisionView,
        disableCurrentRevision,
    } = useRevisions()
    const { isEditDisabled } = useEditDisabled()

    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId

    const [revisions, setRevisions] = useState<Revision[]>([])

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    useEffect(() => {
        if (apiData) {
            const revisionsResult = apiData.revisionDetailsList.map((r: Components.Schemas.RevisionDetailsDto) => ({
                id: r.id,
                revisionId: r.revisionId,
                name: r.revisionName,
                date: r.revisionDate,
            }))
            setRevisions(revisionsResult)
        }
    }, [apiData])

    const navToRevision = (revision: Revision) => {
        setIsMenuOpen(false)
        navigateToRevision(revision.revisionId)
    }

    const exitRevision = () => {
        setIsMenuOpen(false)
        exitRevisionView()
    }

    return (
        <Menu
            id="menu-complex"
            open={isMenuOpen}
            anchorEl={menuAnchorEl}
            onClose={() => (setIsMenuOpen(false))}
            placement={isCaseMenu ? "left" : "bottom"}
        >
            <StyledInnerModal>
                {
                    revisions.map((revision) => (
                        <Menu.Item
                            key={revision.revisionId}
                            onClick={() => (isCaseMenu ? navToRevision(revision) : navigateToRevision(revision.revisionId))}
                            disabled={disableCurrentRevision(revision.revisionId)}
                        >
                            <Typography group="navigation" variant="menu_title" as="span">
                                {truncateText(revision.name, 50)}
                                {" "}
                                -
                                {" "}
                                {formatFullDate(revision.date)}
                            </Typography>
                        </Menu.Item>
                    ))
                }
            </StyledInnerModal>
            <ModalControls>
                <Menu.Item
                    onClick={() => setIsCreateRevisionModalOpen(true)}
                    disabled={isEditDisabled}
                >
                    <Icon data={add} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Create new revision
                    </Typography>
                </Menu.Item>
                <Menu.Item
                    onClick={() => (isCaseMenu ? exitRevision() : exitRevisionView())}
                    disabled={!isRevision}
                >
                    <Icon data={exit_to_app} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Exit revision view
                    </Typography>
                </Menu.Item>
            </ModalControls>

        </Menu>
    )
}

export default RevisionsDropMenu
