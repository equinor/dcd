import React, { useEffect, useState } from "react"
import {
    Menu, Typography, Icon,
} from "@equinor/eds-core-react"
import { add, exit_to_app } from "@equinor/eds-icons"
import { useNavigate, useParams } from "react-router"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery, useQueryClient } from "@tanstack/react-query"
import { useProjectContext } from "../../Context/ProjectContext"
import { projectQueryFn } from "@/Services/QueryFunctions"
import { formatFullDate } from "@/Utils/common"
import {
    disableCurrentRevision, exitRevisionView, navigateToRevision, openRevisionModal,
} from "@/Utils/RevisionUtils"
import CreateRevisionModal from "../Modal/CreateRevisionModal"
import useEditDisabled from "@/Hooks/useEditDisabled"

type RevisionsDropMenuProps = {
    isMenuOpen: boolean
    setIsMenuOpen: (isMenuOpen: boolean) => void
    menuAnchorEl: HTMLElement | null
    setIsRevisionMenuOpen?: React.Dispatch<React.SetStateAction<boolean>>
    isCaseMenu: boolean
}

interface Revision {
    id: string
    name: string
    description: string
    date: string
}

const RevisionsDropMenu: React.FC<RevisionsDropMenuProps> = ({
    isMenuOpen, setIsMenuOpen, menuAnchorEl, isCaseMenu, setIsRevisionMenuOpen,
}) => {
    const { setIsRevision, isRevision, projectId } = useProjectContext()
    const navigate = useNavigate()
    const queryClient = useQueryClient()
    const { revisionId } = useParams()
    const { isEditDisabled } = useEditDisabled()

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

    const navToRevision = (revision: Revision) => {
        setIsMenuOpen(false)
        navigateToRevision(revision.id, setIsRevision, queryClient, externalId, navigate)
    }

    const exitRevision = () => {
        setIsMenuOpen(false)
        exitRevisionView(setIsRevision, queryClient, externalId, currentContext, navigate)
    }

    return (
        <>
            <CreateRevisionModal
                isOpen={creatingRevision}
                setCreatingRevision={setCreatingRevision}
            />
            <Menu
                id="menu-complex"
                open={isMenuOpen}
                anchorEl={menuAnchorEl}
                onClose={() => (isCaseMenu && setIsRevisionMenuOpen ? setIsRevisionMenuOpen(false) : setIsMenuOpen(false))}
                placement={isCaseMenu ? "left" : "bottom"}
            >
                {
                    revisions.map((revision) => (
                        <Menu.Item
                            onClick={() => (isCaseMenu ? navToRevision(revision) : navigateToRevision(revision.id, setIsRevision, queryClient, projectId, navigate))}
                            disabled={disableCurrentRevision(revision.id, isRevision, revisionId)}
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
                <Menu.Item
                    onClick={() => openRevisionModal(setCreatingRevision)}
                    disabled={isEditDisabled}
                >
                    <Icon data={add} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Create new revision
                    </Typography>
                </Menu.Item>
                <Menu.Item
                    onClick={() => (isCaseMenu ? exitRevision() : exitRevisionView(setIsRevision, queryClient, projectId, currentContext, navigate))}
                    disabled={!isRevision}
                >
                    <Icon data={exit_to_app} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Exit revision view
                    </Typography>
                </Menu.Item>
            </Menu>
        </>
    )
}

export default RevisionsDropMenu
