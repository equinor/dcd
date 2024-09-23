import React, { useState, useEffect } from "react"
import { useNavigate, useParams, useLocation } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import {
    Button,
    Typography,
} from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useQuery } from "@tanstack/react-query"
import styled from "styled-components"
import { projectPath } from "../../Utils/common"
import { useProjectContext } from "../../Context/ProjectContext"
import { GetProjectService } from "../../Services/ProjectService"
import { useAppContext } from "../../Context/AppContext"
import CaseControls from "./CaseControls"
import WhatsNewModal from "../Modal/WhatsNewModal"
import Modal from "../Modal/Modal"
import ProjectControls from "./ProjectControls"
import { caseQueryFn } from "../../Services/QueryFunctions"

const Wrapper = styled(Grid)`
    padding-top: 20px;
    margin-bottom: 20px;
    background-color: white;
`

const Controls = () => {
    const {
        project,
        setProject,
        projectEdited,
        setProjectEdited,
    } = useProjectContext()

    const navigate = useNavigate()
    const location = useLocation()
    const { currentContext } = useModuleCurrentContext()
    const { editMode, setEditMode } = useAppContext()
    const { caseId } = useParams()
    const projectId = project?.id
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

    const { data: apiData } = useQuery({
        queryKey: ["apiData", { projectId, caseId }],
        queryFn: () => caseQueryFn(projectId, caseId),
        enabled: !!projectId && !!caseId,
    })

    const caseData = apiData?.case as Components.Schemas.CaseWithProfilesDto

    useEffect(() => {
        if (location.pathname.includes("case")) {
            setCaseLastUpdated(caseData?.modifyTime ?? "")
            setProjectLastUpdated(caseData?.modifyTime ?? "")
        } else {
            setProjectLastUpdated(caseData?.modifyTime ?? "")
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
        <Wrapper>
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

            {project && caseId ? (
                <CaseControls
                    backToProject={backToProject}
                    projectId={project.id}
                    caseId={caseId}
                    caseLastUpdated={caseLastUpdated}
                    handleEdit={handleEdit}
                />
            )
                : (
                    <ProjectControls
                        projectLastUpdated={projectLastUpdated}
                        handleEdit={handleEdit}
                        setIsCanceling={setIsCanceling}
                    />
                )}
        </Wrapper>
    )
}

export default Controls
