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
import { useQueryClient } from "@tanstack/react-query"
import { useMemo, useState } from "react"

import BaseModal from "@/Components/Modal/BaseModal"
import { useDataFetch } from "@/Hooks"
import { useCaseMutation } from "@/Hooks/Mutations"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import useEditProject from "@/Hooks/useEditProject"
import { useAppNavigation } from "@/Hooks/useNavigate"
import { useCaseStore } from "@/Store/CaseStore"
import { deleteCase, duplicateCase, setCaseAsReference } from "@/Utils/CaseUtils"
import { caseTabNames } from "@/Utils/Config/constants"

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
    const { isEditDisabled } = useCanUserEdit()
    const revisionAndProjectData = useDataFetch()
    const { activeTabCase } = useCaseStore()
    const { updateArchived } = useCaseMutation()

    const [confirmDelete, setConfirmDelete] = useState(false)

    const selectedCase = useMemo(
        () => revisionAndProjectData?.commonProjectAndRevisionData.cases.find((c) => c.caseId === selectedCaseId),
        [revisionAndProjectData, selectedCaseId],
    )

    const openCase = async () => {
        try {
            if (selectedCaseId) {
                const currentTab = caseTabNames[activeTabCase]

                navigateToCase(selectedCaseId, currentTab)
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
        if (!selectedCase || selectedCaseId === undefined || !revisionAndProjectData?.projectId) {
            console.error("Missing required data for archiving case:", {
                selectedCase,
                selectedCaseId,
                projectId: revisionAndProjectData?.projectId,
            })

            return
        }

        try {
            await updateArchived(isArchived, selectedCaseId)

            // Invalidate the project data query to update the case list in the UI
            if (revisionAndProjectData.commonProjectAndRevisionData.fusionProjectId) {
                queryClient.invalidateQueries({
                    queryKey: ["projectApiData", revisionAndProjectData.commonProjectAndRevisionData.fusionProjectId],
                })
            }
        } catch (error) {
            console.error("Failed to archive/unarchive case:", error)
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
            <BaseModal
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
