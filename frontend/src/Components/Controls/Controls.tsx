import { useState, useEffect } from "react"
import { useNavigate, useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import {
    Icon,
    Button,
    Progress,
} from "@equinor/eds-core-react"
import {
    save,
    edit,
    keyboard_tab,
    more_vertical,

} from "@equinor/eds-icons"
import Grid from "@mui/material/Grid"
import { projectPath } from "../../Utils/common"
import { useProjectContext } from "../../Context/ProjectContext"
import { useCaseContext } from "../../Context/CaseContext"
import { useModalContext } from "../../Context/ModalContext"
import CaseDropMenu from "../Case/Components/CaseDropMenu"
import { GetProjectService } from "../../Services/ProjectService"
import { useAppContext } from "../../Context/AppContext"
import HistoryButton from "../Buttons/HistoryButton"
import UndoControls from "./UndoControls"
import CaseControls from "./CaseControls"

const Controls = () => {
    const {
        project,
        setProject,
        projectEdited,
        setProjectEdited,
    } = useProjectContext()
    const {
        projectCase,
        setProjectCase,
        projectCaseEdited,
        setProjectCaseEdited,
        setSaveProjectCase,
    } = useCaseContext()

    const navigate = useNavigate()
    const { setTechnicalModalIsOpen } = useModalContext()
    const { currentContext } = useModuleCurrentContext()
    const { isSaving, editMode, setEditMode } = useAppContext()
    const { caseId } = useParams()

    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLButtonElement | null>(null)

    const handleCancel = async () => {
        setEditMode(false)
        setProjectEdited(undefined)
        setProjectCaseEdited(undefined)
    }

    const handleProjectSave = async () => {
        if (project && projectEdited) {
            const updatedProject = {
                ...projectEdited,
            }
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
            // handleCaseSave() no longer needed with autosave
            handleCancel()
        } else if (projectEdited) {
            handleProjectSave()
        } else if (projectCase) {
            handleCaseEdit()
        } else {
            handleProjectEdit()
        }
    }

    // goes out of edit mode if case changes
    useEffect(() => {
        handleCancel()
    }, [caseId])

    return (
        <Grid container spacing={1} justifyContent="space-between" alignItems="center">
            {project && caseId && (
                <CaseControls
                    backToProject={backToProject}
                    projectId={project?.id}
                    caseId={caseId}
                />
            )}
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
                                    {editMode ? "Close edit mode" : "Edit"}
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
            <Grid item>
                <UndoControls />
            </Grid>
            <Grid item>
                <HistoryButton size={24} />
            </Grid>
            {projectCase && (
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
