import React, { useEffect, useState } from "react"
import {
    Menu, Typography, Icon, Button,
} from "@equinor/eds-core-react"
import { add, exit_to_app } from "@equinor/eds-icons"
import { useLocation, useNavigate } from "react-router"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery, useQueryClient } from "@tanstack/react-query"
import { useProjectContext } from "../../Context/ProjectContext"
import Modal from "../Modal/Modal"
import { projectQueryFn } from "@/Services/QueryFunctions"
import { formatFullDate } from "@/Utils/common"
import {
    createRevision, disableCurrentRevision, exitRevisionView, navigateToRevision, openRevisionModal,
} from "@/Utils/RevisionUtils"

type RevisionsDropMenuProps = {
    isMenuOpen: boolean
    setIsMenuOpen: React.Dispatch<React.SetStateAction<boolean>>
    menuAnchorEl: HTMLElement | null
}

interface Revision {
    id: string
    name: string
    description: string
    date: string
}

const RevisionsCaseDropMenu: React.FC<RevisionsDropMenuProps> = ({ isMenuOpen, setIsMenuOpen, menuAnchorEl }) => {
    const { setIsRevision, isRevision, projectId } = useProjectContext()
    const navigate = useNavigate()
    const queryClient = useQueryClient()
    const location = useLocation()

    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId

    const [creatingRevision, setCreatingRevision] = useState(false)
    const [revisions, setRevisions] = useState<Revision[]>([])

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    useEffect(() => {
        if (apiData) {
            const revisionsResult = apiData.revisions.map((r: Components.Schemas.ProjectDto) => ({
                id: r.id,
                name: r.name,
                description: r.description,
                date: r.createDate,
            }))
            setRevisions(revisionsResult)
        }
    }, [apiData])

    return (
        <>
            <Modal
                title="Create revision"
                size="sm"
                isOpen={creatingRevision}
                content={(
                    <Typography variant="body_short">
                        Create revision
                    </Typography>
                )}
                actions={(
                    <div>
                        <Button variant="ghost" onClick={() => setCreatingRevision(false)}>Cancel</Button>
                        <Button onClick={() => createRevision(projectId, setCreatingRevision)}>Create revision</Button>
                    </div>
                )}
            />
            <Menu
                id="menu-complex"
                open={isMenuOpen}
                anchorEl={menuAnchorEl}
                onClose={() => setIsMenuOpen(false)}
                placement="left"
            >
                {
                    revisions.map((revision) => (
                        <Menu.Item
                            onClick={() => navigateToRevision(revision.id, setIsRevision, queryClient, externalId, navigate)}
                            disabled={disableCurrentRevision(revision.id, isRevision, location)}
                        >
                            <Typography group="navigation" variant="menu_title" as="span">
                                {revision.name}
                                {" "}
                                -
                                {" "}
                                {formatFullDate(revision.date)}
                            </Typography>
                        </Menu.Item>
                    ))
                }
                {!isRevision ? (
                    <Menu.Item onClick={() => openRevisionModal(setCreatingRevision)}>
                        <Icon data={add} size={16} />
                        <Typography group="navigation" variant="menu_title" as="span">
                            Create new revision
                        </Typography>
                    </Menu.Item>
                ) : (
                    <Menu.Item onClick={() => exitRevisionView(setIsRevision, queryClient, externalId, currentContext, navigate)}>
                        <Icon data={exit_to_app} size={16} />
                        <Typography group="navigation" variant="menu_title" as="span">
                            Exit revision view
                        </Typography>
                    </Menu.Item>
                )}
            </Menu>
        </>
    )
}

export default RevisionsCaseDropMenu
