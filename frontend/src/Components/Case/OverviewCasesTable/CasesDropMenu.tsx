import {
    Menu, Typography, Icon, Button,
} from "@equinor/eds-core-react"
import { useNavigate } from "react-router-dom"
import {
    bookmark_filled,
    bookmark_outlined,
    delete_to_trash,
    edit,
    folder,
    library_add,
    archive,
    unarchive,
    history,
} from "@equinor/eds-icons"
import { useMemo, useState } from "react"
import { useQuery, useQueryClient } from "@tanstack/react-query"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"

import { deleteCase, duplicateCase, setCaseAsReference } from "@/Utils/CaseController"
import { ResourceObject } from "@/Models/Interfaces"
import { useSubmitToApi } from "@/Hooks/UseSubmitToApi"
import { projectQueryFn } from "@/Services/QueryFunctions"
import useEditProject from "@/Hooks/useEditProject"
import Modal from "../../Modal/Modal"
import RevisionsDropMenu from "@/Components/Controls/RevisionsDropMenu"

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
    const navigate = useNavigate()
    const { updateCase } = useSubmitToApi()
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId
    const [isRevisionMenuOpen, setIsRevisionMenuOpen] = useState<boolean>(false)
    const [revisionMenuAnchorEl, setRevisionMenuAnchorEl] = useState<any | null>(null)

    const { data: projectData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const [confirmDelete, setConfirmDelete] = useState(false)

    const selectedCase = useMemo(() => projectData?.cases.find((c) => c.id === selectedCaseId), [projectData, selectedCaseId])

    if (!projectData) { return <p>project not found</p> }

    const openCase = async () => {
        try {
            if (selectedCaseId) {
                navigate(`case/${selectedCaseId}`)
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const handleDelete = () => {
        setConfirmDelete(false)

        if (selectedCaseId) {
            deleteCase(selectedCaseId, projectData, addProjectEdit)
        }
    }

    const archiveCase = async (isArchived: boolean) => {
        if (!selectedCase || selectedCaseId === undefined || !projectData.id) { return }
        const newResourceObject = { ...selectedCase, archived: isArchived } as ResourceObject
        const result = await updateCase({ projectId: projectData.id, caseId: selectedCaseId, resourceObject: newResourceObject })
        if (result) {
            queryClient.invalidateQueries(
                { queryKey: ["projectApiData", projectData.fusionProjectId] },
            )
        }
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
                    disabled={selectedCase?.archived}
                    onClick={() => (projectData && selectedCaseId) && duplicateCase(selectedCaseId, projectData, addProjectEdit)}
                >
                    <Icon data={library_add} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Duplicate
                    </Typography>
                </Menu.Item>
                {selectedCase?.archived
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
                <Menu.Item
                    disabled={selectedCase?.archived}
                    onClick={() => editCase()}
                >
                    <Icon data={edit} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Edit
                    </Typography>
                </Menu.Item>
                <Menu.Item
                    onClick={() => setConfirmDelete(true)}
                >
                    <Icon data={delete_to_trash} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Delete
                    </Typography>
                </Menu.Item>
                {projectData.referenceCaseId === selectedCaseId
                    ? (
                        <Menu.Item
                            disabled={selectedCase?.archived}
                            onClick={() => projectData && setCaseAsReference(selectedCaseId, projectData, addProjectEdit)}
                        >
                            <Icon data={bookmark_outlined} size={16} />
                            <Typography group="navigation" variant="menu_title" as="span">
                                Remove as reference case
                            </Typography>
                        </Menu.Item>
                    )
                    : (
                        <Menu.Item
                            disabled={selectedCase?.archived}
                            onClick={() => projectData && setCaseAsReference(selectedCaseId, projectData, addProjectEdit)}
                        >
                            <Icon data={bookmark_filled} size={16} />
                            <Typography group="navigation" variant="menu_title" as="span">
                                Set as reference case
                            </Typography>
                        </Menu.Item>
                    )}
                {/* Uncomment to show project revisions button */}
                {/* <Menu.Item
                    ref={setRevisionMenuAnchorEl}
                    onMouseEnter={() => setIsRevisionMenuOpen(!isRevisionMenuOpen)}
                >
                    <Icon data={history} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Project revisions
                    </Typography>
                </Menu.Item>
                <RevisionsDropMenu
                    isMenuOpen={isRevisionMenuOpen}
                    setIsMenuOpen={setIsMenuOpen}
                    setIsRevisionMenuOpen={setIsRevisionMenuOpen}
                    menuAnchorEl={revisionMenuAnchorEl}
                    isCaseMenu
                /> */}
            </Menu>
        </>
    )
}

export default CasesDropMenu
