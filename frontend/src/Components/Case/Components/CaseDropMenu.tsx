import React from "react"
import { Menu, Typography, Icon } from "@equinor/eds-core-react"
import {
    add,
    library_add,
    edit,
    delete_to_trash,
    bookmark_outlined,
    bookmark_filled,
} from "@equinor/eds-icons"
import { useNavigate } from "react-router-dom"
import { useProjectContext } from "../../../Context/ProjectContext"
import { deleteCase, duplicateCase, setCaseAsReference } from "../../../Utils/CaseController"
import { useModalContext } from "../../../Context/ModalContext"

interface CaseDropMenuProps {
    isMenuOpen: boolean
    setIsMenuOpen: React.Dispatch<React.SetStateAction<boolean>>
    menuAnchorEl: HTMLElement | null
    projectCase: any
    setRenameProjectCase: React.Dispatch<React.SetStateAction<boolean>>
}

const CaseDropMenu: React.FC<CaseDropMenuProps> = ({
    isMenuOpen,
    setIsMenuOpen,
    menuAnchorEl,
    projectCase,
    setRenameProjectCase,
}) => {
    const navigate = useNavigate()
    const {
        project,
        setProject,
    } = useProjectContext()

    const { addNewCase } = useModalContext()

    const deleteAndGoToProject = async () => {
        if (!projectCase || !project) return

        if (await deleteCase(projectCase?.id, project, setProject)) {
            if (project.fusionProjectId) navigate(`/${project.fusionProjectId}`)
        }
    }

    return (
        <Menu
            id="menu-complex"
            open={isMenuOpen}
            anchorEl={menuAnchorEl}
            onClose={() => setIsMenuOpen(false)}
            placement="bottom"
        >
            <Menu.Item
                onClick={() => addNewCase()}
            >
                <Icon data={add} size={16} />
                <Typography group="navigation" variant="menu_title" as="span">
                    Add New Case
                </Typography>
            </Menu.Item>
            <Menu.Item
                onClick={() => project && duplicateCase(projectCase?.id, project, setProject)}
            >
                <Icon data={library_add} size={16} />
                <Typography group="navigation" variant="menu_title" as="span">
                    Duplicate
                </Typography>
            </Menu.Item>
            <Menu.Item
                onClick={() => setRenameProjectCase(true)}
            >
                <Icon data={edit} size={16} />
                <Typography group="navigation" variant="menu_title" as="span">
                    Rename
                </Typography>
            </Menu.Item>
            <Menu.Item
                onClick={() => project && deleteAndGoToProject()}
            >
                <Icon data={delete_to_trash} size={16} />
                <Typography group="navigation" variant="menu_title" as="span">
                    Delete
                </Typography>
            </Menu.Item>
            {project?.referenceCaseId === projectCase?.id
                ? (
                    <Menu.Item
                        onClick={() => project && setCaseAsReference(projectCase?.id, project, setProject)}
                    >
                        <Icon data={bookmark_outlined} size={16} />
                        <Typography group="navigation" variant="menu_title" as="span">
                            Remove as reference case
                        </Typography>
                    </Menu.Item>
                )
                : (
                    <Menu.Item
                        onClick={() => project && setCaseAsReference(projectCase?.id, project, setProject)}
                    >
                        <Icon data={bookmark_filled} size={16} />
                        <Typography group="navigation" variant="menu_title" as="span">
                            Set as reference case
                        </Typography>
                    </Menu.Item>
                )}
        </Menu>
    )
}

export default CaseDropMenu
