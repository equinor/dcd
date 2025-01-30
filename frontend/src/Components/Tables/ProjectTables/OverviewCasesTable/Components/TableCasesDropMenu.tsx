import {
    Menu, Typography, Icon, Button,
} from "@equinor/eds-core-react"
import {
    bookmark_filled,
    bookmark_outlined,
    delete_to_trash,
    edit,
    folder,
    library_add,
    archive,
    unarchive,
} from "@equinor/eds-icons"
import { useMemo, useState } from "react"
import { useQueryClient } from "@tanstack/react-query"

import { deleteCase, duplicateCase, setCaseAsReference } from "@/Utils/CaseController"
import { ResourceObject } from "@/Models/Interfaces"
import { useSubmitToApi } from "@/Hooks/UseSubmitToApi"
import { useDataFetch } from "@/Hooks/useDataFetch"
import useEditDisabled from "@/Hooks/useEditDisabled"
import useEditProject from "@/Hooks/useEditProject"
import Modal from "@/Components/Modal/Modal"
import { useAppNavigation } from "@/Hooks/useNavigate"

interface CasesDropMenuProps {
    isMenuOpen: boolean
    setIsMenuOpen: (isMenuOpen: boolean) => void
    menuAnchorEl: HTMLElement | null
    selectedCaseId: string | undefined
    editCase: () => void
}

const CasesDropMenu = ({
    isMenuOpen,
    setIsMenuOpen,
    menuAnchorEl,
    selectedCaseId,
    editCase,
}: CasesDropMenuProps): JSX.Element => {
    const queryClient = useQueryClient()
    const { addProjectEdit } = useEditProject()
    const { navigateToCase } = useAppNavigation()
    const { updateCase } = useSubmitToApi()
    const { isEditDisabled } = useEditDisabled()
    const revisionAndProjectData = useDataFetch()

    const [confirmDelete, setConfirmDelete] = useState(false)

    const selectedCase = useMemo(
        () => revisionAndProjectData?.commonProjectAndRevisionData.cases.find((c) => c.caseId === selectedCaseId),
        [revisionAndProjectData, selectedCaseId],
    )

    const openCase = async () => {
        try {
            if (selectedCaseId) {
                navigateToCase(selectedCaseId)
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const handleDelete = () => {
        if (!revisionAndProjectData) { return }
        setConfirmDelete(false)

        if (selectedCaseId) {
            deleteCase(selectedCaseId, revisionAndProjectData?.projectId, addProjectEdit)
        }
    }

    const archiveCase = async (isArchived: boolean) => {
        if (!selectedCase || selectedCaseId === undefined || !revisionAndProjectData?.projectId) { return }
        const newResourceObject = { ...selectedCase, archived: isArchived } as ResourceObject
        const result = await updateCase({ projectId: revisionAndProjectData.projectId, caseId: selectedCaseId, resourceObject: newResourceObject })
        if (result.success) {
            queryClient.invalidateQueries(
                { queryKey: ["projectApiData", revisionAndProjectData.projectId] },
            )
        }
    }

    if (!revisionAndProjectData) { return <p>project not found</p> }

    const projectData = revisionAndProjectData.dataType === "project"
        ? revisionAndProjectData as Components.Schemas.ProjectDataDto
        : null

    const handleReferenceCaseToggle = () => {
        if (!projectData || selectedCase?.archived || isEditDisabled) { return }
        setCaseAsReference(selectedCaseId, projectData, addProjectEdit)
    }
    return (
        <>
            <Modal
                title="Delete Case?"
                isOpen={confirmDelete}
                size="sm"
                content={[
                    <Typography key="modal-content" variant="body_short">
                        Are you sure you want to delete this case?
                    </Typography>,
                ]}
                actions={(
                    <div>
                        <Button variant="ghost" onClick={() => setConfirmDelete(false)}>Cancel</Button>
                        <Button onClick={handleDelete} color="danger"> Delete </Button>
                    </div>
                )}
            />

            <Menu
                id="menu-complex"
                open={isMenuOpen}
                anchorEl={menuAnchorEl}
                onClose={() => setIsMenuOpen(false)}
                placement="right"
            >
                <Menu.Item
                    onClick={openCase}
                >
                    <Icon data={folder} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Open
                    </Typography>
                </Menu.Item>
                <Menu.Item
                    disabled={selectedCase?.archived || isEditDisabled}
                    onClick={() => (revisionAndProjectData && selectedCaseId) && duplicateCase(selectedCaseId, revisionAndProjectData.projectId, addProjectEdit)}
                >
                    <Icon data={library_add} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Duplicate
                    </Typography>
                </Menu.Item>
                {selectedCase?.archived
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
                    disabled={selectedCase?.archived || isEditDisabled}
                    onClick={() => editCase()}
                >
                    <Icon data={edit} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Edit
                    </Typography>
                </Menu.Item>
                <Menu.Item
                    onClick={() => setConfirmDelete(true)}
                    disabled={isEditDisabled}
                >
                    <Icon data={delete_to_trash} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Delete
                    </Typography>
                </Menu.Item>
                <Menu.Item
                    disabled={selectedCase?.archived || isEditDisabled}
                    onClick={handleReferenceCaseToggle}
                >
                    <Icon data={revisionAndProjectData.commonProjectAndRevisionData.referenceCaseId === selectedCaseId ? bookmark_outlined : bookmark_filled} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        {revisionAndProjectData.commonProjectAndRevisionData.referenceCaseId === selectedCaseId ? "Remove as reference case" : "Set as reference case"}
                    </Typography>
                </Menu.Item>
            </Menu>
        </>
    )
}

export default CasesDropMenu
