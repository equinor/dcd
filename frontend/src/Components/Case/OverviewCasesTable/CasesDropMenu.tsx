import { Menu, Typography, Icon } from "@equinor/eds-core-react"
import { useNavigate } from "react-router-dom"
import {
    bookmark_filled,
    bookmark_outlined,
    delete_to_trash,
    edit,
    folder,
    library_add,
} from "@equinor/eds-icons"
import { useAppContext } from "../../../Context/AppContext"
import { deleteCase, duplicateCase, setCaseAsReference } from "../../../Utils/CaseController"

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
    const { project, setProject } = useAppContext()
    if (!project) return <p>project not found</p>

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
                onClick={() => (project && selectedCaseId) && duplicateCase(selectedCaseId, project, setProject)}

            >
                <Icon data={library_add} size={16} />
                <Typography group="navigation" variant="menu_title" as="span">
                    Duplicate
                </Typography>
            </Menu.Item>
            <Menu.Item
                onClick={() => editCase()}
            >
                <Icon data={edit} size={16} />
                <Typography group="navigation" variant="menu_title" as="span">
                    Edit
                </Typography>
            </Menu.Item>
            <Menu.Item
                onClick={() => selectedCaseId && deleteCase(selectedCaseId, project, setProject)}
            >
                <Icon data={delete_to_trash} size={16} />
                <Typography group="navigation" variant="menu_title" as="span">
                    Delete
                </Typography>
            </Menu.Item>
            {project.referenceCaseId === selectedCaseId
                ? (
                    <Menu.Item
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
                        onClick={() => project && setCaseAsReference(selectedCaseId, project, setProject)}
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
