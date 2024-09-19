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
    unarchive
} from "@equinor/eds-icons"
import { useMemo, useState } from "react"

import { deleteCase, duplicateCase, setCaseAsReference } from "@/Utils/CaseController"
import { useProjectContext } from "@/Context/ProjectContext"
import { ResourceObject } from "@/Models/Interfaces"
import { useSubmitToApi } from "@/Hooks/UseSubmitToApi"
import Modal from "../../Modal/Modal"

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
    const { project, setProject } = useProjectContext()
    const [confirmDelete, setConfirmDelete] = useState(false)
    
    const { updateCase } = useSubmitToApi()
    const projectId = project?.id || null

    if (!project) { return <p>project not found</p> }
    const selectedCase = useMemo(() => project.cases.find((c) => c.id === selectedCaseId), [project, selectedCaseId])

    const navigate = useNavigate()

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
            deleteCase(selectedCaseId, project, setProject)
        }
    }

    const archiveCase = async (isArchived: boolean) => {
        const currentCase = project.cases.find((c) => c.id === selectedCaseId)
        if(!currentCase || selectedCaseId === undefined || projectId === undefined || projectId === null) { return }
        const newResourceObject = { ...currentCase, archived: isArchived } as ResourceObject
        console.log("currentCase", currentCase)
        console.log("newResourceObject: ", newResourceObject)
        updateCase({ projectId, caseId: selectedCaseId, resourceObject: newResourceObject })
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
                    disabled={selectedCase?.archived}
                    onClick={openCase}
                >
                    <Icon data={folder} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Open
                    </Typography>
                </Menu.Item>
                <Menu.Item
                    disabled={selectedCase?.archived}
                    onClick={() => (project && selectedCaseId) && duplicateCase(selectedCaseId, project, setProject)}

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
                    ):(
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
                {project.referenceCaseId === selectedCaseId
                    ? (
                        <Menu.Item
                            disabled={selectedCase?.archived}
                            onClick={() => project && setCaseAsReference(selectedCaseId, project, setProject)}
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
                            onClick={() => project && setCaseAsReference(selectedCaseId, project, setProject)}
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

export default CasesDropMenu
