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
import { useQuery, useQueryClient } from "react-query"

import { useProjectContext } from "@/Context/ProjectContext"
import { deleteCase, duplicateCase, setCaseAsReference } from "@/Utils/CaseController"
import { useModalContext } from "@/Context/ModalContext"
import Modal from "../../Modal/Modal"
import { ResourceObject } from "@/Models/Interfaces"
import { useSubmitToApi } from "@/Hooks/UseSubmitToApi"

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
    const queryClient = useQueryClient()
    const navigate = useNavigate()
    const {
        project,
        setProject,
    } = useProjectContext()
    const { updateCase } = useSubmitToApi()
    const projectId = project?.id || null

    const { addNewCase } = useModalContext()
    const [confirmDelete, setConfirmDelete] = useState(false)

    const deleteAndGoToProject = async () => {
        if (!caseId || !project) { return }

        if (await deleteCase(caseId, project, setProject)) {
            if (project.fusionProjectId) { navigate(`/${project.fusionProjectId}`) }
        }
    }

    const { data: apiData } = useQuery<Components.Schemas.CaseWithAssetsDto | undefined>(
        ["apiData", { projectId, caseId }],
        () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        {
            enabled: !!projectId && !!caseId,
            initialData: () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        },
    )

    const caseData = apiData?.case

    const archiveCase = async (isArchived: boolean) => {
        if(caseData === undefined && projectId === undefined || projectId === null) { return }
        const newResourceObject = { ...caseData, archived: isArchived } as ResourceObject
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
                <Menu.Item disabled={isArchived} onClick={() => project && duplicateCase(caseId, project, setProject)}>
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
                <Menu.Item onClick={() => project && setConfirmDelete(true)}>
                    <Icon data={delete_to_trash} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Delete
                    </Typography>
                </Menu.Item>
                {project?.referenceCaseId === caseId
                    ? (
                        <Menu.Item
                            disabled={isArchived}
                            onClick={() => project && setCaseAsReference(caseId, project, setProject)}
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
                            onClick={() => project && setCaseAsReference(caseId, project, setProject)}
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
