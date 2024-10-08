import React, { useState } from "react"
import {
    Menu, Typography, Icon, Button,
} from "@equinor/eds-core-react"
import {
    add,
    library_add,
    delete_to_trash,
    bookmark_outlined,
    bookmark_filled,
    archive,
    unarchive,
    history,
} from "@equinor/eds-icons"
import { useNavigate, useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery, useQueryClient } from "@tanstack/react-query"

import styled from "styled-components"
import { useSubmitToApi } from "@/Hooks/UseSubmitToApi"
import { deleteCase, duplicateCase, setCaseAsReference } from "@/Utils/CaseController"
import { useModalContext } from "@/Context/ModalContext"
import { ResourceObject } from "@/Models/Interfaces"
import { caseQueryFn, projectQueryFn, revisionQueryFn } from "@/Services/QueryFunctions"
import useEditProject from "@/Hooks/useEditProject"
import { useProjectContext } from "@/Context/ProjectContext"
import Modal from "../../Modal/Modal"
import RevisionsCaseDropMenu from "@/Components/Controls/RevisionsCaseDropMenu"

interface CaseDropMenuProps {
    isMenuOpen: boolean
    setIsMenuOpen: React.Dispatch<React.SetStateAction<boolean>>
    isRevisionMenuOpen: boolean
    setIsRevisionMenuOpen: React.Dispatch<React.SetStateAction<boolean>>
    menuAnchorEl: HTMLElement | null
    revisionMenuAnchorEl: HTMLElement | null
    setRevisionMenuAnchorEl: React.Dispatch<any>
    caseId: string
    isArchived: boolean
}

const CaseDropMenu: React.FC<CaseDropMenuProps> = ({
    isMenuOpen,
    setIsMenuOpen,
    isRevisionMenuOpen,
    setIsRevisionMenuOpen,
    menuAnchorEl,
    revisionMenuAnchorEl,
    setRevisionMenuAnchorEl,
    caseId,
    isArchived,
}) => {
    const navigate = useNavigate()
    const { currentContext } = useModuleCurrentContext()
    const queryClient = useQueryClient()
    const externalId = currentContext?.externalId
    const { addNewCase } = useModalContext()
    const [confirmDelete, setConfirmDelete] = useState(false)
    const { addProjectEdit } = useEditProject()
    const { projectId, isRevision } = useProjectContext()
    const { revisionId } = useParams()
    const { updateCase } = useSubmitToApi()

    const { data: projectData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const { data: caseApiData } = useQuery({
        queryKey: ["caseApiData", projectId, caseId],
        queryFn: () => caseQueryFn(projectId, caseId),
        enabled: !!projectId && !!caseId,
    })

    const deleteAndGoToProject = async () => {
        if (!caseId || !projectData) { return }

        if (await deleteCase(caseId, projectData, addProjectEdit)) {
            if (projectData.fusionProjectId) { navigate(`/${projectData.fusionProjectId}`) }
        }
    }

    const archiveCase = async (archived: boolean) => {
        if (!caseApiData?.case || !caseId || !projectData?.id) { return }
        const newResourceObject = { ...caseApiData?.case, archived } as ResourceObject
        const result = await updateCase({ projectId: projectData.id, caseId, resourceObject: newResourceObject })
        if (result) {
            queryClient.invalidateQueries(
                { queryKey: ["projectApiData", projectData.id] },
            )
        }
    }

    return (
        <>
            <Modal
                title="Delete Case?"
                size="sm"
                isOpen={confirmDelete}
                content={(
                    <Typography variant="body_short">
                        Are you sure you want to delete this case?
                    </Typography>
                )}
                actions={(
                    <div>
                        <Button variant="ghost" onClick={() => setConfirmDelete(false)}>Cancel</Button>
                        <Button onClick={deleteAndGoToProject} color="danger"> Delete </Button>
                    </div>
                )}
            />
            <Menu
                id="menu-complex"
                open={isMenuOpen}
                anchorEl={menuAnchorEl}
                onClose={() => setIsMenuOpen(false)}
                placement="bottom"
            >
                <Menu.Item disabled={isArchived} onClick={() => addNewCase()}>
                    <Icon data={add} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Add New Case
                    </Typography>
                </Menu.Item>
                <Menu.Item disabled={isArchived} onClick={() => projectData && duplicateCase(caseId, projectData, addProjectEdit)}>
                    <Icon data={library_add} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Duplicate
                    </Typography>
                </Menu.Item>
                {isArchived
                    ? (
                        <Menu.Item onClick={() => archiveCase(false)}>
                            <Icon data={unarchive} size={16} />
                            <Typography group="navigation" variant="menu_title" as="span">
                                Restore Case
                            </Typography>
                        </Menu.Item>
                    ) : (
                        <Menu.Item onClick={() => archiveCase(true)}>
                            <Icon data={archive} size={16} />
                            <Typography group="navigation" variant="menu_title" as="span">
                                Archive Case
                            </Typography>
                        </Menu.Item>
                    )}
                <Menu.Item onClick={() => projectData && setConfirmDelete(true)}>
                    <Icon data={delete_to_trash} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Delete
                    </Typography>
                </Menu.Item>
                {projectData?.referenceCaseId === caseId
                    ? (
                        <Menu.Item
                            disabled={isArchived}
                            onClick={() => projectData && setCaseAsReference(caseId, projectData, addProjectEdit)}
                        >
                            <Icon data={bookmark_outlined} size={16} />
                            <Typography group="navigation" variant="menu_title" as="span">
                                Remove as reference case
                            </Typography>
                        </Menu.Item>
                    )
                    : (
                        <Menu.Item
                            disabled={isArchived}
                            onClick={() => projectData && setCaseAsReference(caseId, projectData, addProjectEdit)}
                        >
                            <Icon data={bookmark_filled} size={16} />
                            <Typography group="navigation" variant="menu_title" as="span">
                                Set as reference case
                            </Typography>
                        </Menu.Item>
                    )}
                <Menu.Item
                    ref={setRevisionMenuAnchorEl}
                    onMouseOver={() => setIsRevisionMenuOpen(!isRevisionMenuOpen)}
                >
                    <Icon data={history} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Project revisions
                    </Typography>
                </Menu.Item>
                <RevisionsCaseDropMenu
                    isMenuOpen={isRevisionMenuOpen}
                    setIsMenuOpen={setIsRevisionMenuOpen}
                    menuAnchorEl={revisionMenuAnchorEl}
                />
            </Menu>
        </>
    )
}

export default CaseDropMenu
