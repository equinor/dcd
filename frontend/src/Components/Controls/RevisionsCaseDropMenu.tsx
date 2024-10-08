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
import { GetProjectService } from "@/Services/ProjectService"
import { formatFullDate } from "@/Utils/common"

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

    const openRevisionModal = () => {
        console.log("Creating revision")
        setCreatingRevision(true)
    }

    const createRevision = async () => {
        const projectService = await GetProjectService()
        const newRevision = await projectService.createRevision(projectId)
        if (newRevision) {
            setCreatingRevision(false)
        }
    }

    const navigateToRevision = (revisionId: string) => {
        // disable for current revision
        setIsRevision(true)
        queryClient.invalidateQueries(
            { queryKey: ["projectApiData", externalId] },
        )
        navigate(`revision/${revisionId}`)
    }

    const exitRevisionView = () => {
        setIsRevision(false)
        queryClient.invalidateQueries(
            { queryKey: ["projectApiData", externalId] },
        )

        if (currentContext) {
            navigate(`/${currentContext.id}`)
        } else {
            navigate("/")
        }
        console.log("Exiting revision view")
    }

    const disableCurrentRevision = (revisionId: string) => {
        // this is stupid
        const currentRevisionId = location.pathname.split("/revision/")[1]
        if (isRevision && currentRevisionId === revisionId) {
            return true
        } return false
    }

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
                        <Button onClick={() => createRevision()}>Create revision</Button>
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
                        <Menu.Item onClick={() => navigateToRevision(revision.id)} disabled={disableCurrentRevision(revision.id)}>
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
                    <Menu.Item onClick={() => openRevisionModal()}>
                        <Icon data={add} size={16} />
                        <Typography group="navigation" variant="menu_title" as="span">
                            Create new revision
                        </Typography>
                    </Menu.Item>
                ) : (
                    <Menu.Item onClick={() => exitRevisionView()}>
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