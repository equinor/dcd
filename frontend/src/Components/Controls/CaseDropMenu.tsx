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
} from "@equinor/eds-icons"
import { useQuery, useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router-dom"
import { useAppNavigation } from "@/Hooks/useNavigate"

import { useSubmitToApi } from "@/Hooks/UseSubmitToApi"
import { deleteCase, duplicateCase, setCaseAsReference } from "@/Utils/CaseController"
import { useModalContext } from "@/Store/ModalContext"
import { ResourceObject } from "@/Models/Interfaces"
import { caseQueryFn } from "@/Services/QueryFunctions"
import useEditProject from "@/Hooks/useEditProject"
import { useProjectContext } from "@/Store/ProjectContext"
import Modal from "@/Components/Modal/Modal"
import useEditDisabled from "@/Hooks/useEditDisabled"
import { useDataFetch } from "@/Hooks/useDataFetch"

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
    isArchived,
}) => {
    const { navigateToProject } = useAppNavigation()
    const queryClient = useQueryClient()
    const { addNewCase } = useModalContext()
    const { addProjectEdit } = useEditProject()
    const { projectId } = useProjectContext()
    const { updateCase } = useSubmitToApi()
    const { isEditDisabled } = useEditDisabled()
    const { isRevision } = useProjectContext()
    const { revisionId } = useParams()
    const revisionAndProjectData = useDataFetch()

    const [confirmDelete, setConfirmDelete] = useState(false)

    const { data: caseApiData } = useQuery({
        queryKey: ["caseApiData", isRevision ? revisionId : projectId, caseId],
        queryFn: () => caseQueryFn(isRevision ? revisionId ?? "" : projectId, caseId),
        enabled: !!projectId && !!caseId,
    })

    const deleteAndGoToProject = async () => {
        if (!caseId || !revisionAndProjectData) { return }

        if (await deleteCase(caseId, revisionAndProjectData.projectId, addProjectEdit)) {
            if (revisionAndProjectData.commonProjectAndRevisionData.fusionProjectId) {
                const { fusionProjectId } = revisionAndProjectData.commonProjectAndRevisionData
                navigateToProject(fusionProjectId)
            }
        }
    }

    const archiveCase = async (archived: boolean) => {
        if (!caseApiData?.case || !caseId || !revisionAndProjectData?.projectId) { return }
        const newResourceObject = { ...caseApiData?.case, archived } as ResourceObject
        const result = await updateCase({ projectId: revisionAndProjectData.projectId, caseId, resourceObject: newResourceObject })
        if (result.success) {
            queryClient.invalidateQueries(
                { queryKey: ["projectApiData", revisionAndProjectData.projectId] },
            )
        }
    }

    const projectData = revisionAndProjectData?.dataType === "project"
        ? revisionAndProjectData as Components.Schemas.ProjectDataDto
        : null

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
                <Menu.Item
                    disabled={isArchived || isEditDisabled}
                    onClick={() => addNewCase()}
                >
                    <Icon data={add} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Add New Case
                    </Typography>
                </Menu.Item>
                <Menu.Item
                    disabled={isArchived || isEditDisabled}
                    onClick={() => revisionAndProjectData && duplicateCase(caseId, revisionAndProjectData.projectId, addProjectEdit)}
                >
                    <Icon data={library_add} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Duplicate
                    </Typography>
                </Menu.Item>
                {isArchived
                    ? (
                        <Menu.Item
                            onClick={() => archiveCase(false)}
                            disabled={isEditDisabled}
                        >
                            <Icon data={unarchive} size={16} />
                            <Typography group="navigation" variant="menu_title" as="span">
                                Restore Case
                            </Typography>
                        </Menu.Item>
                    ) : (
                        <Menu.Item
                            onClick={() => archiveCase(true)}
                            disabled={isEditDisabled}
                        >
                            <Icon data={archive} size={16} />
                            <Typography group="navigation" variant="menu_title" as="span">
                                Archive Case
                            </Typography>
                        </Menu.Item>
                    )}
                <Menu.Item
                    onClick={() => revisionAndProjectData && setConfirmDelete(true)}
                    disabled={isArchived || isEditDisabled}
                >
                    <Icon data={delete_to_trash} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Delete
                    </Typography>
                </Menu.Item>
                {revisionAndProjectData?.commonProjectAndRevisionData.referenceCaseId === caseId
                    ? (
                        <Menu.Item
                            disabled={isArchived || isEditDisabled}
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
                            disabled={isArchived || isEditDisabled}
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
