import { Menu, Typography, Icon } from "@equinor/eds-core-react"
import { useState } from "react"
import { useNavigate } from "react-router-dom"
import {
    bookmark_filled, bookmark_outlined, delete_to_trash, edit, folder, library_add,
} from "@equinor/eds-icons"
import { useAppContext } from "../../../context/AppContext"
import { GetCaseService } from "../../../Services/CaseService"
import EditCaseModal from "../EditCaseModal"
import { EMPTY_GUID } from "../../../Utils/constants"
import { GetProjectService } from "../../../Services/ProjectService"

interface CasesDropMenuProps {
    isMenuOpen: boolean
    setIsMenuOpen: (isMenuOpen: boolean) => void
    menuAnchorEl: HTMLElement | null
    selectedCaseId: string | undefined
    setEditCaseModalIsOpen: (editCaseModalIsOpen: boolean) => void
}

const CasesDropMenu = ({
    isMenuOpen, setIsMenuOpen, menuAnchorEl, selectedCaseId, setEditCaseModalIsOpen,
}: CasesDropMenuProps): JSX.Element => {
    const { project, setProject } = useAppContext()
    if (!project) return <p>project not found</p>

    const navigate = useNavigate()

    const duplicateCase = async () => {
        try {
            if (selectedCaseId) {
                const newProject = await (await GetCaseService()).duplicateCase(project.id, selectedCaseId)
                setProject(newProject)
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const deleteCase = async () => {
        try {
            if (selectedCaseId) {
                const newProject = await (await GetCaseService()).deleteCase(project.id, selectedCaseId)
                setProject(newProject)
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const openCase = async () => {
        try {
            if (selectedCaseId) {
                navigate(`case/${selectedCaseId}`)
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const setCaseAsReference = async () => {
        try {
            const projectDto = { ...project }
            if (projectDto.referenceCaseId === selectedCaseId) {
                projectDto.referenceCaseId = EMPTY_GUID
            } else {
                projectDto.referenceCaseId = selectedCaseId ?? ""
            }
            const newProject = await (await GetProjectService()).updateProject(projectDto)
            setProject(newProject)
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    return (
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
                onClick={duplicateCase}
            >
                <Icon data={library_add} size={16} />
                <Typography group="navigation" variant="menu_title" as="span">
                    Duplicate
                </Typography>
            </Menu.Item>
            <Menu.Item
                onClick={() => setEditCaseModalIsOpen(true)}
            >
                <Icon data={edit} size={16} />
                <Typography group="navigation" variant="menu_title" as="span">
                    Edit
                </Typography>
            </Menu.Item>
            <Menu.Item
                onClick={deleteCase}
            >
                <Icon data={delete_to_trash} size={16} />
                <Typography group="navigation" variant="menu_title" as="span">
                    Delete
                </Typography>
            </Menu.Item>
            {project.referenceCaseId === selectedCaseId
                ? (
                    <Menu.Item
                        onClick={setCaseAsReference}
                    >
                        <Icon data={bookmark_outlined} size={16} />
                        <Typography group="navigation" variant="menu_title" as="span">
                            Remove as reference case
                        </Typography>
                    </Menu.Item>
                )
                : (
                    <Menu.Item
                        onClick={setCaseAsReference}
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

export default CasesDropMenu
