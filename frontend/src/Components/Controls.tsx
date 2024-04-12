import {
    useState, useRef, useEffect, SetStateAction,
} from "react"
import { useNavigate } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import {
    Icon, Typography, Button, Progress, Input,
} from "@equinor/eds-core-react"
import {
    save, edit, keyboard_tab, more_vertical, arrow_back,
} from "@equinor/eds-icons"
import Grid from "@mui/material/Grid"
import { projectPath } from "../Utils/common"
import { useProjectContext } from "../Context/ProjectContext"
import { useCaseContext } from "../Context/CaseContext"
import { useModalContext } from "../Context/ModalContext"
import CaseDropMenu from "../Components/Case/Components/CaseDropMenu"
import { GetProjectService } from "../Services/ProjectService"
import { useAppContext } from "../Context/AppContext"

const Controls = () => {
    const navigate = useNavigate()

    const { currentContext } = useModuleCurrentContext()

    const { isSaving, editMode, setEditMode } = useAppContext()
    const {
        project, setProject, projectEdited, setProjectEdited,
    } = useProjectContext()
    const {
        projectCase, setProjectCase, renameProjectCase, setRenameProjectCase, projectCaseEdited, setProjectCaseEdited, setSaveProjectCase,
    } = useCaseContext()
    const { setTechnicalModalIsOpen } = useModalContext()
    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLButtonElement | null>(null)
    const nameInput = useRef<any>(null)
    const [updatedName, setUpdatedName] = useState<string>("")

    useEffect(() => {
        if (nameInput.current !== undefined && renameProjectCase && projectCase) {
            nameInput.current.focus()
        }
    }, [renameProjectCase, nameInput])

    useEffect(() => {
        if (projectCase) {
            const updatedCase = { ...projectCase }
            updatedCase.name = updatedName
            setProjectCaseEdited(updatedCase)
        }
    }, [updatedName])

    const handleCancel = async () => {
        setRenameProjectCase(false)
        setEditMode(false)
        setProjectEdited(undefined)
        setProjectCaseEdited(undefined)
    }

    const handleProjectSave = async () => {
        if (project && projectEdited) {
            const updatedProject = { ...projectEdited }
            const result = await (await GetProjectService()).updateProject(project.id, updatedProject)
            setProject(result)
            setProjectEdited(undefined)
            setEditMode(false)
            return result
        }
        return null
    }

    const handleProjectEdit = async () => {
        setProjectEdited(project)
        setEditMode(true)
    }

    const handleCaseEdit = async () => {
        setRenameProjectCase(true)
        setEditMode(true)
    }

    const handleCaseSave = async () => {
        setProjectCase(projectCaseEdited)
        setSaveProjectCase(true)
        handleCancel()
    }

    const backToProject = async () => {
        handleCancel()
        setProjectCase(undefined)
        navigate(projectPath(currentContext?.id!))
    }

    const handleEdit = () => {
        if (projectCaseEdited) {
            handleCaseSave()
        } else if (projectEdited) {
            handleProjectSave()
        } else if (projectCase) {
            handleCaseEdit()
        } else {
            handleProjectEdit()
        }
    }

    return (
        <Grid container spacing={1} justifyContent="space-between" alignItems="center">

            {projectCase
                && (
                    <Grid item xs={0}>
                        <Button
                            onClick={backToProject}
                            variant="ghost_icon"
                        >
                            <Icon data={arrow_back} />
                        </Button>
                    </Grid>
                )}
            <Grid item xs>
                {renameProjectCase
                    ? (
                        <Input
                            ref={nameInput}
                            type="text"
                            defaultValue={projectCase && projectCase.name}
                            onChange={(e: { target: { value: SetStateAction<string> } }) => setUpdatedName(e.target.value)}
                        />
                    )
                    : <Typography variant="h4">{projectCase ? projectCase.name : currentContext?.title}</Typography>}
            </Grid>
            <Grid item xs container spacing={1} alignItems="center" justifyContent="flex-end">
                {editMode
                    && (
                        <Grid item>
                            <Button variant="outlined" onClick={handleCancel}>Cancel</Button>
                        </Grid>
                    )}
                <Grid item>
                    <Button onClick={handleEdit} variant={editMode ? "outlined" : "contained"}>
                        {isSaving
                            ? <Progress.Dots />
                            : (
                                <>
                                    {editMode ? "Save and close edit mode" : "Edit"}
                                    {" "}
                                    {!editMode && projectCase && "case"}
                                    {!editMode && !projectCase && "project"}
                                    <Icon data={editMode ? save : edit} />
                                </>
                            )}
                    </Button>
                </Grid>
                <Grid item>
                    <Button
                        onClick={() => setTechnicalModalIsOpen(true)}
                        variant="outlined"
                    >
                        <Icon data={keyboard_tab} />
                        {`${editMode ? "Edit" : "Open"} technical input`}
                    </Button>
                </Grid>
            </Grid>

            {projectCase
                && (
                    <Grid item>
                        <Button
                            variant="ghost_icon"
                            aria-label="case menu"
                            ref={setMenuAnchorEl}
                            onClick={() => setIsMenuOpen(!isMenuOpen)}
                        >
                            <Icon data={more_vertical} />
                        </Button>
                        <CaseDropMenu
                            setRenameProjectCase={setRenameProjectCase}
                            isMenuOpen={isMenuOpen}
                            setIsMenuOpen={setIsMenuOpen}
                            menuAnchorEl={menuAnchorEl}
                            projectCase={projectCase}
                        />
                    </Grid>
                )}
        </Grid>
    )
}

export default Controls
