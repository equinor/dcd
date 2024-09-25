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
    unarchive
} from "@equinor/eds-icons"
import { useNavigate } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"

import { useSubmitToApi } from "@/Hooks/UseSubmitToApi"
import { deleteCase, duplicateCase, setCaseAsReference } from "@/Utils/CaseController"
import { useModalContext } from "@/Context/ModalContext"
import { ResourceObject } from "@/Models/Interfaces"
import { caseQueryFn, projectQueryFn } from "@/Services/QueryFunctions"
import useEditProject from "@/Hooks/useEditProject"
import Modal from "../../Modal/Modal"
import { useProjectContext } from "@/Context/ProjectContext"

interface CaseDropMenuProps {
    isMenuOpen: boolean
    setIsMenuOpen: React.Dispatch<React.SetStateAction<boolean>>
    menuAnchorEl: HTMLElement | null
    caseId: string
    isArchived: boolean
}

const CaseDropMenu: React.FC<CaseDropMenuProps> = ({
    isMenuOpen,
    setIsMenuOpen,
    menuAnchorEl,
    caseId,
    isArchived
}) => {
    const navigate = useNavigate()
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId
    const { addNewCase } = useModalContext()
    const [confirmDelete, setConfirmDelete] = useState(false)
    const { addProjectEdit } = useEditProject()
    const { projectId } = useProjectContext()
    const { updateCase } = useSubmitToApi()

    const { data: projectData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const { data: apiData } = useQuery({
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


    const archiveCase = async (isArchived: boolean) => {
        if(apiData?.case === undefined && projectId === undefined || projectId === null) { return }
        const newResourceObject = { ...apiData?.case, archived: isArchived } as ResourceObject
        updateCase({ projectId, caseId, resourceObject: newResourceObject })
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
                <Menu.Item onClick={() => addNewCase()}>
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
                    ):(
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
            </Menu>
        </>
    )
}

export default CaseDropMenu
