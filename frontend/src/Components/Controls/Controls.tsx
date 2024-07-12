import React, { useState, useEffect } from "react"
import { useNavigate, useParams, useLocation } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import {
    Icon,
    Button,
    Progress,
    Typography,
} from "@equinor/eds-core-react"
import {
    visibility,
    edit,
    keyboard_tab,
    more_vertical,
    save,
} from "@equinor/eds-icons"
import Grid from "@mui/material/Grid"
import { useQuery, useQueryClient } from "react-query"
import { projectPath } from "../../Utils/common"
import { useProjectContext } from "../../Context/ProjectContext"
import { useModalContext } from "../../Context/ModalContext"
import CaseDropMenu from "../Case/Components/CaseDropMenu"
import { GetProjectService } from "../../Services/ProjectService"
import { useAppContext } from "../../Context/AppContext"
import UndoControls from "./UndoControls"
import CaseControls from "./CaseControls"
import WhatsNewModal from "../Modal/WhatsNewModal"
import Modal from "../Modal/Modal"
import ProjectControls from "./ProjectControls"

const Controls = () => {
    const {
        project,
        setProject,
        projectEdited,
        setProjectEdited,
    } = useProjectContext()

    const navigate = useNavigate()
    const location = useLocation()
    const { setTechnicalModalIsOpen } = useModalContext()
    const { currentContext } = useModuleCurrentContext()
    const { isSaving, editMode, setEditMode } = useAppContext()
    const { caseId } = useParams()

    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLButtonElement | null>(null)
    const [isCanceling, setIsCanceling] = useState<boolean>(false)
    const [projectLastUpdated, setProjectLastUpdated] = useState<string>("")
    const [caseLastUpdated, setCaseLastUpdated] = useState<string>("")

    const cancelEdit = async () => {
        setEditMode(false)
        setProjectEdited(undefined)
        setIsCanceling(false)
    }

    const handleProjectSave = async () => {
        if (project && projectEdited) {
            const updatedProject = { ...projectEdited }
            const result = await (await GetProjectService()).updateProject(
                project.id,
                updatedProject,
            )
            setProject(result)
            setProjectEdited(undefined)
            setEditMode(false)
            setProjectLastUpdated(result.modifyTime)
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

    const backToProject = async () => {
        cancelEdit()
        navigate(projectPath(currentContext?.id!))
    }

    const handleEdit = () => {
        if (editMode && caseId) { // user is going out of edit mode in case
            cancelEdit()
        } else if (projectEdited) { // user is saving project
            handleProjectSave()
        } else if (!editMode && caseId) { // user is going into edit mode in case
            handleCaseEdit()
        } else { // user is going into edit mode in project
            handleProjectEdit()
        }
    }

    const projectId = project?.id || null

    const queryClient = useQueryClient()
    const { data: apiData } = useQuery<
        Components.Schemas.CaseWithAssetsDto | undefined
    >(
        ["apiData", { projectId, caseId }],
        () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        {
            enabled: !!projectId && !!caseId,
            initialData: () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        },
    )

    const caseData = apiData?.case

    const formatDate = (dateString: string | undefined | null) => {
        if (!dateString) { return "" }
        const date = new Date(dateString)
        const options: Intl.DateTimeFormatOptions = {
            day: "2-digit",
            month: "short",
            year: "numeric",
            hour: "2-digit",
            minute: "2-digit",
            hour12: false,
        }
        return new Intl.DateTimeFormat("en-GB", options)
            .format(date)
            .replace(",", "")
    }

    useEffect(() => {
        if (location.pathname.includes("case")) {
            setCaseLastUpdated(caseData?.modifyTime ?? "")
        } else {
            setProjectLastUpdated(project?.modifyTime ?? "")
        }
    }, [location.pathname, caseData, project])

    useEffect(() => {
        cancelEdit()
    }, [caseId])

    useEffect(() => {
        setProjectLastUpdated(project?.modifyTime ?? "")
    }, [caseData, project])

    useEffect(() => {
        const fetchData = async () => {
            if (location.pathname.includes("case") && project?.id && caseId) {
                const projectService = await GetProjectService()
                const projectData = await projectService.getProject(project.id)
                setProject(projectData)
                setProjectLastUpdated(projectData.modifyTime)
            }
        }

        fetchData()
    }, [location.pathname, project?.id, caseId, setProject])

    return (
        <Grid container spacing={1} justifyContent="space-between" alignItems="center">
            <WhatsNewModal />
            <Modal
                isOpen={isCanceling}
                title="Are you sure you want to cancel?"
                size="sm"
                content={<Typography>All unsaved changes will be lost. This action cannot be undone.</Typography>}
                actions={(
                    <>
                        <Button onClick={() => setIsCanceling(false)} variant="outlined">
                            Continue editing
                        </Button>
                        <Button onClick={cancelEdit} variant="contained" color="danger">
                            Discard changes
                        </Button>
                    </>
                )}
            />
            {project && caseId && (
                <CaseControls
                    backToProject={backToProject}
                    projectId={project.id}
                    caseId={caseId}
                />
            )}
            {project && !caseId && <ProjectControls />}
            <Grid item xs container spacing={1} alignItems="center" justifyContent="flex-end">
                <Grid item>
                    {editMode && caseId && <UndoControls />}
                </Grid>
                {editMode && !caseId && (
                    <Grid item>
                        <Button variant="outlined" onClick={() => setIsCanceling(true)}>
                            Cancel
                        </Button>
                    </Grid>
                )}
                {!editMode && (
                    <Grid item>
                        <Typography variant="caption">
                            {caseId ? "Case last updated:" : "Project last updated:"}
                            {" "}
                            {caseId ? formatDate(caseLastUpdated) : formatDate(projectLastUpdated)}
                        </Typography>
                    </Grid>
                )}
                <Grid item>
                    <Button onClick={handleEdit} variant={editMode ? "outlined" : "contained"}>
                        {isSaving ? (
                            <Progress.Dots />
                        ) : (
                            <>
                                {editMode && (
                                    <>
                                        <Icon data={caseId ? visibility : save} />
                                        <span>{caseId ? "View" : "Save"}</span>
                                    </>
                                )}
                                {!editMode && (
                                    <>
                                        <Icon data={edit} />
                                        <span>Edit</span>
                                    </>
                                )}
                            </>
                        )}
                    </Button>
                </Grid>
                <Grid item>
                    <Button onClick={() => setTechnicalModalIsOpen(true)} variant="outlined">
                        <Icon data={keyboard_tab} />
                        {`${editMode ? "Edit" : "Open"} technical input`}
                    </Button>
                </Grid>
            </Grid>
            {caseId && (
                <Grid item>
                    <Button variant="ghost_icon" aria-label="case menu" ref={setMenuAnchorEl} onClick={() => setIsMenuOpen(!isMenuOpen)}>
                        <Icon data={more_vertical} />
                    </Button>
                    <CaseDropMenu
                        isMenuOpen={isMenuOpen}
                        setIsMenuOpen={setIsMenuOpen}
                        menuAnchorEl={menuAnchorEl}
                        caseId={caseId}
                    />
                </Grid>
            )}
        </Grid>
    )
}

export default Controls
