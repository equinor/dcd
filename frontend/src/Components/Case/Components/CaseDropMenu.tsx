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
} from "@equinor/eds-icons"
import { useNavigate } from "react-router-dom"
import { useProjectContext } from "../../../Context/ProjectContext"
import { deleteCase, duplicateCase, setCaseAsReference } from "../../../Utils/CaseController"
import { useModalContext } from "../../../Context/ModalContext"
import Modal from "../../Modal/Modal"

interface CaseDropMenuProps {
    isMenuOpen: boolean
    setIsMenuOpen: React.Dispatch<React.SetStateAction<boolean>>
    menuAnchorEl: HTMLElement | null
    caseId: string
}

const CaseDropMenu: React.FC<CaseDropMenuProps> = ({
    isMenuOpen,
    setIsMenuOpen,
    menuAnchorEl,
    caseId,
}) => {
    const navigate = useNavigate()
    const {
        project,
        setProject,
    } = useProjectContext()

    const { addNewCase } = useModalContext()
    const [confirmDelete, setConfirmDelete] = useState(false)

    const deleteAndGoToProject = async () => {
        if (!caseId || !project) { return }

        if (await deleteCase(caseId, project, setProject)) {
            if (project.fusionProjectId) { navigate(`/${project.fusionProjectId}`) }
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
                <Menu.Item onClick={() => addNewCase()}>
                    <Icon data={add} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Add New Case
                    </Typography>
                </Menu.Item>
                <Menu.Item onClick={() => project && duplicateCase(caseId, project, setProject)}>
                    <Icon data={library_add} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Duplicate
                    </Typography>
                </Menu.Item>
                <Menu.Item onClick={() => project && setConfirmDelete(true)}>
                    <Icon data={delete_to_trash} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Delete
                    </Typography>
                </Menu.Item>
                {project?.referenceCaseId === caseId
                    ? (
                        <Menu.Item
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
