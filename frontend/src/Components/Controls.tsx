import { useState, useRef } from "react"
import { useNavigate } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import {
    Icon,
    Typography,
    Button,
    Progress,
    Input,
    Chip,
    Tooltip,
} from "@equinor/eds-core-react"
import {
    save,
    edit,
    keyboard_tab,
    more_vertical,
    arrow_back,
} from "@equinor/eds-icons"
import Grid from "@mui/material/Grid"
import { projectPath } from "../Utils/common"
import { useProjectContext } from "../Context/ProjectContext"
import { useCaseContext } from "../Context/CaseContext"
import { useModalContext } from "../Context/ModalContext"
import CaseDropMenu from "../Components/Case/Components/CaseDropMenu"
import { GetProjectService } from "../Services/ProjectService"
import { useAppContext } from "../Context/AppContext"
import useDataEdits from "../Hooks/useDataEdits"
import EditsSideBar from "./EditTracker/EditsSideBar"
import { PROJECT_CLASSIFICATION } from "../Utils/constants"

const Controls = () => {
    const navigate = useNavigate()

    const { addEdit } = useDataEdits()
    const { setTechnicalModalIsOpen } = useModalContext()
    const { currentContext } = useModuleCurrentContext()
    const { isSaving, editMode, setEditMode } = useAppContext()
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

    const nameInput = useRef<any>(null)

    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLButtonElement | null>(null)

    const handleCaseNameChange = (name: string) => {
        if (projectCase) {
            const newCase = {
                ...projectCase,
            }
            addEdit(name, newCase.name, "name", "name", "case", newCase.id)
            newCase.name = name
            setProjectCaseEdited(newCase)
        }
    }

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
            {projectCase && (
                <Grid item xs={0}>
                    <Button
                        onClick={backToProject}
                        variant="ghost_icon"
                    >
                        <Icon data={arrow_back} />
                    </Button>
                </Grid>
            )}

            <Grid item xs display="flex" alignItems="center" gap={1}>
                {editMode && projectCase
                    ? (
                        <Input // todo: should not be allowed to be empty
                            ref={nameInput}
                            type="text"
                            defaultValue={projectCase && projectCase.name}
                            onBlur={() => handleCaseNameChange(nameInput.current.value)}
                        />
                    )
                    : (
                        <>
                            <Typography variant="h4">
                                {projectCase ? projectCase.name : project?.name}
                            </Typography>
                            {project && !projectCase && (
                                <Tooltip placement="bottom-start" title={PROJECT_CLASSIFICATION[project?.classification].description}>
                                    <Chip variant={PROJECT_CLASSIFICATION[project?.classification].color}>
                                        <Icon data={PROJECT_CLASSIFICATION[project?.classification].icon} />
                                        {PROJECT_CLASSIFICATION[project?.classification].label}
                                    </Chip>
                                </Tooltip>
                            )}
                        </>
                    )}
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

            <Grid item>
                <EditsSideBar />
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
